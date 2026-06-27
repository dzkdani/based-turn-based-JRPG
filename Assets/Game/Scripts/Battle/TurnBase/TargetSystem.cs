using System.Collections.Generic;

public class TargetSystem
{
    public List<BattleUnit> GetTargets(
        BattleUnit caster,
        TargetType type,
        List<BattleUnit> players,
        List<BattleUnit> enemies)
    {
        // switch(type)
        // {
        //     case TargetType.SingleEnemy:
        //         return GetSingleEnemy(caster, players, enemies);

        //     case TargetType.AllEnemies:
        //         return GetAllEnemies(caster, players, enemies);

        //     case TargetType.SingleAlly:
        //         return GetSingleAlly(caster, players, enemies);

        //     case TargetType.AllAllies:
        //         return GetAllAllies(caster, players, enemies);

        //     case TargetType.Self:
        //         return new List<BattleUnit>{ caster };
        // }

        return new List<BattleUnit>();
    }
}

public enum TargetType
{
    SingleEnemy,
    AllEnemies,

    SingleAlly,
    AllAllies,

    Self,

    Everyone
}