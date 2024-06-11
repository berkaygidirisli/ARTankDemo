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
        
    }
}
