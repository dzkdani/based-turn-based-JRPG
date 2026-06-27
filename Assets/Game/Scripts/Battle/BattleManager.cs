using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class BattleManager : MonoBehaviour
    {
        public TurnManager TurnManager { get; private set; }
        public BattleState CurrentState;
        private TargetSystem targetSystem;

        public List<BattleUnit> PlayerUnits;
        public List<BattleUnit> EnemyUnits;
        public BattleUnit CurrentUnit;

        private BattleActionSO currentAction;
        private BattleUnit currentTarget;

        private void OnEnable()
        {
            BattleEvents.OnAttackPressed += OnAttackPressed;
        }

        private void OnDisable()
        {
            BattleEvents.OnAttackPressed -= OnAttackPressed;
        }

        private void Awake()
        {
            TurnManager = new TurnManager();
            targetSystem = new TargetSystem();
        }

        public void InitializeBattle(List<BattleUnit> players, List<BattleUnit> enemies)
        {
            PlayerUnits = players;
            EnemyUnits = enemies;

            StartBattle();
        }

        public void StartBattle()
        {
            WorldEvents.OnWorldFreeze?.Invoke();

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
            CurrentUnit = TurnManager.GetCurrentUnit();
            
            BattleEvents.OnBattleLog?.Invoke($"{CurrentUnit.Data.Name} Turn");

            if(CurrentUnit.Team == Team.Player)
            {
                CurrentState = BattleState.WaitingForCommand;
                
                BattleEvents.OnPlayerTurn?.Invoke();
            }
            else
            {   
                CurrentState = BattleState.Executing;

                BattleEvents.OnEnemyTurn?.Invoke();

                StartCoroutine(EnemyAttack());
            }
            
        }

        public void CheckBattleResult()
        {
            if (AreAllEnemiesDead())
            {
                CurrentState = BattleState.Victory;

                BattleEvents.OnBattleLog?.Invoke($"VICTORY!");

                BattleEvents.OnVictory?.Invoke();

                EndBattle();
                return;
            }

            if (AreAllPlayersDead())
            {
                CurrentState = BattleState.Defeat;

                BattleEvents.OnBattleLog?.Invoke($"DEFEAT!");

                BattleEvents.OnDefeat?.Invoke();

                EndBattle();
                return;
            }
        }

        private void EndCurrentTurn()
        {
            CheckBattleResult();

            if(CurrentState == BattleState.Victory ||
            CurrentState == BattleState.Defeat)
            {
                return;
            }

            TurnManager.AdvanceTurn();
            StartTurn();
        }

        public bool AreAllEnemiesDead()
        {
            return EnemyUnits.TrueForAll(x => x.IsDead);
        }

        public bool AreAllPlayersDead()
        {
            return PlayerUnits.TrueForAll(x => x.IsDead);
        }

        public void EndBattle()
        {
            Debug.Log($"Battle End");
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

            if(CurrentUnit.Team != Team.Player)
                return;

            StartCoroutine(PlayerAttack());
        }

        //TEMP TEST
        private void Update()
        {
            if(CurrentState == BattleState.Victory ||
            CurrentState == BattleState.Defeat)
            {
                return;
            }
        }

        private IEnumerator PlayerAttack()
        {
            CurrentState = BattleState.Executing;

            AttackAction attack = new AttackAction();

            BattleActionSO attackAction = CurrentUnit.Data.Skills.FirstOrDefault(a => a.ActionType == BattleActionType.Attack);

            List<BattleUnit> targets =
                targetSystem.GetTargets(
                    CurrentUnit,
                    attackAction.TargetType,
                    PlayerUnits,
                    EnemyUnits);

            yield return StartCoroutine(
                attack.Execute(CurrentUnit, targets, attackAction));

            EndCurrentTurn();
        }

        private IEnumerator EnemyAttack()
        {
            List<BattleUnit> targets = new List<BattleUnit>();
            targets.Add(PlayerUnits[0]);

            AttackAction attack = new AttackAction();

            BattleActionSO attackAction = CurrentUnit.Data.Skills.FirstOrDefault(a => a.ActionType == BattleActionType.Attack);

            yield return StartCoroutine(attack.Execute(CurrentUnit, targets, attackAction));
            BattleEvents.OnBattleLog?.Invoke($"{CurrentUnit.Data.Name} dealt {CurrentUnit.Data.CurrentAtk} atk to {targets[0].Data.Name}");
            yield return new WaitForSeconds(3f); //do some stuff on action

            EndCurrentTurn();
        }
    }