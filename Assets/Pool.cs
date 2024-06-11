using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pool : MonoSingleton<Pool>
{
    [SerializeField] public IObjectPool<Tank> tankPool;
    private int amountToPoolTank = 10;
    [SerializeField] public IObjectPool<GameObject> projectilePool;
    private int amountToPoolProjectile = 50;
    
    
}
