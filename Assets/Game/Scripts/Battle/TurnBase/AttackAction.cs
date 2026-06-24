using System.Collections.Generic;
using System.Collections;

public class AttackAction : BattleAction
{
    public override IEnumerator Execute(BattleUnit user, List<BattleUnit> targets)
    {
        BattleUnit target = targets[0];

        int damage =
            DamageSystem.CalculateDmg(
                user,
                target);

        target.TakeDamage(damage);

        yield return null;
    }
}