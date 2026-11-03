using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [SerializeField] InputActionReference moveInput;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float dashDistance = 2f;
    [SerializeField] float dashCooldown = 1.5f;
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
        }
    }
    private void OnDisable()
    {
        if (moveInput != null)
        {
            moveInput.action.Disable();
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
}
