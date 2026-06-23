using UnityEngine;
using System;
using Fungus; 

public class Interaction : MonoBehaviour
{
    [Header("Fungus")]
    [SerializeField] private Flowchart flowchart;      
    [SerializeField] private string blockName;   
    
    public static event Action OnInteractionStart;
    public static event Action OnInteractionEnd;

    private bool _isInteracting = false;

    public void StartInteraction()
    {
        if (_isInteracting) 
            return;

        _isInteracting = true;
        OnInteractionStart?.Invoke();

        if (flowchart != null && !string.IsNullOrEmpty(blockName))
            flowchart.ExecuteBlock(blockName);
        else
            EndInteraction();
    }

    public void EndInteraction()
    {
        _isInteracting = false;
        OnInteractionEnd?.Invoke();
    }
}