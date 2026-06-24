using UnityEngine;

public class BattleInitializer : MonoBehaviour
{
    [Header("Spawn Anchors")]
    [SerializeField] private Transform playerSpawnAnchor;
    [SerializeField] private Transform[] enemySpawnAnchors;

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
        // 1. Spawn Player
        GameObject playerInstance = Instantiate(
            GameManager.Instance.playerBattlePrefab, 
            playerSpawnAnchor.position, 
            playerSpawnAnchor.rotation
        );

        // Assign the player instance to your Battle Camera (explained below)
        SetupBattleCamera(playerInstance.transform);
        GameManager.Instance.PlayerController.ForceDisableMovement();

        // 2. Spawn Enemies dynamically based on what was passed
        EnemyEncounterData data = GameManager.Instance.currentEncounter;
        
        for (int i = 0; i < data.enemyPrefabs.Length; i++)
        {
            if (i >= enemySpawnAnchors.Length) break; // Safety check

            Instantiate(
                data.enemyPrefabs[i], 
                enemySpawnAnchors[i].position, 
                enemySpawnAnchors[i].rotation
            );
        }
        
        // 3. Kick off your Turn-Based Logic / Fungus sequence here!
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