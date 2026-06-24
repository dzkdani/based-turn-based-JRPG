using UnityEngine;

public class BattleCameraFollow : MonoBehaviour
{
    [Header("Dynamic Targets")]
    public Transform target; // Automatically assigned by BattleInitializer
    
    [Header("Placement Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 3, -6); // Behind and above
    [SerializeField] private float positionSmoothTime = 0.3f;
    [SerializeField] private float lookSmoothSpeed = 5.0f;

    private Vector3 _currentVelocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // 1. Calculate ideal position relative to the target's orientation
        Vector3 targetPosition = target.TransformPoint(offset);

        // 2. Smoothly damp the position
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPosition, 
            ref _currentVelocity, 
            positionSmoothTime
        );

        // 3. Smoothly rotate to stare directly at the target
        Vector3 directionToTarget = target.position - transform.position;
        // Optional: adjust target height to look at chest level rather than feet
        directionToTarget.y += 1.2f; 

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lookSmoothSpeed * Time.deltaTime);
    }
}