using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AttackAction : BattleAction
{
    public override IEnumerator Execute(BattleUnit attacker, List<BattleUnit> targets)
    {
        BattleUnit target = targets[0];

        int damage =
            DamageSystem.CalculateDmg(
                attacker,
                target);

        target.TakeDamage(damage);

        yield return null;
    }
}