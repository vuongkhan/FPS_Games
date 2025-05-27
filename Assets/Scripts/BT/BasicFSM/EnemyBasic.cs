using UnityEngine;

public class EnemyBasic : EnemyBase
{

    protected override void Update()
    {
        base.Update(); 
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void SeeEnemy() 
    {
        if (Target != null && CanSeeEnemy)
        {
            fsmController.ChangeState(new EnemyChaseState(this));
        }
    }
    public void Attack(int damage)
    {
        if (Target == null) return;

        float attackRange = 2f;
        if (Vector3.Distance(transform.position, Target.transform.position) <= attackRange)
        {
            HealthManager.Instance.ApplyDamage(damage);
        }
        else
        {
            Debug.Log($"{name} not in range");
        }
    }
}
