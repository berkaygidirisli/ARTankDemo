using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class Pool : MonoSingleton<Pool>
{
    public AssetReference tankPrefab;
    public AssetReference projectilePrefab;
    
    AsyncOperationHandle<IList<IResourceLocation>> tankLocHandle;
    AsyncOperationHandle<IList<IResourceLocation>> projectileLocHandle;
    
    [SerializeField] public List<Tank> pooledTanks = new List<Tank>();
    private int amountToPoolTanks = 10;
    [SerializeField] public List<GameObject> pooledProjectiles = new List<GameObject>();
    private int amountToPoolProjectile = 50;

    private void Start()
    {
        tankLocHandle = Addressables.LoadResourceLocationsAsync(tankPrefab, typeof(GameObject));
        projectileLocHandle = Addressables.LoadResourceLocationsAsync(projectilePrefab, typeof(GameObject));
        
        tankLocHandle.Completed += LocHandleOnCompleted;
        projectileLocHandle.Completed += LocHandleOnCompleted;
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
        
        var go = Instantiate(asyncOperationHandle.Result);
        
        if (go.TryGetComponent(out Tank _))
        {
            for (int i = 0; i < amountToPoolTanks; i++)
            {
                var tankInstance = Instantiate(go);
                tankInstance.SetActive(false);
                pooledTanks.Add(tankInstance.GetComponent<Tank>());
            }
            
            Destroy(go);
        }
        else
        {
            for (int i = 0; i < amountToPoolProjectile; i++)
            {
                var projectileInstance = Instantiate(go);
                projectileInstance.SetActive(false);
                pooledProjectiles.Add(projectileInstance);
            }
            
            Destroy(go);
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
            tank.rb.angularVelocity = Vector3.zero;
            tank.rb.velocity = Vector3.zero;
            Release(tank.gameObject);
        }
    }
}
