using UnityEngine;


public class ControlManager : MonoSingleton<ControlManager>
{
    private VariableJoystick movementJoystick;
    public float movementSpeed = 10f;
    
    private VariableJoystick rotationJoystick;
    public float rotationSpeed = 5f;
    
    void Start()
    {
        movementJoystick = UIManager.instance.movementJoystick;
        rotationJoystick = UIManager.instance.rotationJoystick;
    }
    private void FixedUpdate()
    {
        if(GameManager.instance.selectedTank == null) return;
        
        HandleMovement(GameManager.instance.selectedTank);
        HandleCannonRotation(GameManager.instance.selectedTank);
        SelectTankUsingRaycast();
    }
    
    void SelectTankUsingRaycast()
    {
        if (TouchPhase.Began == Input.GetTouch(0).phase)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tank"))
                {
                    GameManager.instance.selectedTank = hit.collider.GetComponent<Tank>();
                }
            }
        }
    }
    

    private void HandleCannonRotation(Tank tank)
    {
        if (tank.gameObject == null) return;
        
        Vector3 direction =  Vector3.forward * rotationJoystick.Horizontal;
        tank.cannon.transform.Rotate(direction * rotationSpeed * Time.fixedDeltaTime);
    }

    private void HandleMovement(Tank tank)
    {
        if (tank.gameObject == null) return;
        
        tank.rb.velocity = new Vector3(movementJoystick.Horizontal * movementSpeed, tank.rb.velocity.y, movementJoystick.Vertical * movementSpeed);

        if (movementJoystick.Horizontal != 0 || movementJoystick.Vertical != 0)
        {
            tank.transform.rotation = Quaternion.LookRotation(new Vector3(movementJoystick.Horizontal, 0, movementJoystick.Vertical));
        }
    }
}
