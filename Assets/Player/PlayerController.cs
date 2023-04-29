using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Player player;
    
    private Camera _camera;
    private PlayerInputActions _actions;
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _fireAction;

    private void Awake()
    {
        _camera = Camera.main;
        _actions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _moveAction = _actions.Player.Move;
        _lookAction = _actions.Player.Look;
        _moveAction.Enable();
        _lookAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _lookAction.Disable();
    }

    private void Update()
    {
        player.SetMoveDirection(_moveAction.ReadValue<Vector2>());
        player.SetLookDirection(GetWorldMousePosition());
    }

    private Vector2 GetWorldMousePosition()
    {
        var mousePosition = _lookAction.ReadValue<Vector2>();
        return _camera.ScreenToWorldPoint(mousePosition).ConvertTo<Vector2>();
    }
}
