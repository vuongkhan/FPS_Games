using UnityEngine;

public class TaskReactToPlayer : Node
{
    public override NodeState Evaluate(BlackboardBase blackboard)
    {
        return NodeState.SUCCESS;
    }
}
