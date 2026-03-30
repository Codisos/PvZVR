#define SHOW_NAME_CONTAINERS

using System;
using System.Collections.Concurrent;
using UnityEngine;
#if UNITY_EDITOR
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;

[assembly: InternalsVisibleTo("ComponentNames.Editor")]
#endif

namespace Sisus.ComponentNames.EditorOnly
{
	/// <summary>
	/// Component that acts as a container for the custom name given to a component.
	/// </summary>
	[AddComponentMenu(DontShowInMenu)]
	internal class NameContainer : MonoBehaviour
	{
		#pragma warning disable CS0414
		private const string DontShowInMenu = "";

		internal static bool NowRenaming;
		internal static Component StartingToRename;
		private static readonly ConcurrentDictionary<Component, NameContainer> instances = new();

		[SerializeField]
		private string nameOverride = "";

		[SerializeField]
		private string tooltipOverride = "";

		[SerializeField]
		private Component target = null;

		#pragma warning restore CS0414

		#if UNITY_EDITOR
		internal string NameOverride
		{
			get => nameOverride;

			set
			{
				if(value == nameOverride)
				{
					return;
				}

				nameOverride = value;
				EditorUtility.SetDirty(this);
			}
		}

		internal string TooltipOverride
		{
			get => tooltipOverride;

			set
			{
				if(value == tooltipOverride)
				{
					return;
				}

				tooltipOverride = value;
				EditorUtility.SetDirty(this);
			}
		}

		private void Awake() => OnValidate();

		private void OnValidate()
		{
			if(NowRenaming)
			{
				return;
			}

			#if DEV_MODE && SHOW_NAME_CONTAINERS
			hideFlags = HideFlags.None;
			gameObject.hideFlags = HideFlags.None;
			#else
			gameObject.hideFlags = HideFlags.HideInHierarchy;
			#endif

			if(!target)
			{
				EditorApplication.delayCall += () =>
				{
					if(this && !target)
					{
						Remove(ModifyOptions.Immediate | ModifyOptions.NonUndoable);
					}
				};
				return;
			}

			if(transform.parent != target.transform)
			{
				if(transform == target.transform)
				{
					target = null;
					Remove(ModifyOptions.NonUndoable);
					return;
				}

				EditorApplication.delayCall += () =>
				{
					if (!this)
					{
						return;
					}

					if(!target)
					{
						Remove(ModifyOptions.Immediate | ModifyOptions.NonUndoable);
						return;
					}

					if(transform.parent == target.transform)
					{
						return;
					}

					// Avoid error 'Setting the parent of a transform which resides in a Prefab Asset is disabled to prevent data corruption'.
					if(PrefabUtility.IsPartOfPrefabAsset(this))
					{
						#if DEV_MODE
						Debug.Log($"Won't reparent NameContainer(\"{nameOverride}\") because it's part of a prefab asset.", transform.parent);
						#endif
						return;
					}

					#if !UNITY_2022_3_OR_NEWER
					// In older versions of Unity it's not possible to reparent/remove game objects inside prefab instances.
					// Would need to unpack the prefab, perform the modifications on the unpacked game object, and apply
					// the changes on top of the old prefab asset - which can get quite complex and error-prone.
					if(PrefabUtility.IsPartOfPrefabInstance(gameObject) && !PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject))
					{
						#if DEV_MODE
						Debug.Log($"Won't reparent NameContainer(\"{nameOverride}\") because it's part of a prefab instance.", transform.parent);
						#endif
						return;
					}
					#endif

					transform.SetParent(target.transform, false);
				};
			}

			if(instances.TryGetValue(target, out var existingContainer) && existingContainer != this && existingContainer && existingContainer.target == target)
			{
				// Copy over name and tooltip from this container to the other one and destroy this one.
				// It is likely that this container contains the name/tooltip for a prefab instance and the other
				// one contains it for the prefab asset.
				// In this situation we want to convert the name and tooltips into instance value overrides
				// instead of having two different name containers for one target.
				// In any case we never want to have two different name containers when it can be avoided.
				existingContainer.NameOverride = NameOverride;
				existingContainer.TooltipOverride = TooltipOverride;
				Remove(ModifyOptions.NonUndoable);
				return;
			}

			instances[target] = this;

			if(IsEmptyOrDefaultName(nameOverride))
			{
				if(TooltipOverride.Length == 0)
				{
					Remove(ModifyOptions.Defaults);
				}
				else
				{
					ComponentName.ResetToDefault(target, ModifyOptions.DontUpdateNameContainer);
				}
			}
			else
			{
				EditorApplication.delayCall += ()=>
				{
					if(!this)
					{
						return;
					}

					if(!target)
					{
						Remove(ModifyOptions.Immediate | ModifyOptions.NonUndoable);
						return;
					}

					if(IsEmptyOrDefaultName(nameOverride))
					{
						if(TooltipOverride.Length == 0)
						{
							Remove(ModifyOptions.Immediate);
							return;
						}

						ComponentName.ResetToDefault(target, ModifyOptions.Immediate | ModifyOptions.DontUpdateNameContainer);
						return;
					}

					ComponentTooltip.Set(target, tooltipOverride, ModifyOptions.Immediate | ModifyOptions.DontUpdateNameContainer);
					ComponentName.Set(target, nameOverride, ModifyOptions.Immediate | ModifyOptions.DontUpdateNameContainer);
				};
			}
		}

		internal static void StartRenaming(Component component)
		{
			#if DEV_MODE
			Debug.Assert(component);
			Debug.Assert(component is not NameContainer);
			#endif

			NowRenaming = true;
			StartingToRename = component;

			EditorGUIUtility.editingTextField = true;
		}

		internal static void TryGetOrCreate([AllowNull] Component component, ModifyOptions modifyOptions, Action<NameContainer> onAcquired, string initialName, string initialTooltip)
		{
			if(modifyOptions.IsDelayed())
			{
				EditorApplication.delayCall += TryGetOrCreateNow;
			}
			else
			{
				TryGetOrCreateNow();
			}

			void TryGetOrCreateNow()
			{
				if(!component)
				{
					return;
				}

				if(TryGetOrCreateImmediate(component, out var nameContainer, initialName, initialTooltip))
				{
					onAcquired?.Invoke(nameContainer);
				}
			}
		}

		private static bool TryGetOrCreateImmediate([AllowNull] Component component, [MaybeNullWhen(false), NotNullWhen(true)] out NameContainer nameContainer, string initialName = null, string initialTooltip = null)
		{
			if(!component)
			{
				nameContainer = null;
				return false;
			}

			var gameObjectWithComponent = component.gameObject;
			if(!gameObjectWithComponent)
			{
				nameContainer = null;
				return false;
			}

			if(TryGet(component, out nameContainer))
			{
				return nameContainer;
			}

			var containerGameObject = new GameObject("NameContainer(EditorOnly)");
			bool wasRenaming = NowRenaming;
			NowRenaming = true;

			try
			{
				if(!Application.isPlaying || PrefabUtility.IsPartOfPrefabAsset(component))
				{
					Undo.RegisterCreatedObjectUndo(containerGameObject, "Set Component Name");
				}

				containerGameObject.transform.SetParent(gameObjectWithComponent.transform, false);

				nameContainer = containerGameObject.AddComponent<NameContainer>();

				#if DEV_MODE && SHOW_NAME_CONTAINERS
				containerGameObject.hideFlags = HideFlags.None;
				nameContainer.hideFlags = HideFlags.None;
				#else
				containerGameObject.hideFlags = HideFlags.HideInHierarchy;
				#endif

				containerGameObject.tag = "EditorOnly";
				nameContainer.target = component;

				if(initialName != null)
				{
					nameContainer.NameOverride = initialName;
				}

				if(initialTooltip != null)
				{
					nameContainer.TooltipOverride = initialTooltip;
				}
			}
			catch(Exception e)
			{
				Debug.LogWarning(e);
				nameContainer.RemoveImmediate(true);
				return false;
			}
			finally
			{
				if(!wasRenaming)
				{
					NowRenaming = false;
				}
			}

			instances[component] = nameContainer;
			return true;
		}

		internal static bool TryGet(Component component, out NameContainer nameContainer)
		{
			if(instances.TryGetValue(component, out nameContainer) && nameContainer && nameContainer.target == component)
			{
				return nameContainer;
			}

			var transform = component.transform;
			for(int i = transform.childCount -  1; i >= 0; i--)
			{
				if(transform.GetChild(i).TryGetComponent(out NameContainer someNameContainer) && someNameContainer.target == component)
				{
					nameContainer = someNameContainer;
					return true;
				}
			}

			nameContainer = null;
			return false;
		}
		
		internal static void TryGet(Component component, Action<NameContainer> onAcquired, ModifyOptions modifyOptions)
		{
			if(!component)
			{
				return;
			}

			if(modifyOptions.IsDelayed())
			{
				EditorApplication.delayCall += ()=> TryGet(component, onAcquired, modifyOptions | ModifyOptions.Immediate);
				return;
			}

			var transform = component.transform;
			for(int i = transform.childCount -  1; i >= 0; i--)
			{
				if(transform.GetChild(i).TryGetComponent(out NameContainer nameContainer) && nameContainer.target == component)
				{
					onAcquired(nameContainer);
					return;
				}
			}
		}

		internal void Remove(ModifyOptions modifyOptions)
		{
			if(!modifyOptions.IsRemovingNameContainerAllowed())
			{
				nameOverride = "";
				tooltipOverride = "";
				return;
			}

			bool isUndoable = modifyOptions.IsUndoable();
			if(modifyOptions.IsDelayed())
			{
				EditorApplication.delayCall += ()=> RemoveImmediate(isUndoable);
			}
			else
			{
				RemoveImmediate(isUndoable);
			}
		}

		private void RemoveImmediate(bool undoable)
		{
			if(!this)
			{
				return;
			}

			#if !UNITY_2022_3_OR_NEWER
			// In older versions of Unity it's not possible to simply remove game objects from prefab instances.
			// Would need to unpack the prefab, perform the modifications on the unpacked game object, and apply
			// the changes on top of the old prefab asset - which can get quite complex and error-prone.
			if(PrefabUtility.IsPartOfPrefabInstance(gameObject) && !PrefabUtility.IsOutermostPrefabInstanceRoot(gameObject))
			{
				#if DEV_MODE && DEBUG_DESTROY
				Debug.Log($"Won't destroy NameContainer(\"{nameOverride}\") because it's part of a prefab instance.", transform.parent);
				#endif

				nameOverride = "";
				tooltipOverride = "";
				return;
			}
			#endif

			#if DEV_MODE && DEBUG_DESTROY
			Debug.Log($"Destroying NameContainer(\"{nameOverride}\")");
			#endif

			// If the GameObject somehow contains more components than just a Transform and a NameContainer,
			// or has any child game objects, then don't destroy the whole game object to avoid undesired data loss.
			if(gameObject.GetComponents<Component>().Length > 2 || transform.childCount > 0)
			{
				#if DEV_MODE
				Debug.LogWarning($"Destroying NameContainer(\"{nameOverride}\") component only because game object contains extra components: {string.Join(", ", GetComponents<Component>().Select(c => c?.GetType().Name))}.", transform);
				#endif

				ObjectUtility.Destroy(this, undoable);
				return;
			}

			#if DEV_MODE && DEBUG_DESTROY
			Debug.Log($"Destroying NameContainer(\"{nameOverride}\").", transform.parent);
			#endif

			ObjectUtility.Destroy(gameObject, undoable);
		}

		private bool IsEmptyOrDefaultName(string name) => name.Length == 0 || string.Equals(name, ComponentName.GetDefault(target));
		#endif
	}
}