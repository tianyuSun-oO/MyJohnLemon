using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DICIController : MonoBehaviour
{
    public Transform player;
    public AudioClip clip;

    bool play;

    void OnTriggerEnter(Collider other)
    {
        if(other.transform == player)
            attack();
    }
    void attack()
    {
        player.GetComponent<PlayerController>().beAttack(1);
    }
    void Update()
    {
        //控制地刺弹出时播放声音
        if (!play)
        {
            if (GetComponent<BoxCollider>().transform.position.y > 0)
            {
                AudioSource.PlayClipAtPoint(clip, transform.position);
                play = true;
            }
        }
        else
        {
            if (GetComponent<BoxCollider>().transform.position.y < 0)
            {
                play = false;
            }
        }
        
    }

}
