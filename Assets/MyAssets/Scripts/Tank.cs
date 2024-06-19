using System;
using System.Collections;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Transform cannon;
    public Transform cannonPoint;
    public Rigidbody rb;
    public ParticleSystem particleSystem;
    [SerializeField] private LineRenderer laserLine;

    public float cannonForce = 1f;
    public float movementSpeed = 0.1f;
    public float rotationSpeed = 2f;
    
    public void Fire()
    {
        //var go = Pool.instance.projectilePool.Get();
        var go = Pool.instance.GetPooledProjectile();
        go.transform.position = cannonPoint.position;
        go.transform.rotation = cannonPoint.rotation;
        go.gameObject.SetActive(true);
        go.GetComponent<Rigidbody>().AddForce(-cannonPoint.up * cannonForce);
        particleSystem.Play();
        StartCoroutine(DisableProjectile(go));
    }

    private IEnumerator DisableProjectile(GameObject go)
    {
        yield return new WaitForSeconds(5f);
        DisableObject(go);
    }

    private static void DisableObject(GameObject go)
    {
        go.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        go.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        Pool.instance.Release(go);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            Tank t = gameObject.GetComponent<Tank>();
            
            GameManager.instance.deadTankCount++;
            GameManager.instance.tankCount--;
            
            UIManager.instance.RemoveTankFromList(t);
            
            DisableObject(gameObject);
            DisableObject(other.gameObject);
        }
    }

    private void OnEnable()
    {
        cannon.localRotation = Quaternion.Euler(-90f,0f,0f);
        laserLine.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        laserLine.gameObject.SetActive(this == GameManager.instance.selectedTank);
    }
}
