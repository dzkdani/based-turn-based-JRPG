using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    public Interaction CurrentInteraction { get; private set; }

    public void Interact()
    {
        if (CurrentInteraction == null)
            return;

        FaceInteraction(CurrentInteraction);
        CurrentInteraction.StartInteraction();
    }

    public void HidePrompt()
    {
        if (WorldUIManager.Instance != null)
            WorldUIManager.Instance.HideInteraction();
    }

    public void ShowPrompt()
    {
        if (CurrentInteraction != null && WorldUIManager.Instance != null)
            WorldUIManager.Instance.ShowInteraction(CurrentInteraction);
    }

    private void FaceInteraction(Interaction interaction)
    {
        Vector3 lookDirection = interaction.transform.position - transform.position;
        lookDirection.y = 0;

        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Interaction>(out Interaction interaction))
        {
            CurrentInteraction = interaction;
            ShowPrompt();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Interaction>(out Interaction interaction) &&
            CurrentInteraction == interaction)
        {
            CurrentInteraction = null;
            HidePrompt();
        }
    }
}
