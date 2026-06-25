using UnityEngine;

[System.Serializable]
public class EnemyEncounterData
{
    public string encounterID;
    public GameObject[] enemyPrefabs; 
    public Vector3[] spawnPositions;   
}