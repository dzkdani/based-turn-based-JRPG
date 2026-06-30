using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }

    [Header("Gameplay lock")]
    [SerializeField] private List<MonoBehaviour> lockables = new List<MonoBehaviour>();

    public bool IsPlaying { get; private set; }
    public PlayableDirector CurrentDirector { get; private set; }

    public event Action OnCutsceneStarted;
    public event Action OnCutsceneFinished;

    private readonly Queue<CutsceneStep> _stepQueue = new Queue<CutsceneStep>();
    private Action _cutsceneComplete;
    private Action _directorComplete;
    private bool _isSequenceRunning;
    private CutsceneStep _currentStep; // Track the active step for skipping

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RunSequence(IEnumerable<CutsceneStep> steps, Action onComplete = null)
    {
        if (IsPlaying)
        {
            Debug.LogWarning("A cutscene sequence is already playing.");
            return;
        }

        _stepQueue.Clear();
        foreach (var step in steps)
        {
            if (step != null) _stepQueue.Enqueue(step);
        }

        if (_stepQueue.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        _isSequenceRunning = true;
        BeginCutscene(onComplete);
        AdvanceSequence();
    }

    private void AdvanceSequence()
    {
        if (!_isSequenceRunning) return;

        if (_stepQueue.Count == 0)
        {
            CompleteCutscene();
            return;
        }

        var context = new CutsceneSequenceContext { Manager = this , Registry = CutsceneRegistry.Instance};

        _currentStep = _stepQueue.Dequeue();
        _currentStep.Execute(context, AdvanceSequence);
    }

    public void PlayTimelineStep(PlayableDirector director, Action onComplete)
    {
        CurrentDirector = director;
        _directorComplete = onComplete;

        CurrentDirector.stopped -= HandleTimelineFinished;
        CurrentDirector.stopped += HandleTimelineFinished;

        CurrentDirector.time = 0;
        CurrentDirector.Play();
    }

    private void HandleTimelineFinished(PlayableDirector director)
    {
        if (director != CurrentDirector) return;
        director.stopped -= HandleTimelineFinished;
        CurrentDirector = null;

        var onComplete = _directorComplete;
        _directorComplete = null;
        onComplete?.Invoke();
    }

    public void Skip()
    {
        if (!IsPlaying) return;

        // 1. If the specific step has custom skip logic (like Fungus), use it
        if (_currentStep != null)
        {
            _currentStep.Skip();
        }

        // 2. If a timeline is running, fast-forward it
        if (CurrentDirector != null)
        {
            CurrentDirector.time = CurrentDirector.duration;
            CurrentDirector.Evaluate();
            CurrentDirector.Stop(); // This naturally triggers HandleTimelineFinished -> AdvanceSequence
        }
    }

    private void BeginCutscene(Action onComplete)
    {
        _cutsceneComplete = onComplete;
        IsPlaying = true;
        LockGameplay();
        OnCutsceneStarted?.Invoke();
    }

    private void CompleteCutscene()
    {
        _isSequenceRunning = false;
        _stepQueue.Clear();
        _currentStep = null;
        UnlockGameplay();

        IsPlaying = false;
        OnCutsceneFinished?.Invoke();
        
        _cutsceneComplete?.Invoke();
        _cutsceneComplete = null;
    }

    private void LockGameplay()
    {
        foreach (var behaviour in lockables)
        {
            if (behaviour is ICutsceneLockable lockable) lockable.LockForCutscene();
        }
    }

    private void UnlockGameplay()
    {
        foreach (var behaviour in lockables)
        {
            if (behaviour is ICutsceneLockable lockable) lockable.UnlockFromCutscene();
        }
    }
}