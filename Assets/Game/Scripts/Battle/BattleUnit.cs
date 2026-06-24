using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public UnitData Data;

    public UnitSO unitSO;

    public Team Team;

    public bool IsDead;

    public void TakeDamage(int dmg)
    {
        Data.CurrentHP -= dmg;

        if(Data.CurrentHP <= 0)
        {
            Data.CurrentHP = 0;

            IsDead = true;
        }
    }

    public void Heal(int amount)
    {
        
    }
}

public enum Team
{
    Player,
    Enemy
}

