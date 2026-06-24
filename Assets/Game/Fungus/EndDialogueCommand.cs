using Fungus;

[CommandInfo("Game",
             "End Dialogue",
             "Ends current dialogue")]
public class EndDialogueCommand : Command
{
    public override void OnEnter()
    {
        DialogueManager.Instance.EndDialogue();
        Continue();
    }
}