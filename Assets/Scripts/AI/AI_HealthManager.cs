using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_HealthManager : MonoBehaviour
{
    [SerializeField] protected BlackboardBase bb;
    public EnemyFSMController fsmController { get; private set; }

    protected float staminaRegenTimer = 0f;
    protected const float StaminaRegenInterval = 1f;
    protected const float StaminaRegenAmount = 1f;
    protected const float MaxStamina = 100f;

    private EnemyBase enemyBase;

    protected virtual void Awake()
    {
        if (bb == null)
            bb = GetComponent<BlackboardBase>();

        enemyBase = GetComponent<EnemyBase>();

        if (enemyBase != null)
        {
            fsmController = enemyBase.fsmController;
        }
        else
        {
            Debug.LogError("Cannot Find EnemyBase");
        }
    }

    protected virtual IEnumerator Start()
    {
        // Đợi 1 frame để đảm bảo tất cả component được khởi tạo
        yield return null;

        if (fsmController == null && enemyBase != null)
        {
            fsmController = enemyBase.fsmController;
            if (fsmController == null)
            {
                Debug.LogWarning($"⚠️ [{gameObject.name}] FSM Controller vẫn NULL sau 1 frame.");
            }
        }
    }

    void Update()
    {
        RegenerateStamina();

        if (fsmController != null)
        {
            fsmController.Update();
        }
        else
        {
            Debug.LogWarning($"⚠️ [{gameObject.name}] FSM Controller chưa sẵn sàng, bỏ qua Update.");
        }
    }

    protected virtual void RegenerateStamina()
    {
        staminaRegenTimer += Time.deltaTime;
        if (staminaRegenTimer >= StaminaRegenInterval)
        {
            staminaRegenTimer = 0f;

            if (bb.TryGet<float>("stamina", out float currentStamina))
            {
                if (currentStamina < MaxStamina)
                {
                    float newStamina = Mathf.Min(currentStamina + StaminaRegenAmount, MaxStamina);
                    bb.Set("stamina", newStamina);
                    Debug.Log($"💚 [{gameObject.name}] Hồi stamina: {currentStamina} ➡️ {newStamina}");
                }
            }
        }
    }

    public virtual void TakeDamage(float damage)
    {
        Debug.Log($"💥 [{gameObject.name}] Nhận sát thương: {damage}");
        bb.Set("lastDamage", damage);

        if (bb.TryGet<float>("hp", out float currentHP))
        {
            float newHP = Mathf.Max(currentHP - damage, 0f);
            bb.Set("hp", newHP);
            Debug.Log($"❤️ [{gameObject.name}] HP: {currentHP} ➖ {damage} = {newHP}");

            if (damage >= 50f && fsmController != null)
            {
                fsmController.ChangeState(new EnemyStunState(enemyBase));
                Debug.Log($"😵 [{gameObject.name}] Vào trạng thái bị khống chế!");
            }

            if (newHP <= 0f && fsmController != null)
            {
                fsmController.ChangeState(new EnemyDieState(enemyBase));
            }
        }
        else
        {
            Debug.LogWarning($"❌ [{gameObject.name}] Không tìm thấy 'hp' trong Blackboard.");
        }
    }
}
