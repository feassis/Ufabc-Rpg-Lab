using UnityEngine;
using UnityEngine.Rendering;

//classe base para o controlador dos inimigos
public class EnemyController : MonoBehaviour
{
    private GameObject player;
    [Header("References")]
    [SerializeField] private EnemyTriggerCollision collision;
    [SerializeField] private Transform body;
    [SerializeField] private Transform shadow;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] protected Animator animator;
    [Header("Visuals")]
    [SerializeField] private GameObject visuals;

    private Vector3 visualsStartScale;
    private Vector3 visualsStartEuler;

    private Vector3 bodyStartlocalPos;

    public LevelManager levelManager {  get; private set; }

    public Animator GetAnimator() => animator;

    [field: SerializeField] public EnemyData Data {  get; protected set; }

    public GameObject GetPlayer() => player;
    public GameObject SetPlayer(GameObject player) => this.player = player;

    public void SetLevelManager(LevelManager levelManager) => this.levelManager = levelManager;

    public Vector3 GetPlayerPos() => player.transform.position;

    public void SetVelocity(Vector3 velocity) => rb.linearVelocity = velocity; 

    public bool IsTouchingPlayer() => isTouchingPlayer;

    protected bool isTouchingPlayer;

    //se inscreve aos eventos de colis�o com o player
    protected virtual void Awake()
    {
        collision.OnPlayerEntry += Collision_OnPlayerEntry;
        collision.OnPlayerExit += Collision_OnPlayerExit;
        bodyStartlocalPos = body.transform.localPosition;
        visualsStartScale = visuals != null ? visuals.transform.localScale : Vector3.one;
        visualsStartEuler = visuals != null ? visuals.transform.localEulerAngles : Vector3.zero;
    }

    protected virtual void Update()
    {
        UpdateFacing();
    }

    private void UpdateFacing()
    {
        if (player == null || visuals == null) return;

        float dir = player.transform.position.x - transform.position.x;
        if (Mathf.Approximately(dir, 0f)) { Debug.Log("Player is directly in front of enemy."); return; }
        float yAngle = dir < 0f ? 180f : 0f;

        visuals.transform.localEulerAngles = new Vector3(visualsStartEuler.x, yAngle, visualsStartEuler.z);
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
