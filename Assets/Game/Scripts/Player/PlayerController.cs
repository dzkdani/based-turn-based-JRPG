using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Matrix4x4 isoMatrix;
    private float horizontal;
    private float vertical;
    private Vector3 input;
    private Vector3 isoInput;

    void Start()
    {
        isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void HandleInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal"); 
        vertical = Input.GetAxisRaw("Vertical");     

        input = new Vector3(horizontal, 0, vertical).normalized;

        isoInput = isoMatrix.MultiplyPoint3x4(input);
    }

    private void Move()
    {
        if (input.magnitude > 0.1f)
        {
            transform.position += isoInput * moveSpeed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(isoInput, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
        }
    }
}