using UnityEngine;
using UnityEngine.Events;
using Fungus; 

public class NPCInteraction : MonoBehaviour
{
    public Flowchart flowchart;      
    public string blockName;   
    
    private PlayerController _player;
    private bool _playerInRange = false;
    private bool _isInteracting = false;


    [Header("Events")]
    public UnityEvent onInteractionStart;
    public UnityEvent onInteractionEnd;


    void Update()
    {
        PlayerChecker();
    }

    private void PlayerChecker()
    {
        if (!_playerInRange || _isInteracting)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isInteracting = true;
            flowchart.ExecuteBlock(blockName);
            onInteractionStart?.Invoke();
        }
    }

    public void EndInteraction()
    {
        _isInteracting = false;
        onInteractionEnd?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            Debug.Log("Player entered NPC interaction range.");
            _playerInRange = true;
            _player = player;
        }
    }

    // private void OnTriggerStay(Collider other)
    // {
    //     if (other.TryGetComponent<PlayerController>(out PlayerController player))
    //     {
    //         _playerInRange = true;
    //         _player = player;
    //     }
    // }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out PlayerController player))
        {
            Debug.Log("Player exited NPC interaction range.");
            _playerInRange = false;
            _player = null;
        }
    }
}  