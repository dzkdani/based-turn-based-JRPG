using UnityEngine;
using System;
using Fungus; 

public class Interaction : MonoBehaviour
{
    [Header("Fungus")]
    [SerializeField] private Flowchart flowchart;      
    [SerializeField] private string blockName;   
    
    [Header("UI")]
    [SerializeField] private Transform promptAnchor;
    public Transform PromptAnchor => promptAnchor;

    private IInteractionService InteractionService => DialogueManager.Instance;

    public static event Action OnInteractionStart;
    public static event Action OnInteractionEnd;

    private bool _isInteracting = false;
    public bool IsInteracting => _isInteracting;

    public virtual void StartInteraction()
    {
        if (_isInteracting)
            return;

        if (flowchart != null && !string.IsNullOrEmpty(blockName))
        {
            if (InteractionService == null)
            {
                Debug.LogError("No IInteractionService assigned to Interaction.", this);
                return;
            }

            if (!InteractionService.StartInteraction(this, flowchart, blockName))
                return;

            _isInteracting = true;
            OnInteractionStart?.Invoke();
            return;
        }

        _isInteracting = true;
        OnInteractionStart?.Invoke();
        EndInteraction();
    }

    public virtual void EndInteraction()
    {
        if (!_isInteracting)
            return;

        _isInteracting = false;
        OnInteractionEnd?.Invoke();
    }
}