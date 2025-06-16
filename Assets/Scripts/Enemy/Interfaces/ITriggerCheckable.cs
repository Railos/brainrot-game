using UnityEngine;

public interface ITriggerCheckable
{
    bool isAggroed { get; set; }
    bool isWithingAttackDistance { get; set; }

    void SetAggroStatus(bool setIsAggroed);
    void SetAttackStatus(bool isWithinAttackRange);
}
