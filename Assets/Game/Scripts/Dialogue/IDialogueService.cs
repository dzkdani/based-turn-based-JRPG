using Fungus;

public interface IDialogueService
{
    bool IsDialogueRunning { get; }
    bool StartDialogue(Flowchart flowchart, string blockName, Interaction interaction);
    void EndDialogue();
}
