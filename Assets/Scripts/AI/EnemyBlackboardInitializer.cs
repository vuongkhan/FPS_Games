using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BlackboardBase))]
public class EnemyBlackboardInitializer : MonoBehaviour
{
    [SerializeField] protected BlackboardBase bb;

    [Header("Enemy Stats")]
    [SerializeField] protected float hp = 1000f;
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float lastDamage = 1f;
    [SerializeField] protected float maxStamina = 100f;
    [SerializeField] protected float attackRange = 5f;

    [Header("Patrol Settings")]
    public Transform[] patrolWaypoints;

    protected float staminaRegenTimer = 0f;
    protected const float StaminaRegenInterval = 1f;
    protected const float StaminaRegenAmount = 1f;

    protected virtual void Awake()
    {
        if (bb == null)
            bb = GetComponent<BlackboardBase>();

        if (bb == null)
        {
            Debug.LogError("Cannot Find BB");
            return;
        }
    }

    protected virtual void Start()
    {
        Initialize();
    }

    public virtual void Initialize()
    {
        bb.TrySetDefault<NavMeshAgent>("agent", GetComponent<NavMeshAgent>());
        bb.TrySetDefault<Animator>("animator", GetComponent<Animator>());
        bb.TrySetDefault<GameObject>("owner", gameObject);
        bb.TrySetDefault("stamina", maxStamina);
        bb.TrySetDefault("hp", hp);
        bb.TrySetDefault("speed", speed);
        bb.TrySetDefault("lastDamage", lastDamage);
        bb.TrySetDefault("attackRange", attackRange);
        bb.TrySetDefault<string>("currentAttack", null);

        var enemyBase = GetComponent<EnemyBase>();
        if (enemyBase != null)
        {
            bb.TrySetDefault("enemy", enemyBase);
        }
        else
        {
            Debug.LogWarning("Without EnemyBase");
        }

        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            bb.TrySetDefault(BBKeys.Target, player);
        }

        bb.TrySetDefault(BBKeys.CanSeeEnemy, false);

        if (patrolWaypoints != null && patrolWaypoints.Length > 0)
        {
            bb.TrySetDefault("waypoints", patrolWaypoints);
            bb.TrySetDefault("waypointIndex", 0);
            bb.TrySetDefault("currentWaypoint", patrolWaypoints[0]);
        }
        else
        {
            Debug.LogWarning("Without Point");
        }
    }
}
