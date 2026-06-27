using UnityEngine;

public class WorldUIManager : MonoBehaviour
{
    public static WorldUIManager Instance { get; private set; }

    [SerializeField]
    private InteractionPromptUI interactionPrompt;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowInteraction(Interaction interaction)
    {
        interactionPrompt.Show(interaction);
    }

    public void HideInteraction()
    {
        interactionPrompt.Hide();
    }
}