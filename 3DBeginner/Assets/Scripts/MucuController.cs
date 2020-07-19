using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MucuController : MonoBehaviour
{/*
  * 减速陷阱，就很简单的将任务移速减半
  */
    public PlayerController player;
    
    void OnTriggerEnter(Collider other)
    {
        triggerEnterHelper(other);
    }

    void OnTriggerExit(Collider other)
    {
        triggerExitHelper(other);
    }

    void triggerEnterHelper(Collider other)
    {
        if(other.transform == player.transform)
        {
            player.moveSpeed *= 0.5f;
        }
    }

    void triggerExitHelper(Collider other)
    {
        if (other.transform == player.transform)
        {
            player.moveSpeed *= 2f;
        }
    }
}
