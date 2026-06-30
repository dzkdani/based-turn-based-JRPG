using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteractionHandler))]
public class PlayerController : MonoBehaviour, ICutsceneLockable
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    public bool CanMove { get; private set; } = true;

    private PlayerInput _inputReader;
    private PlayerMovement _movement;
    private PlayerInteractionHandler _interactionHandler;

    private void Awake()
    {
        _inputReader = GetOrAddComponent<PlayerInput>();
        _movement = GetOrAddComponent<PlayerMovement>();
        _interactionHandler = GetOrAddComponent<PlayerInteractionHandler>();

        _movement.SetMoveSpeed(moveSpeed);
    }

    private void OnEnable()
    {
        Interaction.OnInteractionStart += DisableMovement;
        Interaction.OnInteractionEnd += EnableMovement;

        WorldEvents.OnWorldFreeze += DisableMovement;
        WorldEvents.OnWorldUnfreeze += EnableMovement;
    }

    private void OnDisable()
    {
        Interaction.OnInteractionStart -= DisableMovement;
        Interaction.OnInteractionEnd -= EnableMovement;

        WorldEvents.OnWorldFreeze -= DisableMovement;
        WorldEvents.OnWorldUnfreeze -= EnableMovement;
    }

    private void Update()
    {
        _inputReader.Poll();

        if (CanMove && _inputReader.InteractPressed)
            _interactionHandler.Interact();
    }

    private void FixedUpdate()
    {
        if (CanMove)
            _movement.Move(_inputReader.MoveInput);
    }

    private void DisableMovement()
    {
        CanMove = false;
        _inputReader.SetInputEnabled(false);
        _interactionHandler.HidePrompt();
    }

    private void EnableMovement()
    {
        CanMove = true;
        _inputReader.SetInputEnabled(true);
        _interactionHandler.ShowPrompt();
    }

    public void LockForCutscene()
    {
        DisableMovement();
    }

    public void UnlockFromCutscene()
    {
        EnableMovement();
    }

    private T GetOrAddComponent<T>() where T : Component
    {
        if (TryGetComponent(out T component))
            return component;

        return gameObject.AddComponent<T>();
    }
}
