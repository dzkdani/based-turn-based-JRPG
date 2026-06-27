using System;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance { get; private set; }

    [Header("Objects to lock during cutscene")]
    [SerializeField] private MonoBehaviour[] lockables;

    public bool IsPlaying { get; private set; }

    public PlayableDirector CurrentDirector { get; private set; }

    public event Action OnCutsceneStarted;
    public event Action OnCutsceneFinished;

    private Action _onComplete;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Play(PlayableDirector director, Action onComplete = null)
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
        _onComplete = onComplete;

        LockGameplay();

        IsPlaying = true;

        CurrentDirector.stopped += HandleTimelineFinished;
        CurrentDirector.Play();

        OnCutsceneStarted?.Invoke();
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

        CurrentDirector?.Stop();
    }

    public void Skip()
    {
        if (!IsPlaying)
            return;

        if (CurrentDirector == null)
            return;

        CurrentDirector.time = CurrentDirector.duration;
        CurrentDirector.Evaluate();
        CurrentDirector.Stop();
    }

    private void HandleTimelineFinished(PlayableDirector director)
    {
        if (director != CurrentDirector)
            return;

        director.stopped -= HandleTimelineFinished;

        UnlockGameplay();

        IsPlaying = false;

        OnCutsceneFinished?.Invoke();

        _onComplete?.Invoke();

        _onComplete = null;
        CurrentDirector = null;
    }

    private void LockGameplay()
    {
        foreach (var behaviour in lockables)
        {
            if (behaviour == null)
                continue;

            // if (behaviour is ICutsceneLockable lockable)
            //     lockable.LockForCutscene();
        }
    }

    private void UnlockGameplay()
    {
        foreach (var behaviour in lockables)
        {
            if (behaviour == null)
                continue;

            // if (behaviour is ICutsceneLockable lockable)
            //     lockable.UnlockFromCutscene();
        }
    }
}