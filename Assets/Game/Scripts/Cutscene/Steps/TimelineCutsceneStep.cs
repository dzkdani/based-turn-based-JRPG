using System;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(menuName = "Cutscene/Timeline Step")]
public class TimelineCutsceneStep : CutsceneStep
{
    [SerializeField] private string timelineID;

    private PlayableDirector director;

    public void SetDirector(PlayableDirector newDirector)
    {
        director = newDirector;
    }

    public override void Execute(CutsceneSequenceContext context, Action onComplete)
    {
        director = context.Registry.GetTimeline(timelineID);

        if (director == null)
        {
            Debug.LogError("TimelineCutsceneStep is missing a PlayableDirector.");
            onComplete?.Invoke();
            return;
        }

        context.Manager.PlayTimelineStep(director, onComplete);
    }
}
