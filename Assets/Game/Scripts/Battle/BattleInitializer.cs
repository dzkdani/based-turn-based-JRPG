using UnityEngine;
using System.Collections.Generic;

public class BattleInitializer : MonoBehaviour
{
    [SerializeField]
    private BattleManager battleManager;
    
    [Header("Spawn Anchors")]
    [SerializeField] private Transform playerSpawnAnchor;
    [SerializeField] private Transform[] enemySpawnAnchors;

    private readonly List<BattleUnit> spawnedPlayers = new();
    private readonly List<BattleUnit> spawnedEnemies = new();

    void Start()
    {
        if (GameManager.Instance == null || GameManager.Instance.currentEncounter == null)
        {
            Debug.LogError("No battle data found to initialize!");
            return;
        }

        SpawnCombatants();
    }

    private void SpawnCombatants()
    {
        GameObject playerInstance = Instantiate(
            GameManager.Instance.playerBattlePrefab, 
            playerSpawnAnchor.position, 
            playerSpawnAnchor.rotation
        );

        BattleUnit playerUnit =
            playerInstance.GetComponent<BattleUnit>();
        spawnedPlayers.Add(playerUnit);
        
        GameManager.Instance.PlayerController.ForceDisableMovement();
        SetupBattleCamera(playerInstance.transform);

        EnemyEncounterData data = GameManager.Instance.currentEncounter;
        for (int i = 0; i < data.enemyPrefabs.Length; i++)
        {
            if (i >= enemySpawnAnchors.Length) break; 

            GameObject enemyInstance = Instantiate(
                data.enemyPrefabs[i], 
                enemySpawnAnchors[i].position, 
                enemySpawnAnchors[i].rotation
            );

            BattleUnit enemyUnit =
                enemyInstance.GetComponent<BattleUnit>();
            spawnedEnemies.Add(enemyUnit);
        }
        
        //Kick off your Turn-Based Logic / Fungus sequence here!
        battleManager.InitializeBattle(
            spawnedPlayers,
            spawnedEnemies);
    }

    private void SetupBattleCamera(Transform playerTransform)
    {
        BattleCameraFollow battleCam = Camera.main.GetComponent<BattleCameraFollow>();
        if (battleCam != null)
        {
            battleCam.target = playerTransform;
        }
    }
}