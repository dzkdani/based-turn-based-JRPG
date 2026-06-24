using UnityEngine;

[System.Serializable]
public class EnemyEncounterData
{
    public string encounterID;
    public GameObject[] enemyPrefabs; // Array of enemies to spawn in this battle
    public Vector3[] spawnPositions;   // Where they should appear on the battle stage
}