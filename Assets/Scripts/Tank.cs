using System.Collections;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Transform cannon;
    public Transform cannonPoint;
    public Rigidbody rb;
    
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
            DisableObject(gameObject);
            DisableObject(other.gameObject);
            cannon.rotation = Quaternion.Euler(-90f,0f,0f);
            GameManager.instance.deadTankCount++;
            GameManager.instance.tankCount--;
            UIManager.instance.UpdateUI();
        }
    }
}
