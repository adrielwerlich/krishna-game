using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private InputActions inputManager;

    public static event Action FireArrow;
    public static event Action FireBomb;
    public static event Action FireLightning;
    public static event Action ShowRotationScrollBar;
    public static event Action<bool> ToggleShowGameMenu;
    public static event Action GoToMainMenu;
    public static event Action ReloadLevel;

    private bool gameMenuOpen = false;
    private void Awake()
    {
        inputManager = new InputActions();
        inputManager.Enable();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        inputManager.Disable();
    }

    private void Start()
    {
        inputManager.FireWeapon.FireArrow.performed += ctx => ArrowFired(ctx);
        inputManager.FireWeapon.FireBomb.performed += ctx => BombFired(ctx);
        inputManager.FireWeapon.FireLightning.performed += ctx => LightningFired(ctx);
        inputManager.Configs.RotationSpeed.performed += ctx => SetRotation(ctx);
        inputManager.Configs.OpenGameMenu.performed += ctx => OpenGameMenu(ctx);
        inputManager.Configs.GoToMainMenu.performed += ctx => FunctionGoToMainMenu(ctx);
        inputManager.Configs.ReloadLevel.performed += ctx => FunctionReloadLevel(ctx);
    }

    private void FunctionReloadLevel(InputAction.CallbackContext ctx)
    {
        ReloadLevel.Invoke();
    }
    private void FunctionGoToMainMenu(InputAction.CallbackContext ctx)
    {
        GoToMainMenu.Invoke();
    }

    private void OpenGameMenu(InputAction.CallbackContext ctx)
    {
        gameMenuOpen = !gameMenuOpen;
        ToggleShowGameMenu.Invoke(gameMenuOpen);
    }
    private void SetRotation(InputAction.CallbackContext ctx)
    {
        ShowRotationScrollBar.Invoke();
    }

    private void ArrowFired(InputAction.CallbackContext ctx)
    {
        FireArrow.Invoke();
    }

    private void BombFired(InputAction.CallbackContext ctx)
    {
        FireBomb.Invoke();
    }

    private void LightningFired(InputAction.CallbackContext ctx)
    {
        FireLightning.Invoke();
    }
    public Vector2 MovementVectorNormalized
    {
        get
        {
            if (inputManager != null)
            {
                return inputManager.Movement.Walk.ReadValue<Vector2>().normalized;
            }
            return Vector2.zero;
        }
    }
}
