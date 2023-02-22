using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayGun : MonoBehaviour
{
    public float shootRate;
    private float m_shootRateTimeStamp;

    public GameObject m_shotPrefab;
    public GameObject ammoLaunchPoint;

    RaycastHit hit;
    float range = 1000.0f;

    void Update()
    {

        // if (Input.GetMouseButton(0))
        // {
        //     if (Time.time > m_shootRateTimeStamp)
        //     {
        //         shootRay();
        //         m_shootRateTimeStamp = Time.time + shootRate;
        //     }
        // }

    }

    public void bulletAnimation()
    {
        shootRay();
        // if (Time.time > m_shootRateTimeStamp)
        // {
        //     shootRay();
        //     m_shootRateTimeStamp = Time.time + shootRate;
        // }
    }

    void shootRay()
    {
        Debug.Log("Shot animation called");
        // Ray ray = Camera.main.ScreenPointToRay(new Vector3(0, 0, 90));
        // if (Physics.Raycast(ray, out hit, range))
        // {
        //     GameObject laser = GameObject.Instantiate(m_shotPrefab, transform.position, transform.rotation) as GameObject;
        //     laser.GetComponent<ShotBehavior>().setTarget(hit.point);
        //     GameObject.Destroy(laser, 2f);
        // }

        GameObject laser = Instantiate(m_shotPrefab, ammoLaunchPoint.transform.position, ammoLaunchPoint.transform.rotation);
        laser.GetComponent<ShotBehavior>().setTarget(new Vector3(0, 0, 90));
        GameObject.Destroy(laser, 2f);
    }

}

