using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public Light lighter;
    public EnemyController enemyController;
    public float lightOnInterval = 15;//灯光关闭时间（开灯间隔）
    public float lightOnTime = 10;//灯光打开时间（持续时间）
    public float reviveTime = 3;//怪物复活所需时间
    public TextController textController;//文字控制

    bool m_Statu = false;//灯光状态
    bool m_ReviveStatu = false;//怪物复活状态
    Animator m_Animator;
    AudioSource m_AudioSource;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void OnTriggerStay(Collider other)
    {
        doSwitch(other);
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "JohnLemon")
        {
            textController.doPaint();//绘制文字
            doSwitch(other);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform.name == "JohnLemon")
        {
            textController.doFade();//渐隐文字
        }
    }

    //操作开关
    void doSwitch(Collider other)
    {//按f开关灯
        if (Input.GetKeyDown(KeyCode.F))
        {
            m_AudioSource.Play();
            Animator playerAnimator = other.transform.GetComponent<Animator>();
            if (!m_Statu)
            {
                lightOn();
                if (playerAnimator.GetBool("GetWeapon"))
                {
                    playerAnimator.Play("Fight_SwitchOn");
                }
                else
                {
                    playerAnimator.Play("SwitchOn");
                }
            }
            else
            {
                lightOff();
                if (playerAnimator.GetBool("GetWeapon"))
                {
                    playerAnimator.Play("Fight_SwitchOff");
                }
                else
                {
                    playerAnimator.Play("SwitchOff");
                }
            }
        }   
    }

    //开灯
    void lightOn()
    {/*
      * 开灯
      * 将灯光状态设置为开
      * 播放开关动画
      * 将亮灯间隔重置为15s
      * 将怪物复活时间重置为3s
      * 将怪物复活状态改为false
      * 将灯光亮度设为100
      * 检测EnemiesController中的所有怪物，将范围内的怪物杀死
      */
        lighter.GetComponents<AudioSource>()[0].Play();

        m_Statu = true;
        m_Animator.SetBool("IsSwitchOn", m_Statu);

        lightOnInterval = 15;
        reviveTime = 3;
        m_ReviveStatu = false;

        lighter.intensity = 100;
        List<Transform> enemies = enemyController.getEnemies();
        for(int i = 0; i < enemies.Count; i++)
        {
            float dis = (enemies[i].position - lighter.transform.position).sqrMagnitude;
            if(dis <= lighter.range * lighter.range)
            {
                enemyController.beAttack(enemies[i].name);
            }
        }
    }
    
    //关灯
    void lightOff()
    {/*
      * 关灯
      * 将灯光状态设置为关
      * 播放开关动画
      * 将亮灯时间重置为10s
      * 将怪物复活状态设为true
      * 将复活时间设为3s
      * 将灯光亮度设为0
      */
        lighter.GetComponents<AudioSource>()[1].Play();

        m_Statu = false;
        m_Animator.SetBool("IsSwitchOn", m_Statu);

        lightOnTime = 10;
        m_ReviveStatu = true;
        reviveTime = 3;

        lighter.intensity = 0;
    }

    void Update()
    {
        //自动开关
        if (m_Statu)
        {
            if(lightOnTime > 0)
            {
                lightOnTime -= Time.deltaTime;
            }
            else
            {
                lightOff();
            }
        }
        else
        {
            if(lightOnInterval > 0)
            {
                lightOnInterval -= Time.deltaTime;
            }
            else
            {
                lightOn();
            }
        }

        //关灯3秒后复活怪物
        if (m_ReviveStatu)
        {
            if(reviveTime > 0)
            {
                reviveTime -= Time.deltaTime;
            }
            else
            {
                enemyController.reviveAll();
                m_ReviveStatu = false;
                reviveTime = 3;
            }
        }
    }
}
