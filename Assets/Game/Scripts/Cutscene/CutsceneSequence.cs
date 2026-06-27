using System;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneSequence : MonoBehaviour
{
    [SerializeField] private List<CutsceneStep> steps = new List<CutsceneStep>();

    public void Run(CutsceneManager manager, Action onComplete = null)
    {
        if (manager == null)
        {
            Debug.LogError("CutsceneSequence requires a CutsceneManager.");
            onComplete?.Invoke();
            return;
        }

        manager.RunSequence(steps, onComplete);
    }
}
