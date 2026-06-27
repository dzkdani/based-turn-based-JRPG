using System;
using UnityEngine;
using UnityEngine.Playables;
using Fungus;

public class CutsceneSequenceContext
{
    public CutsceneManager Manager { get; set; }
    public PlayableDirector Director { get; set; }
    public Flowchart Flowchart { get; set; }
    public string FungusBlockName { get; set; }
    public Action OnFinished { get; set; }

    public void Finish()
    {
        OnFinished?.Invoke();
    }
}
