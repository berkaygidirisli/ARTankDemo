using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Transform cannon;
    public Transform cannonPoint;
    public Rigidbody rb;
    
    public void Fire()
    {
        //var go = Pool.instance.projectilePool.Get();
        var go = Pool.instance.GetPooledProjectile();
        go.transform.position = cannonPoint.position;
        go.transform.rotation = cannonPoint.rotation;
        go.gameObject.SetActive(true);
        go.GetComponent<Rigidbody>().AddForce(-cannonPoint.up * 1000f);
        StartCoroutine(DisableProjectile(go));
    }

    private IEnumerator DisableProjectile(GameObject go)
    {
        yield return new WaitForSeconds(5f);
        
        go.GetComponent<Rigidbody>().velocity = Vector3.zero;
        go.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Pool.instance.Release(gameObject);
            Pool.instance.Release(other.gameObject);
            GameManager.instance.deadTankCount++;
            GameManager.instance.tankCount--;
        }
    }
}
