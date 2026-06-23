using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    public bool CanMove { get; private set; } = true;

    private Matrix4x4 _isoMatrix;
    private Vector3 _inputDirection;
    private Vector3 _isoDirection;
    private Rigidbody _rigidbody;

    private Interaction _currentInteraction; 

    private void OnEnable()
    {
        Interaction.OnInteractionStart += DisableMovement;
        Interaction.OnInteractionEnd += EnableMovement;
    }

    private void OnDisable()
    {
        Interaction.OnInteractionStart -= DisableMovement;
        Interaction.OnInteractionEnd -= EnableMovement;
    }

    private void Start()
    {
        _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Input();
        Interact();
    }

    private void FixedUpdate()
    {
        if (CanMove)
            Move();
    }

    private void Input()
    {
        if (!CanMove)
        {
            _inputDirection = Vector3.zero;
            _isoDirection = Vector3.zero;
            return;
        }

        float horizontal = UnityEngine.Input.GetAxisRaw("Horizontal"); 
        float vertical = UnityEngine.Input.GetAxisRaw("Vertical");     

        _inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        _isoDirection = _isoMatrix.MultiplyPoint3x4(_inputDirection);
    }

    private void Interact()
    {
        if (CanMove && _currentInteraction != null && UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            _currentInteraction.StartInteraction();

            // Calculate direction to the NPC (ignoring height differences)
            Vector3 lookDirection = _currentInteraction.transform.position - transform.position;
            lookDirection.y = 0; 

            if (lookDirection != Vector3.zero)
            {
                // Instantly snap or smoothly rotate the player to face the NPC
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }

            _currentInteraction.StartInteraction(); 
        }
    }

    private void Move()
    {
        if (_inputDirection.magnitude > 0.1f)
        {
            Vector3 targetPosition = transform.position + (_isoDirection * moveSpeed * Time.fixedDeltaTime);
            
            _rigidbody.MovePosition(targetPosition);

            Quaternion targetRotation = Quaternion.LookRotation(_isoDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.15f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Interaction>(out Interaction interaction))
        {
            _currentInteraction = interaction;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Interaction>(out Interaction interaction))
        {
            if (_currentInteraction == interaction)
            {
                _currentInteraction = null;
            }
        }
    }

    private void DisableMovement() => CanMove = false;
    private void EnableMovement() => CanMove = true;
}