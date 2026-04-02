using UnityEngine;

[CreateAssetMenu(fileName = "New Player Movement Data", menuName = "Setup/Player/Movement Data")]
public class PlayerMovementData : ScriptableObject
{
    public float Speed;
    public float SprintSpeed;
    public float DashSpeed;
    public float DashDuration;
    public float DashCoolDown;
    public float RotationSpeed;
}
