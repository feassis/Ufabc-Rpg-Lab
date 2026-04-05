using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{
    private GameObject player; //This Will Get Player From the enemymaneger once implemented
    [SerializeField] private EnemyTriggerCollision collision;
    [SerializeField] private Transform body;
    [SerializeField] private Transform shadow;

    private Vector3 bodyStartlocalPos;

    [field: SerializeField] public EnemyData Data {  get; protected set; }

    public GameObject GetPlayer() => player;

    public Vector3 GetPlayerPos() => player.transform.position;

    public bool IsTouchingPlayer() => isTouchingPlayer;

    protected bool isTouchingPlayer;
    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Not the most efficient way to get the player, but it works for now.
        collision.OnPlayerEntry += Collision_OnPlayerEntry;
        collision.OnPlayerExit += Collision_OnPlayerExit;
        bodyStartlocalPos = body.transform.localPosition;
    }

    public Vector3 GetBodyPos() => body.transform.position;

    public void SetBodyPos(Vector3 pos) => body.transform.position = pos;

    public Vector3 GetStartingLocalPos() => bodyStartlocalPos;

    public Vector3 GetShadowPos() => shadow.transform.position;

    protected void Collision_OnPlayerExit(GameObject obj)
    {
        isTouchingPlayer = false;
    }

    protected void Collision_OnPlayerEntry(GameObject obj)
    {
        isTouchingPlayer = true;
    }
}
