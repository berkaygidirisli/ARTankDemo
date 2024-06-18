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

    public GameObject uiIndicator;
    public TMP_Dropdown tankList;
    
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

        tankList.onValueChanged.AddListener(delegate
        {
            GameManager.instance.SetSelectedTank(Pool.instance.activeTanks[tankList.value]);
            UpdateDropdown();
        });
        
        uiIndicator.SetActive(false);
    }
    
    public void AddTankToList(Tank t)
    {
        Pool.instance.activeTanks.Add(t);
        UpdateDropdown();
    }
    
    public void RemoveTankFromList(Tank t)
    {
        Pool.instance.activeTanks.Remove(t);
        UpdateDropdown();
    }

    private void UpdateUI()
    {
        if (GameManager.instance.selectedTank == null)
        {
            uiIndicator.SetActive(false);
        }
        
        tankCountText.text = "Tank Count: " + GameManager.instance.tankCount;
        destroyedTankCountText.text = "Destroyed Tank Count: " + GameManager.instance.deadTankCount;
        activePlanesText.text = "Active Planes: " + GameManager.instance.activePlanesCount;
    }

    public void UpdateDropdown()
    {
        tankList.options.Clear();
        
        foreach (var tank in Pool.instance.activeTanks)
        {
            tankList.options.Add(new TMP_Dropdown.OptionData(tank.name));
        }
        
        tankList.value = Pool.instance.activeTanks.IndexOf(GameManager.instance.selectedTank);
    }

    private void Update()
    {
        fpsText.text = "FPS: " + (1 / Time.deltaTime).ToString("F0");
    }

    private void FixedUpdate()
    {
        UpdateUI();
    }

    public void ClearList()
    {
        tankList.options.Clear();
        Pool.instance.activeTanks.Clear();
    }
}
