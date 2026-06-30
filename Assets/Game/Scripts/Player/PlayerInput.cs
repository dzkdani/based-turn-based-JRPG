using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 MoveInput { get; private set; }
    public bool InteractPressed { get; private set; }

    private bool _inputEnabled = true;

    public void Poll()
    {
        InteractPressed = false;

        if (!_inputEnabled)
        {
            MoveInput = Vector3.zero;
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        MoveInput = new Vector3(horizontal, 0, vertical).normalized;
        InteractPressed = Input.GetKeyDown(KeyCode.Space);
    }

    public void SetInputEnabled(bool enabled)
    {
        _inputEnabled = enabled;

        if (!enabled)
        {
            MoveInput = Vector3.zero;
            InteractPressed = false;
        }
    }
}
