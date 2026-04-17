using UnityEngine;
using UnityEngine.Rendering;

public class EnemyController : MonoBehaviour
{
    private GameObject player; 
    [SerializeField] private EnemyTriggerCollision collision;
    [SerializeField] private Transform body;
    [SerializeField] private Transform shadow;
    [SerializeField] private Rigidbody2D rb;

    private Vector3 bodyStartlocalPos;

    public LevelManager levelManager {  get; private set; }

    [field: SerializeField] public EnemyData Data {  get; protected set; }

    public GameObject GetPlayer() => player;
    public GameObject SetPlayer(GameObject player) => this.player = player;

    public void SetLevelManager(LevelManager levelManager) => this.levelManager = levelManager;

    public Vector3 GetPlayerPos() => player.transform.position;

    public void SetVelocity(Vector3 velocity) => rb.linearVelocity = velocity; 

    public bool IsTouchingPlayer() => isTouchingPlayer;

    protected bool isTouchingPlayer;
    protected virtual void Awake()
    {
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
