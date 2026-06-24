using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Current Battle Data")]
    public EnemyEncounterData currentEncounter;
    public GameObject playerBattlePrefab;

    private PlayerController _playerController;
    public PlayerController PlayerController => _playerController; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }

        _playerController = playerBattlePrefab.GetComponent<PlayerController>();
    }
}