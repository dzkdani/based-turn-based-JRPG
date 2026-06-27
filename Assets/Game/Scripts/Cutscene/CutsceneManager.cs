using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }

    [Header("Gameplay lock")]
    [SerializeField] private List<MonoBehaviour> lockables = new List<MonoBehaviour>();

    [Header("Runtime state")]
    public bool IsPlaying { get; private set; }
    public PlayableDirector CurrentDirector { get; private set; }
    public CutsceneSequenceContext CurrentContext { get; private set; }

    public event Action OnCutsceneStarted;
    public event Action OnCutsceneFinished;

    private readonly Queue<CutsceneStep> _stepQueue = new Queue<CutsceneStep>();
    private Action _sequenceComplete;
    private bool _isSequenceRunning;
    private bool _isDirectorOwnedBySequence;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PlayDirector(PlayableDirector director, Action onComplete = null)
    {
        if (IsPlaying)
        {
            Debug.LogWarning("A cutscene is already playing.");
            return;
        }

        if (director == null)
        {
            Debug.LogError("PlayableDirector is NULL.");
            return;
        }

        CurrentDirector = director;
        _sequenceComplete = onComplete;
        _isDirectorOwnedBySequence = true;

        LockGameplay();
        IsPlaying = true;

        CurrentDirector.stopped += HandleTimelineFinished;
        CurrentDirector.Play();

        OnCutsceneStarted?.Invoke();
    }

    public void RunSequence(IEnumerable<CutsceneStep> steps, Action onComplete = null)
    {
        if (IsPlaying)
        {
            Debug.LogWarning("A cutscene sequence is already playing.");
            return;
        }

        if (steps == null)
        {
            Debug.LogError("Cutscene sequence steps are NULL.");
            onComplete?.Invoke();
            return;
        }

        _stepQueue.Clear();
        foreach (var step in steps)
        {
            if (step != null)
                _stepQueue.Enqueue(step);
        }

        _sequenceComplete = onComplete;
        _isSequenceRunning = true;
        LockGameplay();
        IsPlaying = true;

        OnCutsceneStarted?.Invoke();
        AdvanceSequence();
    }

    public void Pause()
    {
        if (!IsPlaying)
            return;

        CurrentDirector?.Pause();
    }

    public void Resume()
    {
        if (!IsPlaying)
            return;

        CurrentDirector?.Resume();
    }

    public void Stop()
    {
        if (!IsPlaying)
            return;

        if (_isDirectorOwnedBySequence && CurrentDirector != null)
        {
            CurrentDirector.Stop();
        }
        else
        {
            CompleteSequence();
        }
    }

    public void Skip()
    {
        if (!IsPlaying)
            return;

        if (CurrentDirector != null)
        {
            CurrentDirector.time = CurrentDirector.duration;
            CurrentDirector.Evaluate();
            CurrentDirector.Stop();
        }
        else
        {
            CompleteSequence();
        }
    }

    private void AdvanceSequence()
    {
        if (!_isSequenceRunning || _stepQueue.Count == 0)
        {
            CompleteSequence();
            return;
        }

        var step = _stepQueue.Dequeue();
        var context = new CutsceneSequenceContext
        {
            Manager = this,
            OnFinished = () => AdvanceSequence()
        };

        CurrentContext = context;
        step.Execute(context, () =>
        {
            if (context.OnFinished != null)
            {
                context.OnFinished();
            }
        });
    }

    private void HandleTimelineFinished(PlayableDirector director)
    {
        if (director != CurrentDirector)
            return;

        director.stopped -= HandleTimelineFinished;
        FinishCurrentCutscene();
    }

    private void FinishCurrentCutscene()
    {
        UnlockGameplay();

        IsPlaying = false;
        CurrentDirector = null;
        CurrentContext = null;
        _isDirectorOwnedBySequence = false;

        OnCutsceneFinished?.Invoke();
        _sequenceComplete?.Invoke();
        _sequenceComplete = null;
    }

    private void CompleteSequence()
    {
        _isSequenceRunning = false;
        _stepQueue.Clear();
        UnlockGameplay();

        IsPlaying = false;
        CurrentDirector = null;
        CurrentContext = null;
        _isDirectorOwnedBySequence = false;

        OnCutsceneFinished?.Invoke();
        _sequenceComplete?.Invoke();
        _sequenceComplete = null;
    }

    private void LockGameplay()
    {
        foreach (var behaviour in lockables)
        {
            if (behaviour == null)
                continue;

            if (behaviour is ICutsceneLockable lockable)
            {
                lockable.LockForCutscene();
            }
        }
    }

    private void UnlockGameplay()
    {
        foreach (var behaviour in lockables)
        {
            if (behaviour == null)
                continue;

            if (behaviour is ICutsceneLockable lockable)
            {
                lockable.UnlockFromCutscene();
            }
        }
    }
}