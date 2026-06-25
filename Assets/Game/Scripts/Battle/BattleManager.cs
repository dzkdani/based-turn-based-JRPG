    using UnityEngine;
    using System.Collections.Generic;
    using System.Collections;

    public class BattleManager : MonoBehaviour
    {
        public TurnManager TurnManager { get; private set; }
        public BattleState CurrentState;
        public ActionSystem ActionSystem;

        public List<BattleUnit> PlayerUnits;
        public List<BattleUnit> EnemyUnits;
        public BattleUnit CurrentUnit;

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
            ActionSystem = new ActionSystem();
        }

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
                CurrentState = BattleState.WaitingForInput;
                
                BattleEvents.OnPlayerTurn?.Invoke();
            }
            else
            {   
                CurrentState = BattleState.ExecutingAction;

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
            if(CurrentState != BattleState.WaitingForInput)
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
            CurrentState = BattleState.ExecutingAction;

            AttackAction attack = new AttackAction();

            BattleUnit attacker =
                CurrentUnit;

            List<BattleUnit> targets = new List<BattleUnit>();
            targets.Add(EnemyUnits[0]);

            yield return StartCoroutine(attack.Execute(CurrentUnit, targets));
            // yield return new WaitForSeconds(3f); //do some stuff on action

            EndCurrentTurn();
        }

        private IEnumerator EnemyAttack()
        {
            List<BattleUnit> targets = new List<BattleUnit>();
            targets.Add(PlayerUnits[0]);

            AttackAction attack = new AttackAction();

            yield return StartCoroutine(attack.Execute(CurrentUnit, targets));
            yield return new WaitForSeconds(3f); //do some stuff on action

            EndCurrentTurn();
        }
    }