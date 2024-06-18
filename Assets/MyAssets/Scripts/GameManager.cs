using MobileConsole;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class GameManager : MonoSingleton<GameManager>
{
    public Tank selectedTank;
    
    public Transform plane;
    public Transform spawnPoint;

    public int tankCount;
    public int deadTankCount;
    public int activePlanesCount = 0;

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
        planeFinderBehaviour.OnInteractiveHitTest.AddListener(OnInteractiveHitBehavior());
        contentPositioningBehaviour.OnContentPlaced.AddListener(StartGame);

        //Check if the current build is development build
        if (Debug.isDebugBuild)
        {
            //Debug.Log("Development Build");
            LogConsole.Show();
        }
        else
        {
            LogConsole.Hide();
        }
    }

    private UnityAction<HitTestResult> OnInteractiveHitBehavior()
    {
        return hitTestResult =>
        {
            if (hitTestResult == null)
            {
                //Debug.LogError("Hit Test Result is Null!");
                return;
            }
            contentPositioningBehaviour.PositionContentAtPlaneAnchor(hitTestResult);
        };
    }

    public void ResetGame()
    {
        selectedTank = null;
        
        tankCount = 0;
        deadTankCount = 0;
        activePlanesCount = 0;
        
        Pool.instance.CloseAllTanks();
        Pool.instance.CloseAllPlanes();
        
        plane = null;
        spawnPoint = null;
        
        UIManager.instance.ClearList();
        
        isGameStarted = false;
        //Debug.Log("Game Reset");
    }

    private void StartGame(GameObject target)
    {
        UpdateSpawnPoint(target.transform);
        if (isGameStarted) return;
        
        ResetGame();
        
        isGameStarted = true;
        //Debug.Log("Game Started");
    }

    private void UpdateSpawnPoint(Transform targetTransform)
    {
        var pooledPlane = Pool.instance.GetPooledPlane();
        
        pooledPlane.transform.position = targetTransform.transform.position;
        pooledPlane.transform.rotation = targetTransform.transform.rotation;
        pooledPlane.SetActive(true);
        
        plane = pooledPlane.transform;
        spawnPoint = plane.GetChild(0);
        
        activePlanesCount++;
        
        //Debug.Log("Spawn Point Updated");
        //Debug.Log("Active Planes: " + activePlanesCount);
    }

    public void SpawnTank()
    {
        if (plane == null || spawnPoint == null)
        {
            //Debug.LogWarning("Plane or Spawn Point is Null!");
            return;
        }
        
        var tank = Pool.instance.GetPooledTank();
        if (tank == null)
        {
            //Debug.LogError("Tank Pool is Empty! Pool limit: " + Pool.instance.pooledTanks.Count);
            return;
        }
        
        tank.gameObject.SetActive(true);
        
        tank.transform.position = spawnPoint.position;
        tank.transform.rotation = spawnPoint.rotation;
        UIManager.instance.AddTankToList(tank);
        
        tankCount++;
        
        if (tankCount == 1)
        {
            SetSelectedTank(tank);
            //Debug.Log("Selected Tank Updated!");
        }
        
        //Debug.Log("Tank Spawned");
    }

    public void SetSelectedTank(Tank tank)
    {
        selectedTank = tank;
        UIManager.instance.uiIndicator.SetActive(true);
        UIManager.instance.uiIndicator.transform.SetParent(tank.transform);
        UIManager.instance.uiIndicator.transform.localPosition = Vector3.zero;
        UIManager.instance.UpdateDropdown();
    }
}
