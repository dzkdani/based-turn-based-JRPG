using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class BattleFlow
{
    private readonly TurnManager turnManager;
    private readonly ActionSystem actionSystem;
    private readonly BattleAISystem aiSystem;
    private readonly TargetSystem targetSystem;

    private BattleActionSO currentAction;
    private BattleUnit currentTarget;

    public BattleState CurrentState { get; private set; }
    public BattleUnit CurrentUnit { get; private set; }
    public event Action EnemyTurnStarted;

    public BattleFlow(
        TurnManager turnManager,
        ActionSystem actionSystem,
        BattleAISystem aiSystem,
        TargetSystem targetSystem)
    {
        this.turnManager = turnManager;
        this.actionSystem = actionSystem;
        this.aiSystem = aiSystem;
        this.targetSystem = targetSystem;
    }

    public void StartBattle(List<BattleUnit> players, List<BattleUnit> enemies)
    {
        CurrentState = BattleState.Setup;

        List<BattleUnit> allUnits = new List<BattleUnit>();
        allUnits.AddRange(players);
        allUnits.AddRange(enemies);

        turnManager.Initialize(allUnits);
        StartTurn();
    }

    private void StartTurn()
    {
        CurrentUnit = turnManager.GetCurrentUnit();
        currentAction = null;
        currentTarget = null;
        targetSystem.ClearSelectedTarget();
        BattleEvents.OnTargetSelected?.Invoke(null);

        if (CurrentUnit == null)
        {
            BattleEvents.OnBattleLog?.Invoke("No current turn unit.");
            return;
        }

        BattleEvents.OnBattleLog?.Invoke($"{CurrentUnit.Data.Name} Turn");

        if (CurrentUnit.Team == Team.Player)
        {
            CurrentState = BattleState.WaitingForCommand;
            BattleEvents.OnPlayerTurn?.Invoke();
            return;
        }

        CurrentState = BattleState.Executing;
        BattleEvents.OnEnemyTurn?.Invoke();
        EnemyTurnStarted?.Invoke();
    }

    public IEnumerator PerformEnemyTurn(
        List<BattleUnit> players,
        List<BattleUnit> enemies)
    {
        if (CurrentUnit == null)
            yield break;

        BattleEvents.OnBattleLog?.Invoke($"{CurrentUnit.Data.Name} Turn");
        yield return new WaitForSeconds(1.5f); //small delay

        BattleUnit target = aiSystem.ChooseTarget(CurrentUnit, players, enemies);
        if (target == null)
        {
            EndCurrentTurn(players, enemies);
            yield break;
        }

        List<BattleUnit> targets = new List<BattleUnit> { target };
        BattleActionSO attackAction = CurrentUnit.Data.Skills.FirstOrDefault(a => a.ActionType == BattleActionType.Attack);

        if (attackAction == null)
        {
            EndCurrentTurn(players, enemies);
            yield break;
        }

        yield return actionSystem.ExecuteAction(CurrentUnit, targets, attackAction);
        BattleEvents.OnBattleLog?.Invoke($"{CurrentUnit.Data.Name} dealt {CurrentUnit.Data.CurrentAtk} atk to {target.Data.Name}");
        yield return new WaitForSeconds(1.5f);
        EndCurrentTurn(players, enemies);
    }

    public void OnPlayerActionSelected(BattleActionSO action)
    {
        if (CurrentState != BattleState.WaitingForCommand)
            return;

        if (CurrentUnit == null || CurrentUnit.Team != Team.Player)
            return;

        if (action == null)
        {
            BattleEvents.OnBattleLog?.Invoke("No action selected.");
            return;
        }

        currentAction = action;
        CurrentState = BattleState.WaitingForTarget;
        BattleEvents.OnBattleLog?.Invoke($"Select target for {action.ActionName}");
    }

    public IEnumerator HandleTargetSelected(
        BattleUnit target,
        List<BattleUnit> players,
        List<BattleUnit> enemies)
    {
        if (CurrentState != BattleState.WaitingForTarget)
            yield break;

        currentTarget = target;
        targetSystem.SetSelectedTarget(target);

        if (currentAction == null)
        {
            BattleEvents.OnBattleLog?.Invoke("No action selected.");
            CurrentState = BattleState.WaitingForCommand;
            yield break;
        }

        CurrentState = BattleState.Executing;

        List<BattleUnit> targets = actionSystem.ResolveTargets(
            CurrentUnit,
            currentAction,
            players,
            enemies,
            currentTarget,
            targetSystem);

        if (targets.Count == 0)
        {
            BattleEvents.OnBattleLog?.Invoke("No valid target.");
            CurrentState = BattleState.WaitingForCommand;
            yield break;
        }

        yield return actionSystem.ExecuteAction(CurrentUnit, targets, currentAction);
        EndCurrentTurn(players, enemies);
    }

    public void AdvanceTurn()
    {
        turnManager.AdvanceTurn();
        StartTurn();
    }

    public void EndCurrentTurn(List<BattleUnit> players, List<BattleUnit> enemies)
    {
        if (AreAllEnemiesDead(enemies))
        {
            CurrentState = BattleState.Victory;
            BattleEvents.OnBattleLog?.Invoke("VICTORY!");
            BattleEvents.OnVictory?.Invoke();
            return;
        }

        if (AreAllPlayersDead(players))
        {
            CurrentState = BattleState.Defeat;
            BattleEvents.OnBattleLog?.Invoke("DEFEAT!");
            BattleEvents.OnDefeat?.Invoke();
            return;
        }

        AdvanceTurn();
    }

    private bool AreAllEnemiesDead(List<BattleUnit> enemies)
    {
        return enemies.TrueForAll(x => x != null && x.IsDead);
    }

    private bool AreAllPlayersDead(List<BattleUnit> players)
    {
        return players.TrueForAll(x => x != null && x.IsDead);
    }
}
