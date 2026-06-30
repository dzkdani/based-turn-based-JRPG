using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CutsceneSequence : MonoBehaviour
{
    [SerializeField] private List<CutsceneStep> steps = new List<CutsceneStep>();

    [Button]
    public void Run()
    {
        Run(CutsceneManager.Instance);
    }

    public void Run(CutsceneManager manager, Action onComplete = null)
    {
        if (manager == null)
        {
            Debug.LogError("CutsceneSequence requires a CutsceneManager.");
            onComplete?.Invoke();
            return;
        }

        manager.RunSequence(steps, () =>
        {
            Debug.Log("[Cutscene] Sequence complete!");
        });
    }
}
