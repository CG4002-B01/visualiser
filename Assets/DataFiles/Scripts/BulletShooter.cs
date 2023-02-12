using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    // Bullet
    public GameObject bullet;
    // Bullet Force
    public float shootForce, upwardForce;
    // Gun Stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold; 
    int bulletsLeft, bulletsShots;
    // bools
    bool shooting, readyToShoot, reloading;
    // Reference
    public Camera fpsCam;
    public Transform attackPoint;
    // bug fixing :D
    public bool allowInvoke = true;

    // Start is called before the first frame update
    void Start()
    {
        shootForce = 300;
        upwardForce = 0;
        timeBetweenShooting = 0.1f;
        spread = 3;
        reloadTime = 1.5f;
        timeBetweenShots = 0;
        magazineSize = 6;
        bulletsPerTap = 1;
        allowButtonHold = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    void Awake()
    {
        // Make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    void MyInput() //This function would take in input from server
    {
        // Check if allowed to hold down button and take corresponding input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            // Set bullets shot to 0
            bulletsShots = 0;
            Shoot();
        }
    }

    void Shoot()
    {
        readyToShoot = false;

        // Find exact hit position using raycast
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); 
        RaycastHit hit;

        // Check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point; //Point where contact is made between bullet and object
        else 
            targetPoint = ray.GetPoint(75); //Some point far away from player

        // Calculate direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // Instantiate bullet
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        // Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithoutSpread.normalized;

        // Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithoutSpread.normalized * shootForce, ForceMode.Impulse);

        bulletsLeft--;
        bulletsShots++;

        // Invoke resetShot function (if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        bulletsLeft = magazineSize;

    }
}
