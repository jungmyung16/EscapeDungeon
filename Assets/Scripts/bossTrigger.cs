using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossTrigger : MonoBehaviour
{
    public GameManager Manager;
    public Transform destination;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("BossStart");
            other.transform.position = destination.position;
            Manager.BossStage();
        }
    }
}
