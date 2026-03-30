using UnityEngine;
using FMODUnity;

public class FMODButtonHover : MonoBehaviour
{
    public EventReference hoverSound;
    public EventReference selectSound;

    public void OnHover()
    {
        RuntimeManager.PlayOneShotAttached(hoverSound, gameObject);
    }

    public void OnSelect()
    {
        RuntimeManager.PlayOneShotAttached(selectSound, gameObject);
    }
}
