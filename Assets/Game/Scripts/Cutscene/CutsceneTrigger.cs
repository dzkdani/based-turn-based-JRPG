using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField]
    private CutsceneSequence sequence;

    [SerializeField]
    private bool triggerOnce = true;

    [SerializeField]
    private string playerTag = "Player";

    private bool _triggered;

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered && triggerOnce)
            return;

        if (!other.CompareTag(playerTag))
            return;

        _triggered = true;

        sequence.Run(CutsceneManager.Instance);
    }
}