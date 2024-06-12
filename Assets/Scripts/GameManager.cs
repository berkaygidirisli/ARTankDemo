using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Vuforia;

public class GameManager : MonoSingleton<GameManager>
{
    public Tank selectedTank;
    public GameObject tankPrefab;
    public Transform plane;
    public Transform spawnPoint;
    public Transform groundPlaneStageTransform;

    public int tankCount;
    public int deadTankCount;

    public AnchorInputListenerBehaviour anchorInputListenerBehaviour;
    public PlaneFinderBehaviour planeFinderBehaviour;
    public ContentPositioningBehaviour contentPositioningBehaviour;
    
    private bool isGameStarted;
    private void Awake()
    {
        isGameStarted = false;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        planeFinderBehaviour.OnInteractiveHitTest.AddListener(arg0 => contentPositioningBehaviour.PositionContentAtPlaneAnchor(arg0));
        contentPositioningBehaviour.OnContentPlaced.AddListener(arg0 => StartGame());
    }

    public void ResetGame()
    {
        tankCount = 0;
        deadTankCount = 0;
        Pool.instance.CloseAllTanks();
        
        UIManager.instance.UpdateUI();
        
        isGameStarted = false;
    }

    public override void Init()
    {
        base.Init();
    }

    private void StartGame()
    {
        if (isGameStarted) return;
        
        ResetGame();
        UpdateSpawnPoint();
        SpawnTank();
        isGameStarted = true;
    }

    private void UpdateSpawnPoint()
    {
        plane = groundPlaneStageTransform.GetChild(0);
        spawnPoint = plane.transform.GetChild(0);
    }

    private void PlacePlayGround()
    {
        plane = groundPlaneStageTransform.GetChild(0);
        spawnPoint = plane.transform.GetChild(0);
        plane.transform.localPosition = new Vector3(0, 0, 0);
        plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        plane.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SpawnTank()
    {
        var tank = Pool.instance.GetPooledTank();
        
        tank.gameObject.SetActive(true);
        tank.transform.position = plane.position;
        tank.transform.rotation = plane.rotation;
        tankCount++;
        UIManager.instance.UpdateUI();

        if (tankCount == 1)
        {
            selectedTank = tank;
        }
    }
}
