using UnityEngine;

public static class DamageSystem
{
    public static int CalculateDamage(
        BattleUnit attacker,
        BattleUnit defender,
        int multiplier)
    {
        int attack =
            attacker.Data.CurrentAtk;

        int defense =
            defender.Data.CurrentDef;

        int damage =
            (attack - defense) * multiplier;

        bool crit = Random.value < attacker.Data.CurrentCritChance;

        if(crit)
            damage *= 2;

        return damage;
    }
}

public struct DamageResult
{
    public int Damage;
    public bool IsCrit;
    public bool IsMiss;
}