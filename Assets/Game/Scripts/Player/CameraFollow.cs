using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;        
    [Header("Follow Settings")]
    public float smoothTime = 0.2f;     
    private Vector3 _offset;
    private Vector3 _currentVelocity = Vector3.zero;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Camera Follow target is not assigned!");
            return;
        }

        _offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + _offset;

        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPosition, 
            ref _currentVelocity, 
            smoothTime
        );
    }
}