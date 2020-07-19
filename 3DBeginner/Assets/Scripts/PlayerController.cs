using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Hp控制
    public int maxHp = 3;//最大Hp
    public GameEnding gameEnding;
    public Material[] materials;

    bool m_IsDead = false;
    int m_CurrentHp;

    //移动
    public float turnSpeed = 20f;//转身速度
    public float moveSpeed = 1f;//移动速度
    public TextController textController;//移动提示文字

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    //攻击
    public float shotInterval = 1f;//射击间隔
    public SightUIPrinter sightUIPrinter;//绘制准星

    bool m_ShotCd = false;//一秒只能开一枪
    bool m_IsShotting = false;//是否正在攻击
    bool m_GetWeapon = false;
    Transform m_Weapon = null;


    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
        textController.doPaint();
        m_CurrentHp = maxHp;
    }

    void FixedUpdate()
    {
        if(!m_IsDead)
            move();
    }

    void Update()
    {
        if (m_GetWeapon && !m_IsDead)
        {
            if (!m_ShotCd)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    shot();
                }

            }
            else
            {
                if (shotInterval > 0)
                {
                    shotInterval -= Time.deltaTime;
                }
                else
                {
                    m_ShotCd = false;
                    shotInterval = 1f;
                }
            }
        }
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude * moveSpeed);
        //如果已经有因为射击导致的转身将不再重复转身
        if (!m_IsShotting) m_Rigidbody.MoveRotation(m_Rotation);
    }
    
    //移动
    void move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        //如果人物正在攻击，播放攻击动画
        if (m_IsShotting)
        {
            m_Animator.Play("Fight_Attack");
            m_IsShotting = false;
        }

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    //拿起武器
    public void getWeapon()
    {
        m_GetWeapon = true;
        m_Weapon = GameObject.FindGameObjectWithTag("Weapon").transform;
        GetComponent<Animator>().SetBool("GetWeapon", true);//改变人物姿势
        sightUIPrinter.draw();//绘制准星
    }

    //射击
    void shot()
    {
        /*
        * 射击动作
        * 向鼠标点击位置发射射线
        * 将人物沿y轴转向射线点
        */
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        Vector3 pos;
        if (Physics.Raycast(ray, out raycastHit))
        {
            pos = raycastHit.point;
            transform.rotation = Quaternion.LookRotation(pos - transform.position);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
        m_IsShotting = true;

        //枪械射击
        m_ShotCd = true;
        m_Weapon.GetComponent<Shot>().shot();
    }

    //遭受攻击
    public void beAttack(int attackFrom, int damage = 1)
    {/*
      * 被攻击
      * 血量减去伤害，如果小于等于0 播放结束动画
      * 否则将头部的材质替换为不同颜色
      * attackFrom表示伤害来源 0为怪物 1为地刺
      */
        if (m_CurrentHp <= 0) return;
        GetComponents<AudioSource>()[1].Play();
        m_CurrentHp -= damage;

        if (m_CurrentHp <= 0)
        {
            dead();
        }
        else
        {
            //播放受击动画
            if (attackFrom == 0)
            {
                if (!GetComponent<Animator>().GetBool("GetWeapon"))
                {
                    GetComponent<Animator>().Play("Hit01");
                }
                else
                {
                    GetComponent<Animator>().Play("Fight_Hit01");
                }
            }
            else if (attackFrom == 1)
            {
                if (!GetComponent<Animator>().GetBool("GetWeapon"))
                {
                    GetComponent<Animator>().Play("Hit02");
                }
                else
                {
                    GetComponent<Animator>().Play("Fight_Hit02");
                }
            }

            SkinnedMeshRenderer skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            if (m_CurrentHp == 2)
            {
                skinnedMeshRenderer.materials[0].color = Color.yellow;
            }
            else if (m_CurrentHp == 1)
            {
                skinnedMeshRenderer.materials[0].color = Color.red;
            }
        }
    }

    //死亡
    void dead()
    {
        m_IsDead = true;
        //播放死亡动画
        if (!GetComponent<Animator>().GetBool("GetWeapon"))
        {
            GetComponent<Animator>().Play("Die");
        }
        else
        {
            GetComponent<Animator>().Play("Fight_Die");
        }
        gameEnding.CaughtPlayer();
    }
}
