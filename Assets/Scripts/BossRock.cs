using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : Weapon
{
    Rigidbody rb;
    float angularPower = 2;
    float scaleValue = 1.1f;
    bool isShoot;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();  
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }
    IEnumerator GainPowerTimer()
    {
        yield return new WaitForSeconds(2.2f);
        isShoot = true;
    }
    // 데굴데굴 굴러감
    IEnumerator GainPower()
    {
        while(!isShoot)
        {
            angularPower += 0.02f;
            scaleValue += 0.005f;
            transform.localScale = Vector3.one * scaleValue;
            rb.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }
    }
}
