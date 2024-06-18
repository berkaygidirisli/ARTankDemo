using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class Pool : MonoSingleton<Pool>
{
    public AssetReference tankPrefab;
    public AssetReference projectilePrefab;
    public AssetReference planePrefab;
    
    AsyncOperationHandle<IList<IResourceLocation>> tankLocHandle;
    AsyncOperationHandle<IList<IResourceLocation>> projectileLocHandle;
    AsyncOperationHandle<IList<IResourceLocation>> planeLocHandle;
    
    [SerializeField] public List<GameObject> pooledTanks = new List<GameObject>();
    [SerializeField] public List<Tank> activeTanks = new List<Tank>();
    public int amountToPoolTanks = 10;
    [SerializeField] public List<GameObject> pooledProjectiles = new List<GameObject>();
    public int amountToPoolProjectile = 50;
    [SerializeField] public List<GameObject> pooledPlanes = new List<GameObject>();
    public int amountToPoolPlane = 10;

    private void Start()
    {
        tankLocHandle = Addressables.LoadResourceLocationsAsync(tankPrefab, typeof(GameObject));
        projectileLocHandle = Addressables.LoadResourceLocationsAsync(projectilePrefab, typeof(GameObject));
        planeLocHandle = Addressables.LoadResourceLocationsAsync(planePrefab, typeof(GameObject));
        
        tankLocHandle.Completed += LocHandleOnCompleted;
        projectileLocHandle.Completed += LocHandleOnCompleted;
        planeLocHandle.Completed += LocHandleOnCompleted;
    }

    private void LocHandleOnCompleted(AsyncOperationHandle<IList<IResourceLocation>> obj)
    {
        if (obj.Status != AsyncOperationStatus.Succeeded) return;
        
        foreach (var location in obj.Result)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>(location);
            asyncOperationHandle.Completed += AsyncOperationHandleCompleted;
        }
    }

    void AsyncOperationHandleCompleted(AsyncOperationHandle<GameObject> asyncOperationHandle)
    {
        if (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded) return;
        
        var go = asyncOperationHandle.Result;
        
        if (go.CompareTag("Tank"))
        {
            AddResultToPool(go, pooledTanks, amountToPoolTanks);
        }
        
        if (go.CompareTag("Projectile"))
        {
            AddResultToPool(go, pooledProjectiles, amountToPoolProjectile);
        }
        
        if (go.CompareTag("Plane"))
        {
            AddResultToPool(go, pooledPlanes, amountToPoolPlane);
        }
    }

    private void AddResultToPool(GameObject go, List<GameObject> targetList, int amountToPool)
    {
        for (var i = 0; i < amountToPool; i++)
        {
            var item = Instantiate(go);

            if (targetList == pooledTanks)
            {
                item.name = "Tank " + (i + 1);
            }
            
            item.SetActive(false);
            targetList.Add(item);
        }
    }

    public GameObject GetPooledProjectile()
    {
        foreach (var projectile in pooledProjectiles)
        {
            if (!projectile.activeInHierarchy)
            {
                return projectile;
            }
        }

        return null;
    }
    
    public GameObject GetPooledPlane()
    {
        foreach (var plane in pooledPlanes)
        {
            if (!plane.activeInHierarchy)
            {
                return plane;
            }
        }

        return null;
    }
    
    public void Release(GameObject go)
    {
        go.SetActive(false);
        Debug.Log(go.name + " released!");
    }
    
    public Tank GetPooledTank()
    {
        foreach (var tank in pooledTanks)
        {
            if (!tank.gameObject.activeInHierarchy)
            {
                return tank.GetComponent<Tank>();
            }
        }

        return null;
    }

    public void CloseAllTanks()
    {
        foreach (var tankGo in pooledTanks)
        {
            var tank = tankGo.GetComponent<Tank>();
            
            tank.rb.angularVelocity = Vector3.zero;
            tank.rb.velocity = Vector3.zero;
            tank.cannon.transform.rotation = Quaternion.Euler(-90f,0f,0f);
            Release(tank.gameObject);
            
            Debug.Log("All tanks closed!");
        }
        
        UIManager.instance.ClearList();
        activeTanks.Clear();
        
        UIManager.instance.uiIndicator.transform.SetParent(null);
        UIManager.instance.uiIndicator.SetActive(false);
        GameManager.instance.selectedTank = null;
    }

    public void CloseAllPlanes()
    {
        foreach (var plane in pooledPlanes)
        {
            Release(plane);
            
            Debug.Log("All planes closed!");
        }
    }
}
