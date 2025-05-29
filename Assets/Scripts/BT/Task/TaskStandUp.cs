using UnityEngine;

public class TaskStandUp : Node
{
    private const string StandUpAnimName = "StandUp";
    private const string CurrentAnimKey = "currentFall"; // Giữ key này để đồng bộ flow

    public override NodeState Evaluate(BlackboardBase blackboard)
    {
        if (!blackboard.TryGet<Animator>("animator", out var animator))
        {
            return NodeState.FAILURE;
        }
        if (!blackboard.TryGet<string>(CurrentAnimKey, out var current) || current != StandUpAnimName)
        {
            animator.CrossFade(StandUpAnimName, 0.1f);
            blackboard.Set(CurrentAnimKey, StandUpAnimName);
            return NodeState.RUNNING;
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!animator.IsInTransition(0) && stateInfo.IsName(StandUpAnimName) && stateInfo.normalizedTime >= 1f)
        {
            blackboard.Remove(CurrentAnimKey);
            animator.Play("Idle");
            if (blackboard.TryGet<EnemyBase>("enemy", out var enemy))
            {
                enemy.fsmController.ForceChangeState(new EnemyChaseState(enemy));
            }
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}
