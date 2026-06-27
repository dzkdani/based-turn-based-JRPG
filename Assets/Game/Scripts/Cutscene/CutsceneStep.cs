using System;
using UnityEngine;

public abstract class CutsceneStep : ScriptableObject
{
    public abstract void Execute(CutsceneSequenceContext context, Action onComplete);
}
