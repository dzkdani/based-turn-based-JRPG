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

    private Vector3? _cutsceneTarget = null;
    private float _cutsceneSpeed;


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
        else if (_cutsceneTarget.HasValue)
        {
            // DoCutscene
        }
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
            Vector3 lookDirection = _currentInteraction.transform.position - transform.position;
            lookDirection.y = 0; 

            if (lookDirection != Vector3.zero)
            {
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
            UIManager.Instance.ShowInteraction(interaction);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Interaction>(out Interaction interaction))
        {
            if (_currentInteraction == interaction)
            {
                _currentInteraction = null;
                UIManager.Instance.HideInteraction();
            }
        }
    }

    public void ForceDisableMovement() => DisableMovement();
    public void ForceEnableMovement() => EnableMovement();

    private void DisableMovement()
    {
        Debug.Log("DisableMovement");
        CanMove = false;
        UIManager.Instance.HideInteraction();
    }
    private void EnableMovement()
    {
        Debug.Log("EnableMovement");
        CanMove = true;
        if (_currentInteraction != null)
            UIManager.Instance.ShowInteraction(_currentInteraction);
    }
}