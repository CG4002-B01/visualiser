using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    public float throwForce = 40f;
    public GameObject grenadePrefab; 
    public Camera playerPOV;
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
        Vector3 newPosition = playerPOV.transform.position + new Vector3(0, 10f, 0);
        GameObject grenade = Instantiate(grenadePrefab, newPosition, playerPOV.transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(playerPOV.transform.forward * throwForce, ForceMode.VelocityChange);
        // rb.AddForce(playerPOV.transform.up * throwForce, ForceMode.VelocityChange);
    }
}
