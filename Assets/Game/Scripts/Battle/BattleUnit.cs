using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public UnitData Data;
    public UnitSO UnitSO;
    public Team Team;
    public bool IsDead;

    private void Awake()
    {
        Data = new UnitData();

        Data.Name = UnitSO.Name;
        Data.MaxHP = UnitSO.MaxHP;
        Data.CurrentHP = UnitSO.MaxHP;
        Data.CurrentAtk = UnitSO.Attack;
        Data.CurrentDef = UnitSO.Defense;
        Data.CurrentSpd = UnitSO.Speed;
        Data.CurrentCritChance = UnitSO.CritChance;
    }

    public void TakeDamage(int dmg)
    {
        Data.CurrentHP -= dmg;

        BattleEvents.OnUnitDamaged?.Invoke(this);

        if(Data.CurrentHP <= 0)
        {
            Data.CurrentHP = 0;
            IsDead = true;
        }

        Debug.Log($"{Data.Name} HP: {Data.CurrentHP}");
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

