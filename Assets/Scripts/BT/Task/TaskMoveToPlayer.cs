using UnityEngine;
using UnityEngine.AI;

public class TaskMoveToPlayer : Node
{
    private float stoppingBuffer = 1f;
    private float maxSpeed = 5f;
    private float accelerationRate = 0.1f;

    public override NodeState Evaluate(BlackboardBase blackboard)
    {
        if (!blackboard.TryGet<NavMeshAgent>("agent", out var agent)) return NodeState.FAILURE;
        if (!blackboard.TryGet<GameObject>("target", out var target) || target == null) return NodeState.FAILURE;
        if (!blackboard.TryGet<Animator>("animator", out var animator)) return NodeState.FAILURE;
        if (!agent.isOnNavMesh) return NodeState.FAILURE;

        Vector3 targetPos = target.transform.position;

        if (blackboard.TryGet<float>("speed", out var speed))
        {
            float newSpeed = Mathf.Min(speed + accelerationRate, maxSpeed);

            if (agent.speed != newSpeed)
            {
                agent.speed = newSpeed;
                blackboard.Set("speed", newSpeed);
            }

            animator.SetFloat("Speed", newSpeed);
        }
        if (agent.destination != targetPos)
        {
            agent.SetDestination(targetPos);
            agent.isStopped = false;
        }
        float attackRange = agent.stoppingDistance; // fallback
        if (blackboard.TryGet<float>("attackRange", out var range))
        {
            attackRange = range;
        }

        if (!agent.pathPending && agent.remainingDistance <= attackRange + stoppingBuffer)
        {
            agent.isStopped = true;
            agent.ResetPath();

            animator.SetFloat("Speed", 0f);
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}
