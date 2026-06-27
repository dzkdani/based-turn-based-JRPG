using UnityEngine;
using Fungus;

public class DialogueManager : MonoBehaviour, IDialogueService, IInteractionService
{
    public static DialogueManager Instance { get; private set; }

    public bool IsDialogueRunning { get; private set; }
    public bool IsInteractionRunning => IsDialogueRunning;

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

    public bool StartInteraction(Interaction interaction, Flowchart flowchart, string blockName)
    {
        return StartDialogue(flowchart, blockName, interaction);
    }

    public bool StartDialogue(Flowchart flowchart, string blockName, Interaction interaction)
    {
        Debug.Log("StartDialogue");
        if (IsDialogueRunning)
            return false;

        if (flowchart == null || string.IsNullOrEmpty(blockName) || interaction == null)
            return false;

        IsDialogueRunning = true;
        _currentInteraction = interaction;
        flowchart.ExecuteBlock(blockName);
        return true;
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