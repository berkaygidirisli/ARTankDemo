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
        if (tankList.value > tankList.options.FindIndex(x => x.text == t.name))
        {
            Pool.instance.activeTanks.Remove(t);
            UpdateDropdown();
            tankList.value -= 1;
        }
        else
        {
            Pool.instance.activeTanks.Remove(t);
            UpdateDropdown();
        }
        
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

    private void UpdateDropdown()
    {
        tankList.options.Clear();
        
        foreach (var tank in Pool.instance.activeTanks)
        {
            tankList.options.Add(new TMP_Dropdown.OptionData(tank.name));
        }
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
