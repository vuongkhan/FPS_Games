using UnityEngine;

public class TaskLightAttack : Node
{
    private const string AttackKey = "LightAttack"; 
    private const string CurrentAttackKey = "currentAttack";

    public override NodeState Evaluate(BlackboardBase blackboard)
    {
        if (!blackboard.TryGet<Animator>("animator", out var animator))
        {
            return NodeState.FAILURE;
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!blackboard.TryGet<string>(CurrentAttackKey, out var currentAttack) || currentAttack != AttackKey)
        {
            if (stateInfo.IsName(AttackKey))
            {
                animator.Play("Idle", 0, 0); 
                animator.Update(0);          
            }

            animator.CrossFade(AttackKey, 0.1f);
            blackboard.Set(CurrentAttackKey, AttackKey);
            return NodeState.RUNNING;
        }
        if (!animator.IsInTransition(0) && stateInfo.IsName(AttackKey) && stateInfo.normalizedTime >= 0.95f)
        {
            animator.Play("Idle");
            blackboard.Remove(CurrentAttackKey);
            if (blackboard.TryGet<EnemyBase>("enemy", out var enemy))
            {
                enemy.fsmController.ForceChangeState(new EnemyChaseState(enemy));
            }

            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}
