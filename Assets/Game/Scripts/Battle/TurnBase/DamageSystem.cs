using UnityEngine;

public static class DamageSystem
{
    public static int CalculateDmg(
        BattleUnit attacker,
        BattleUnit defender)
    {
        int attack =
            attacker.Data.CurrentAtk;

        int defense =
            defender.Data.CurrentDef;

        int damage =
            attack - defense;

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