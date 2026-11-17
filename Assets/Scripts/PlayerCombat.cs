using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private InputActionReference shootInput;
    [SerializeField] private WeaponController weapon;
    [SerializeField] private ChargeAbility chargeAbility;
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
        if(shootInput == null)
        {
            return;
        }
        if(weapon != null && shootInput.action.IsPressed())
        {
            weapon.Shoot();
        }
        if (chargeAbility != null && shootInput.action.WasPressedThisFrame()) 
        { 
            chargeAbility.StartCharge();
        }
    }
}
