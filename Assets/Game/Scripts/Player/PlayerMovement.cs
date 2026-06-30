using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private readonly Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void Move(Vector3 inputDirection)
    {
        if (inputDirection.magnitude <= 0.1f)
            return;

        Vector3 isoDirection = _isoMatrix.MultiplyPoint3x4(inputDirection);
        Vector3 targetPosition = transform.position + (isoDirection * moveSpeed * Time.fixedDeltaTime);

        _rigidbody.MovePosition(targetPosition);

        Quaternion targetRotation = Quaternion.LookRotation(isoDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
    }
}
