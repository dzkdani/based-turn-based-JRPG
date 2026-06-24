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
    
    public static event Action OnInteractionStart;
    public static event Action OnInteractionEnd;

    private bool _isInteracting = false;

    public virtual void StartInteraction()
    {
        if (_isInteracting) 
            return;

        _isInteracting = true;
        OnInteractionStart?.Invoke();

        if (flowchart != null && !string.IsNullOrEmpty(blockName))
        {
            DialogueManager.Instance.StartDialogue(
                flowchart,
                blockName,
                this);
        }
        else
            EndInteraction();
    }

    public virtual void EndInteraction()
    {
        _isInteracting = false;
        OnInteractionEnd?.Invoke();
    }
}