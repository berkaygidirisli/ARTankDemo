using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoSingleton<GameManager>
{
    public Tank selectedTank;
    public GameObject tankPrefab;
    public Transform spawnPoint;

    public int tankCount;
    public int deadTankCount;
    
    void ResetGame()
    {
        tankCount = 0;
        deadTankCount = 0;
        Pool.instance.CloseAllTanks();
    }
    
    void Start()
    {
        ResetGame();
        SpawnTank();
    }

    private void SpawnTank()
    {
        var tank = Pool.instance.GetPooledTank();
        tank.gameObject.SetActive(true);
        tank.transform.position = spawnPoint.position;
        tank.transform.rotation = spawnPoint.rotation;
        tankCount++;

        if (tankCount == 1)
        {
            selectedTank = tank;
        }
    }
}
