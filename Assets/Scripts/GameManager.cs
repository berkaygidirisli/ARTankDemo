using UnityEngine;
using UnityEngine.AddressableAssets;
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

    public void ResetGame()
    {
        tankCount = 0;
        deadTankCount = 0;
        Pool.instance.CloseAllTanks();
    }

    public override void Init()
    {
        base.Init();
        ResetGame();
        SpawnTank();
    }

    private void PlacePlayGround()
    {
        var plane = Instantiate(Resources.Load<GameObject>("Plane"), groundPlaneStageTransform, true);
        spawnPoint = plane.transform.GetChild(0);
        plane.transform.localPosition = new Vector3(0, 0, 0);
        plane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        plane.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SpawnTank()
    {
        var tank = Pool.instance.GetPooledTank();
        tank.transform.SetParent(plane.transform);
        tank.gameObject.SetActive(true);
        tank.transform.position = spawnPoint.position;
        tank.transform.rotation = spawnPoint.rotation;
        tankCount++;
        UIManager.instance.UpdateUI();

        if (tankCount == 1)
        {
            selectedTank = tank;
        }
    }
}
