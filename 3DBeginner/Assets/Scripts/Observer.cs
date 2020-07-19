using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;

    bool m_IsPlayerInRange;
    bool m_CanAttack = true;//表示怪物能否攻击的状态的变量
    float m_CantAttackTime = 3;//刚复活3秒的怪物不能攻击
    float m_AttackInterval = 5;//怪物有5秒的攻击间隔
    bool m_AttackCd = false;
    int m_AttackFrom = 0;

    void Start()
    {
        if(transform.name == "Bone DiCi")
        {
            m_AttackFrom = 1;
        }
    }

    // m_CanAttack的setter函数,设置能否攻击（刚复活的怪物不能攻击）
    public void canAttack(bool statu)
    {
        m_CanAttack = statu;
    }

    void OnTriggerStay(Collider other)
    {
        triggerEnterHelper(other);
    }//玩家停留在怪物视野内要不停进行检测

    void OnTriggerEnter(Collider other)
    {
        triggerEnterHelper(other);
    }

    void OnTriggerExit(Collider other)
    {
        triggerExitHelper(other);
    }

    //攻击
    void attack()//怪物攻击操作，创建指向玩家的射线，判断是否有阻隔（教程自带部分）
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    player.GetComponent<PlayerController>().beAttack(m_AttackFrom);
                    transform.parent.GetComponents<AudioSource>()[0].Play();

                    m_AttackCd = true;//怪物攻击后进入攻击cd状态
                }
            }
        }
    }

    void Update()
    {
        if (m_CanAttack)//如果不能攻击相当于玩家离开视野
        {//（如果不进行这一步会导致玩家在怪物视野内死亡后怪物复活一直攻击玩家）
            if (m_AttackCd)//攻击cd
            {
                if (m_AttackInterval > 0)
                {
                    m_AttackInterval -= Time.deltaTime;
                }
                else
                {
                    m_AttackCd = false;
                    m_AttackInterval = 5;
                }
            }
            else
            {
                attack();
            }
        }
        else
        {
            triggerExitHelper(player.GetComponent<Collider>());
            if(m_CantAttackTime > 0) 
                m_CantAttackTime -= Time.deltaTime;
            else
            {
                canAttack(true);
                m_CantAttackTime = 3;
            }
        }
    }

    void triggerEnterHelper(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }
    void triggerExitHelper(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }
}