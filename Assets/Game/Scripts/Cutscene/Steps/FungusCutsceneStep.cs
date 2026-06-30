using System;
using Fungus;
using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene/Fungus Step")]
public class FungusCutsceneStep : CutsceneStep
{
    [SerializeField] string flowchartID;
    [SerializeField] private string blockName;

    private Flowchart flowchart;
    
    public void SetFlowchart(Flowchart newFlowchart, string newBlockName)
    {
        flowchart = newFlowchart;
        blockName = newBlockName;
    }

    public override void Execute(CutsceneSequenceContext context, Action onComplete)
    {
        flowchart = context.Registry.GetFlowchart(flowchartID);
        
        if (flowchart == null || string.IsNullOrEmpty(blockName))
        {
            Debug.LogError("FungusCutsceneStep is missing a Flowchart or block name.");
            onComplete?.Invoke();
            return;
        }

        var block = flowchart.FindBlock(blockName);
        if (block == null)
        {
            Debug.LogError($"FungusCutsceneStep could not find block '{blockName}'.", flowchart);
            onComplete?.Invoke();
            return;
        }

        if (!flowchart.ExecuteBlock(block, 0, onComplete))
        {
            onComplete?.Invoke();
        }
    }

    public override void Skip()
    {
        if (flowchart != null)
        {
            flowchart.StopAllBlocks();
        }
    }
}
