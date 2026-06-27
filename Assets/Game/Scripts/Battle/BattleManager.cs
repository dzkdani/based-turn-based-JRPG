using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public TurnManager TurnManager { get; private set; }
    public BattleState CurrentState => battleFlow.CurrentState;
    public BattleUnit CurrentUnit => battleFlow.CurrentUnit;

    public List<BattleUnit> PlayerUnits;
    public List<BattleUnit> EnemyUnits;

    private TargetSystem targetSystem;
    private ActionSystem actionSystem;
    private BattleAISystem aiSystem;
    private BattleFlow battleFlow;

    private void OnEnable()
    {
        BattleEvents.OnAttackPressed += OnAttackPressed;
        BattleEvents.OnTargetSelected += OnTargetSelected;
        BattleEvents.OnActionSelected += OnActionSelected;
    }

    private void OnDisable()
    {
        BattleEvents.OnAttackPressed -= OnAttackPressed;
        BattleEvents.OnTargetSelected -= OnTargetSelected;
        BattleEvents.OnActionSelected -= OnActionSelected;
        if (battleFlow != null)
            battleFlow.EnemyTurnStarted -= OnEnemyTurnStarted;
    }

    private void Awake()
    {
        TurnManager = new TurnManager();
        targetSystem = new TargetSystem();
        actionSystem = new ActionSystem();
        aiSystem = new BattleAISystem();
        battleFlow = new BattleFlow(TurnManager, actionSystem, aiSystem, targetSystem);
        battleFlow.EnemyTurnStarted += OnEnemyTurnStarted;
    }

    public void InitializeBattle(List<BattleUnit> players, List<BattleUnit> enemies)
    {
        PlayerUnits = players;
        EnemyUnits = enemies;

        WorldEvents.OnWorldFreeze?.Invoke();
        battleFlow.StartBattle(PlayerUnits, EnemyUnits);
    }

    public void EndBattle()
    {
        WorldEvents.OnWorldUnfreeze?.Invoke();
        // StartCoroutine(BattleEndSequence());
    }

    private IEnumerator BattleEndSequence()
    {
        yield return new WaitForSeconds(1f);

        // Victory UI

        yield return new WaitForSeconds(2f);

        // Fungus Victory Dialogue

        yield return new WaitForSeconds(1f);

        // Load World Scene
    }

    private void OnAttackPressed()
    {
        if(CurrentState != BattleState.WaitingForCommand)
            return;

        BattleActionSO attackAction = CurrentUnit?.Data.Skills.FirstOrDefault(a => a.ActionType == BattleActionType.Attack);
        BattleEvents.OnActionSelected?.Invoke(attackAction);
    }

    private void OnActionSelected(BattleActionSO action)
    {
        battleFlow.OnPlayerActionSelected(action);
    }

    private void OnTargetSelected(BattleUnit target)
    {
        StartCoroutine(battleFlow.HandleTargetSelected(target, PlayerUnits, EnemyUnits));
    }

    private void OnEnemyTurnStarted()
    {
        StartCoroutine(battleFlow.PerformEnemyTurn(PlayerUnits, EnemyUnits));
    }
}