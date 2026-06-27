using System;
using Fungus;
using UnityEngine;

public class FungusCutsceneStep : CutsceneStep
{
    [SerializeField] private Flowchart flowchart;
    [SerializeField] private string blockName;

    public override void Execute(CutsceneSequenceContext context, Action onComplete)
    {
        if (flowchart == null || string.IsNullOrEmpty(blockName))
        {
            Debug.LogError("FungusCutsceneStep is missing a Flowchart or block name.");
            onComplete?.Invoke();
            return;
        }

        context.Flowchart = flowchart;
        context.FungusBlockName = blockName;

        flowchart.ExecuteBlock(blockName);
        context.Finish();
        onComplete?.Invoke();
    }
}
