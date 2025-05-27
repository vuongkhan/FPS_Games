using UnityEngine;

public class TaskHeavyAttack : Node
{
    private const float StaminaCost = 50f;
    private const string AttackKey = "HeavyAttack"; 
    private const string CurrentAttackKey = "currentAttack";

    public override NodeState Evaluate(BlackboardBase blackboard)
    {
        if (!blackboard.TryGet<float>("stamina", out var stamina))
        {
            return NodeState.FAILURE;
        }
        if (!blackboard.TryGet<Animator>("animator", out var animator))
        {
            return NodeState.FAILURE;
        }
        if (!blackboard.TryGet<string>(CurrentAttackKey, out var currentAttack) || currentAttack != AttackKey)
        {
            if (stamina < StaminaCost)
            {
                return NodeState.FAILURE;
            }

            animator.CrossFade(AttackKey, 0.1f);
            blackboard.Set(CurrentAttackKey, AttackKey);

            float newStamina = stamina - StaminaCost;
            blackboard.Set("stamina", newStamina);

            return NodeState.RUNNING;
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!animator.IsInTransition(0) && stateInfo.IsName(AttackKey) && stateInfo.normalizedTime >= 1f)
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
