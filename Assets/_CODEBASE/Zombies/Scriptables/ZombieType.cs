using UnityEngine;

[CreateAssetMenu(fileName = "New Zombie", menuName = "My Assets/Zombies/ZombieType")]
public class ZombieType : ScriptableObject
{
    [SerializeField] private int hp;
    [SerializeField] private int damage;
    [SerializeField] private FloatReference walkSpeed;

    public int HP => hp;
    public int Damage => damage;
    public FloatReference WalkSpeed => walkSpeed;

}
