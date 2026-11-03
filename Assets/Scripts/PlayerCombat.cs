using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private InputActionReference shootInput;
    [SerializeField] private WeaponController weapon;

    private void OnEnable()
    {
        shootInput.action.Enable();
    }
    private void OnDisable()
    {
        shootInput.action.Disable();   
    }

    private void Update()
    {
        if (weapon != null && shootInput != null && shootInput.action.IsPressed()) 
        {
            weapon.Shoot();
        }
    }
}
