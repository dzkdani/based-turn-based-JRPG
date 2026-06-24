using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

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