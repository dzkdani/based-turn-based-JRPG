using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Encounter")]
    public EnemyEncounterData CurrentEncounter;

    [Header("Battle Setup")]
    public GameObject PlayerBattlePrefab;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetEncounter(EnemyEncounterData encounter)
    {
        CurrentEncounter = encounter;
    }
}