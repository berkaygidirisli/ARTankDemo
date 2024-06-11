using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlManager : MonoSingleton<ControlManager>
{
    public VariableJoystick movementJoystick;
    public float movementSpeed = 10f;
    
    public VariableJoystick rotationJoystick;
    public float rotationSpeed = 5f;
    
    public Button fireButton;
    
    void Start()
    {
        fireButton.onClick.AddListener(Fire);
    }
    
    private void FixedUpdate()
    {
        if(GameManager.instance.selectedTank == null) return;
        
        HandleMovement(GameManager.instance.selectedTank);
        HandleCannonRotation(GameManager.instance.selectedTank);
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

    private void Fire()
    {
        if (GameManager.instance.gameObject == null) return;
        
        GameManager.instance.selectedTank.Fire();
    }
}
