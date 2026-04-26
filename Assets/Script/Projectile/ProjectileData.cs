using UnityEngine;

//scriptable object para configurar os dados de um projeto
[CreateAssetMenu(fileName = "ProjectileData", menuName = "Setup/Projectile/Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public float Speed = 8f;
    public float Damage = 10f;
    public float Lifetime = 4f;
}
