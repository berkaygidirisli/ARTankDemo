using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    public TextMeshProUGUI destroyedTankCountText;
    public TextMeshProUGUI tankCountText;
    public TextMeshProUGUI activePlanesText;
    public TextMeshProUGUI fpsText;
    
    public Button fireButton;
    public Button resetButton;
    public Button placePlaneButton;
    public Button createTankButton;
    
    public VariableJoystick movementJoystick;
    public VariableJoystick rotationJoystick;

    private void Start()
    {
        fireButton.onClick.AddListener(() =>
        {
            GameManager.instance.selectedTank.Fire();
        });
        
        resetButton.onClick.AddListener(() =>
        {
            GameManager.instance.ResetGame();
        });
        
        placePlaneButton.onClick.AddListener(() =>
        {
            GameManager.instance.planeFinderBehaviour.PerformHitTest(Vector2.zero);
        });
        
        createTankButton.onClick.AddListener(() =>
        {
            GameManager.instance.SpawnTank();
        });
    }

    public void UpdateUI()
    {
        tankCountText.text = "Tank Count: " + GameManager.instance.tankCount;
        destroyedTankCountText.text = "Destroyed Tank Count: " + GameManager.instance.deadTankCount;
        activePlanesText.text = "Active Planes: " + GameManager.instance.activePlanesCount;
        
        Debug.Log("UI Updated!");
    }

    private void Update()
    {
        fpsText.text = "FPS: " + (1 / Time.deltaTime).ToString("F0");
    }
}
