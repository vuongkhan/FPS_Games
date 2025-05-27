using UnityEngine;
using UnityEngine.AI;

public class EnemyAI_Base : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent m_NavMeshAgent;
    public GameObject m_Target; 
    [SerializeField] protected float speedMultiplier = 1.0f; 
    private Vector3 lastTargetPosition; 
    private float updateThreshold = 0.5f; 

    protected virtual void Start()
    {
        if (m_Target != null && m_NavMeshAgent != null)
        {
            m_NavMeshAgent.speed *= speedMultiplier; 
            lastTargetPosition = m_Target.transform.position;
            m_NavMeshAgent.SetDestination(lastTargetPosition);
        }
    }

    protected virtual void Update()
    {
        if (m_NavMeshAgent == null || m_Target == null) return;

        float targetMoved = Vector3.Distance(lastTargetPosition, m_Target.transform.position);

        if (targetMoved > updateThreshold) 
        {
            lastTargetPosition = m_Target.transform.position;
            m_NavMeshAgent.SetDestination(lastTargetPosition);
            m_NavMeshAgent.isStopped = false;
        }
    }
}
