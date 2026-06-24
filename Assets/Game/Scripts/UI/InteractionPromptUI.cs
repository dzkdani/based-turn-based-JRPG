using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private Interaction currentTarget;

    public void Show(Interaction target)
    {
        currentTarget = target;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (this == null) return;
        
        currentTarget = null;
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (currentTarget == null)
            return;
            
        transform.position = Camera.main.WorldToScreenPoint(currentTarget.PromptAnchor.position);
    }
}