using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrenadeThrower : MonoBehaviour
{
    public float throwForce = 40f;
    public GameObject grenadePrefab; 
    public GameObject enemyGrenadeLaunchPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, enemyGrenadeLaunchPoint.transform.position, enemyGrenadeLaunchPoint.transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(enemyGrenadeLaunchPoint.transform.forward * throwForce, ForceMode.VelocityChange);
        // rb.AddForce(playerPOV.transform.up * throwForce, ForceMode.VelocityChange);
    }
}
