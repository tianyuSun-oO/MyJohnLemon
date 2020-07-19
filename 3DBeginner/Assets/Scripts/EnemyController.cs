using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<Transform> enemies;//所有怪物
    public float reviveTime = 3;//怪物复活时间
    public AudioClip[] audios;//死亡音效
    public int maxHp = 1;//最大生命值
   
    List<Transform> m_DeadEnemies = null;//死亡怪物
    int[] m_EnemiesHp;//当前Hp

    void Awake()
    {
        //初始化enemies列表
        foreach (Transform child in this.transform)
        {
            enemies.Add(child);
        }
        enemies.RemoveAt(0);

        //初始化生命值
        m_EnemiesHp = new int[enemies.Count];
        for(int i = 0; i< m_EnemiesHp.Length; i++)
        {
            m_EnemiesHp[i] = maxHp;
        }

        //初始化deadEnemies
        m_DeadEnemies = new List<Transform>();
    }
    
    //遭受攻击
    public void beAttack(string name, int damage = 1)
    {
        Transform enemy = getObjectByName(name);
        int index = enemies.IndexOf(enemy.transform);

        m_EnemiesHp[index] -= damage;

        if (m_EnemiesHp[index] <= 0)
        {
            enemyDead(index);
        }
    }
    
    //死亡
    void enemyDead(int index)
    {
        Transform deadEnemy = enemies[index];
        if (!m_DeadEnemies.Contains(deadEnemy))//如果怪物已经死亡，就什么也不做
        {
            //在死亡怪物位置播放死亡音效（石像鬼和幽灵的音效不同）
            Vector3 position = deadEnemy.transform.position;

            if (deadEnemy.name.Substring(0, 5) == "Ghost")
            {
                AudioSource.PlayClipAtPoint(audios[0], position);
            }
            else
            {
                AudioSource.PlayClipAtPoint(audios[1], position);
            }

            m_DeadEnemies.Add(deadEnemy);//将死亡的怪物放在死亡列表中
            deadEnemy.gameObject.SetActive(false);//将死亡的怪物设为不活跃状态
        }
    }

    //复活
    public void reviveAll()
    {
        foreach (Transform deadEnemy in m_DeadEnemies)//把死亡怪物列表中的怪物全部复活
        {//即设为活跃状态
            if (deadEnemy.gameObject.activeSelf == false)
            {
                if(deadEnemy.GetComponent<WaypointPatrol>() != null)//如果怪物可以移动，则让它在路线起点复活
                {
                    deadEnemy.transform.position = deadEnemy.GetComponent<WaypointPatrol>().waypoints[0].position;
                }
                deadEnemy.gameObject.SetActive(true);
                deadEnemy.GetComponentInChildren<Observer>().canAttack(false);
                deadEnemy.GetComponents<AudioSource>()[1].Play();
            }
        }
        m_DeadEnemies.Clear();
    }

    //获取所有怪物
    public List<Transform> getEnemies()
    {
        return enemies;
    }

    //根据名字找到怪物
    Transform getObjectByName(string name)
    {
        foreach(Transform transform in enemies)
        {
            Transform get = transform;

            if(get.name == name)
            {
                return get;
            }
        }
        return null;
    }

}
