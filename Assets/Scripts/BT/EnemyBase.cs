using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(BlackboardBase))]
public class EnemyBase : MonoBehaviour
{
    public BlackboardBase blackboard { get; private set; }
    public EnemyFSMController fsmController { get; private set; }
    public int damage;

    private void Awake()
    {
        GetComponent<EnemyBlackboardInitializer>()?.Initialize();
        blackboard = GetComponent<BlackboardBase>();
        if (blackboard == null)
        {
            Debug.LogError("Blackboard is missing on " + gameObject.name);
        }
        fsmController = new EnemyFSMController();
    }

    protected virtual void Start()
    {
        if (fsmController != null)
        {
            fsmController.ChangeState(new EnemyPatrolState(this));
        }
        else
        {
            Debug.LogError("FSM Controller is null on " + gameObject.name);
        }
    }

    protected virtual void Update()
    {
        UpdateVision();
        fsmController?.Update();
        SeeEnemy();        
    }

    private void UpdateVision()
    {
        if (Target == null)
        {
            blackboard?.Set(BBKeys.CanSeeEnemy, false);
            return;
        }

        float distance = Vector3.Distance(transform.position, Target.transform.position);
        blackboard?.Set(BBKeys.CanSeeEnemy, distance <= 10f);
    }
    protected virtual void SeeEnemy() { }
    public GameObject Target => blackboard?.Get<GameObject>(BBKeys.Target);
    public bool CanSeeEnemy => blackboard?.Get<bool>(BBKeys.CanSeeEnemy) ?? false;
}
