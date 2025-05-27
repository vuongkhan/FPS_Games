using System;
using System.Collections.Generic;

public enum ConditionMode
{
    AllMustPass,   
    AnyCanPass,    
    Invert       
}
public class DecoratorConditions : Node
{
    private readonly Node child;
    private readonly List<Func<BlackboardBase, bool>> conditions;
    private readonly ConditionMode mode;

    private bool isRunning = false;

    public DecoratorConditions(Node child, ConditionMode mode, params Func<BlackboardBase, bool>[] conditions)
    {
        this.child = child;
        this.mode = mode;
        this.conditions = new List<Func<BlackboardBase, bool>>(conditions);
    }

    public override NodeState Evaluate(BlackboardBase blackboard)
    {
        if (isRunning)
        {
            var runningResult = child.Evaluate(blackboard);

            if (runningResult != NodeState.RUNNING)
                isRunning = false;

            return runningResult;
        }
        if (!CheckConditions(blackboard))
        {
            return NodeState.FAILURE;
        }
        var result = child.Evaluate(blackboard);

        isRunning = (result == NodeState.RUNNING);
        return result;
    }


    private bool CheckConditions(BlackboardBase blackboard)
    {
        switch (mode)
        {
            case ConditionMode.AllMustPass:
                return conditions.TrueForAll(cond => cond(blackboard));

            case ConditionMode.AnyCanPass:
                return conditions.Exists(cond => cond(blackboard));

            case ConditionMode.Invert:
                return conditions.Count == 1 && !conditions[0](blackboard);

            default:
                return false;
        }
    }
}
