using System;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineCutsceneStep : CutsceneStep
{
    [SerializeField] private PlayableDirector director;

    public override void Execute(CutsceneSequenceContext context, Action onComplete)
    {
        if (director == null)
        {
            Debug.LogError("TimelineCutsceneStep is missing a PlayableDirector.");
            onComplete?.Invoke();
            return;
        }

        context.Director = director;
        context.Manager.PlayDirector(director, () =>
        {
            context.Finish();
            onComplete?.Invoke();
        });
    }
}
