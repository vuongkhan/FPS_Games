using UnityEngine;

public class TaskFallDown : Node
{
    private const string FallAnimName = "FallBack";
    private const string CurrentAnimKey = "currentFall";

    public override NodeState Evaluate(BlackboardBase blackboard)
    {
        if (!blackboard.TryGet<Animator>("animator", out var animator))
        {
            return NodeState.FAILURE;
        }
        if (!blackboard.TryGet<string>(CurrentAnimKey, out var current) || current != FallAnimName)
        {
            animator.CrossFade(FallAnimName, 0.1f);
            blackboard.Set(CurrentAnimKey, FallAnimName);
            return NodeState.RUNNING;
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!animator.IsInTransition(0) && stateInfo.IsName(FallAnimName) && stateInfo.normalizedTime >= 1f)
        {
            animator.Play("Idle");
            blackboard.Remove(CurrentAnimKey);

            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}
