using UnityEngine;

public class TransparencyController : MonoBehaviour
{
    public Transform player;
    public Material material;
    public Vector3 offset;

    void Update()
    {
        material.SetVector("_PlayerPos", player.position + offset);
    }
}