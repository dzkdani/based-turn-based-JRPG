using Fungus;

public interface IInteractionService
{
    bool IsInteractionRunning { get; }
    bool StartInteraction(Interaction interaction, Flowchart flowchart, string blockName);
}
