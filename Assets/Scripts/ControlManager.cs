using UnityEngine;

public class ControlManager : MonoSingleton<ControlManager>
{
    private VariableJoystick movementJoystick;
    private VariableJoystick rotationJoystick;
    
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
        
        Vector3 direction =  Camera.main.transform.forward * rotationJoystick.Horizontal;
        tank.cannon.transform.Rotate(direction * tank.rotationSpeed * Time.fixedDeltaTime);
    }

    private void HandleMovement(Tank tank)
    {
        if (tank.gameObject == null) return;

        Vector3 movement = Camera.main.transform.right * movementJoystick.Horizontal +
                           Camera.main.transform.forward * movementJoystick.Vertical;
        movement.y = 0f;
        //Normalizes the vector3 to ensure that even if 2 inputs is pressed at the same time, the value still is either 1, 0 or -1, before multiplying with
        //deltaTime (framerate issue) and movementSpeed.
        movement.Normalize();
 
        //Takes the transform's current position and adds the vector 3 movement, to create the new position.
        tank.rb.velocity = new Vector3(movement.x * tank.movementSpeed, tank.rb.velocity.y, movement.z * tank.movementSpeed);
        
        //tank.rb.velocity = new Vector3( movementJoystick.Horizontal * tank.movementSpeed, tank.rb.velocity.y, movementJoystick.Vertical * tank.movementSpeed);

        if (movementJoystick.Horizontal != 0 || movementJoystick.Vertical != 0)
        {
            tank.transform.rotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
        }
    }
}
