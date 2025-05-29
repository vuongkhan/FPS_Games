using System.Collections.Generic;

public class SequenceNode : Node
{
    private List<Node> children = new List<Node>();
    private int currentTaskIndex = 0; 

    public void AddChild(Node child) => children.Add(child);

    public override NodeState Evaluate(BlackboardBase blackboard)
    {
        if (currentTaskIndex >= children.Count)
        {
            currentTaskIndex = 0;
            return NodeState.SUCCESS;
        }

        NodeState result = children[currentTaskIndex].Evaluate(blackboard);

        if (result == NodeState.SUCCESS)
        {
            currentTaskIndex++;
            return NodeState.RUNNING; 
        }

        if (result == NodeState.FAILURE)
        {
            currentTaskIndex = 0; 
            return NodeState.FAILURE;
        }

        return NodeState.RUNNING; 
    }
}
