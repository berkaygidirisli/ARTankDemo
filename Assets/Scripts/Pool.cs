using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoSingleton<Pool>
{
    [SerializeField] public List<Tank> pooledTanks = new List<Tank>();
    private int amountToPoolTanks = 10;
    [SerializeField] public List<GameObject> pooledProjectiles = new List<GameObject>();
    private int amountToPoolProjectile = 50;

    private void Awake()
    {
        for (int i = 0; i < amountToPoolProjectile; i++)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Projectile"));
            go.SetActive(false);
            pooledProjectiles.Add(go);
        }

        for (int i = 0; i < amountToPoolTanks; i++)
        {
            var go = Instantiate(Resources.Load<GameObject>("Tank"));
            go.SetActive(false);
            pooledTanks.Add(go.gameObject.GetComponent<Tank>());
        }
    }
    
    public GameObject GetPooledProjectile()
    {
        for (int i = 0; i < pooledProjectiles.Count; i++)
        {
            if (!pooledProjectiles[i].activeInHierarchy)
            {
                return pooledProjectiles[i];
            }
        }
        return null;
    }
    
    public void Release(GameObject go)
    {
        go.SetActive(false);
    }
    
    public Tank GetPooledTank()
    {
        for (int i = 0; i < pooledTanks.Count; i++)
        {
            if (!pooledTanks[i].gameObject.activeInHierarchy)
            {
                return pooledTanks[i];
            }
        }
        return null;
    }

    public void CloseAllTanks()
    {
        foreach (var tank in pooledTanks)
        {
            tank.gameObject.SetActive(false);
        }
    }
}
