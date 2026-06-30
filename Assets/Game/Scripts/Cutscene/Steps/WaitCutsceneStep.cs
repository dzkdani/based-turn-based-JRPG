using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Cutscene/Wait Step")]
public class WaitCutsceneStep : CutsceneStep
{
    [SerializeField] private float duration = 0.5f;

    public override void Execute(CutsceneSequenceContext context, Action onComplete)
    {
        context.Manager.StartCoroutine(WaitRoutine(onComplete));
    }

    private System.Collections.IEnumerator WaitRoutine(Action onComplete)
    {
        yield return new WaitForSeconds(duration);
        onComplete?.Invoke();
    }
}
