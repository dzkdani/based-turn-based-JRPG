using UnityEngine;
using System.Collections.Generic;

public class BattleInitializer : MonoBehaviour
{
    [SerializeField]
    private BattleManager battleManager;

    [SerializeField]
    private BattleCameraFollow battleCamera;

    [Header("Spawn Anchors")]
    [SerializeField]
    private Transform playerSpawnAnchor;

    [SerializeField]
    private Transform[] enemySpawnAnchors;

    private readonly List<BattleUnit> spawnedPlayers = new();
    private readonly List<BattleUnit> spawnedEnemies = new();

    private void Start()
    {
        ValidateSetup();

        SpawnCombatants();

        battleManager.InitializeBattle(spawnedPlayers, spawnedEnemies);
    }

    private void ValidateSetup()
    {
        if(GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");

            enabled = false;
            return;
        }

        if(GameManager.Instance.CurrentEncounter == null)
        {
            Debug.LogError("Current Encounter not assigned!");

            enabled = false;
            return;
        }
    }

    private void SpawnCombatants()
    {
        SpawnPlayer();
        SpawnEnemies();
    }

    private void SpawnPlayer()
    {
        GameObject playerInstance =
            Instantiate(
                GameManager.Instance.PlayerBattlePrefab,
                playerSpawnAnchor.position,
                playerSpawnAnchor.rotation);

        if(!playerInstance.TryGetComponent(out BattleUnit playerUnit))
        {
            Debug.LogError("Player prefab missing BattleUnit!");
            return;
        }

        spawnedPlayers.Add(playerUnit);

        GameManager.Instance.CurrentBattlePlayer = playerUnit;

        SetupBattleCamera(playerInstance.transform);
    }

    private void SpawnEnemies()
    {
        EnemyEncounterData encounter = GameManager.Instance.CurrentEncounter;

        if(encounter.enemyPrefabs.Length >
           enemySpawnAnchors.Length)
        {
            Debug.LogWarning(
                "More enemies than spawn anchors.");
        }

        for(int i = 0; i < encounter.enemyPrefabs.Length; i++)
        {
            if(i >= enemySpawnAnchors.Length)
                break;

            GameObject enemyInstance =
                Instantiate(
                    encounter.enemyPrefabs[i],
                    enemySpawnAnchors[i].position,
                    enemySpawnAnchors[i].rotation);

            if(!enemyInstance.TryGetComponent(out BattleUnit enemyUnit))
            {
                Debug.LogError($"{enemyInstance.name} missing BattleUnit");
                continue;
            }

            spawnedEnemies.Add(enemyUnit);
        }
    }

    private void SetupBattleCamera(Transform playerTransform)
    {
        if(battleCamera == null)
            return;

        battleCamera.target = playerTransform;
    }
}