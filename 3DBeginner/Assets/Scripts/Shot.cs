using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject bullet;
    public Transform muzzle;
    public PlayerController player;
    public CircularProgress circularProgress;

    public void shot()//产生一个子弹，把它放在枪口位置并给它一个向前的力
    {
        GameObject bulletclone = Instantiate(bullet, muzzle.position + player.transform.forward/2 + Vector3.up/2, muzzle.rotation);//将预制体生成对象

        Rigidbody rigidbody = bulletclone.GetComponent<Rigidbody>();
        rigidbody.AddForce(player.transform.forward * 5,ForceMode.VelocityChange);
        GetComponent<AudioSource>().Play();

        circularProgress.fill();
    }

}
