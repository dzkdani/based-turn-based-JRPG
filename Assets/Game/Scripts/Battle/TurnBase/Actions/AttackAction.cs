using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AttackAction : BattleAction
{
    public override IEnumerator Execute(
    BattleUnit attacker,
    List<BattleUnit> targets,
    BattleActionSO actionData)
    {
        BattleUnit target = targets[0];

        int damage =
            DamageSystem.CalculateDamage(
                attacker,
                target,
                actionData.Multiplier);

        target.TakeDamage(damage);

        yield return null;
    }
}