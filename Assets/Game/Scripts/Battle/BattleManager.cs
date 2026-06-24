using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    public BattleState CurrentState;
    public TurnManager TurnManager;
    public ActionSystem ActionSystem;

    public List<BattleUnit> PlayerUnits;
    public List<BattleUnit> EnemyUnits;
    public BattleUnit CurrentUnit;

    public void InitializeBattle(
        List<BattleUnit> players,
        List<BattleUnit> enemies)
    {
        PlayerUnits = players;
        EnemyUnits = enemies;

        StartBattle();
    }

    public void StartBattle()
    {
        CurrentState = BattleState.Setup;

        List<BattleUnit> allUnits =
            new List<BattleUnit>();

        allUnits.AddRange(PlayerUnits);
        allUnits.AddRange(EnemyUnits);

        TurnManager.Initialize(allUnits);

        StartTurn();
    }

    private void StartTurn()
    {
        CurrentState = BattleState.TurnStart;

        CurrentUnit =
            TurnManager.GetCurrentUnit();

        Debug.Log(
            $"Current Turn : {CurrentUnit.name}");
    }

    public void CheckBattleResult(List<BattleUnit> enemies)
    {
        if (AreAllEnemiesDead(enemies))
        {
            CurrentState = BattleState.Victory;
            EndBattle();
            return;
        }

        if (AreAllPlayersDead(enemies))
        {
            CurrentState = BattleState.Defeat;
            EndBattle();
            return;
        }
    }

    public bool AreAllEnemiesDead(List<BattleUnit> enemies)
    {
        foreach(var enemy in enemies)
        {
            if(!enemy.IsDead)
                return false;
        }

        return true;
    }

    public bool AreAllPlayersDead(List<BattleUnit> enemies)
    {
        return enemies.TrueForAll(
            x => x.IsDead);
    }

    public void EndBattle()
    {
        
    }
}