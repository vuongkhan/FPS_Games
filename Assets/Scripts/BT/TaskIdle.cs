using UnityEngine;
public class TaskIdle : Node
{
    public override NodeState Evaluate(BlackboardBase blackboard)
    {
        return NodeState.SUCCESS;
    }
}