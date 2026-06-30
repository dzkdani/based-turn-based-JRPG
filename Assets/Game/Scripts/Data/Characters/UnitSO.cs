using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Unit", order = 1)]
public class UnitSO : ScriptableObject
{
    public string Name;
    public Team team;

    public int MaxHP;
    public int Attack;
    public float CritChance;
    public int Defense;
    public int Speed;

    public AIBehaviorSO AIBehavior;
    public List<BattleActionSO> Skills;
}