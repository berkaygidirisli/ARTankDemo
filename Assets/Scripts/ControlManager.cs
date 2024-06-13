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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (TouchPhase.Began != touch.phase) return;
        
            var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (!Physics.Raycast(ray, out var hit)) return;
        
            if (hit.collider.CompareTag("Tank"))
            {
                GameManager.instance.selectedTank = hit.collider.GetComponent<Tank>();
            }
        }
    }
    

    private void HandleCannonRotation(Tank tank)
    {
        if (tank.gameObject == null) return;
        
        
        tank.cannon.transform.Rotate(Vector3.forward, rotationJoystick.Horizontal * tank.rotationSpeed);
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
