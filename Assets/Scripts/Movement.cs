using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference dashInput;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float dashDistance = 2f;
    [SerializeField] float dashDuration = 0.15f;
    [SerializeField] float dashCooldown = 1.5f;

    private bool isDashing;
    private float nextDashTime;

    private CharacterController controller;
    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        if (moveInput != null) 
        { 
            moveInput.action.Enable();
            dashInput.action.Enable();
        }
    }
    private void OnDisable()
    {
        if (moveInput != null)
        {
            moveInput.action.Disable();
            dashInput.action.Disable();
        }
    }
    // Update is called once per frame
    void Update()
    {
        Moving();
        LookAtMouse();
    }

    public void Moving()
    {
        Vector2 input = moveInput.action.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x,0,input.y).normalized;
        controller.Move(direction * moveSpeed * Time.deltaTime);
        if (dashInput != null && dashInput.action.WasPerformedThisFrame() && !isDashing && Time.time >= nextDashTime && direction.sqrMagnitude > 0.001f) 
        {
            StartCoroutine(Dash(direction));
        }
    }

    public void LookAtMouse()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if(groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);

            Vector3 direction = targetPoint - transform.position;

            if (direction.sqrMagnitude > 0.001f) 
            { 
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

    }

    IEnumerator Dash(Vector3 direction)
    {
        isDashing = true;
        nextDashTime = Time.time + dashCooldown;

        float elapsedTime = 0f;

        float dashSpeed = dashDistance / dashDuration;

        while (elapsedTime < dashDuration)
        {
            controller.Move(direction * dashSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        isDashing = false;
    }
}
