using JetBrains.Annotations;
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
        _fireAction = _actions.Player.Fire;
        _moveAction.Enable();
        _lookAction.Enable();
        _fireAction.Enable();
        _fireAction.performed += Fire;
    }

    private void OnDisable()
    {
        _moveAction.Disable();
        _lookAction.Disable();
        _fireAction.Disable();
    }

    private void Update()
    {
        player.SetMoveDirection(_moveAction.ReadValue<Vector2>());
        player.SetAimPosition(GetWorldMousePosition());
    }

    private Vector3 GetWorldMousePosition()
    {
        var mousePosition = _lookAction.ReadValue<Vector2>();
        return _camera.ScreenToWorldPoint(mousePosition);
    }

    private void Fire(InputAction.CallbackContext context) => player.Fire();
    
}
