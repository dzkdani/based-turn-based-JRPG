using UnityEngine;
using Fungus;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    public bool IsDialogueRunning { get; private set; }

    private Interaction _currentInteraction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartDialogue(Flowchart flowchart, string blockName, Interaction interaction)
    {
        Debug.Log("StartDialogue");
        if (IsDialogueRunning)
            return;

        IsDialogueRunning = true;
        _currentInteraction = interaction;
        flowchart.ExecuteBlock(blockName);

    }

    public void EndDialogue()
    {
        Debug.Log("EndDialogue");
        IsDialogueRunning = false;

        Interaction interaction = _currentInteraction;
        _currentInteraction = null;

        interaction?.EndInteraction();
    }
}