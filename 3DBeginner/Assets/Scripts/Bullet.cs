using UnityEngine;

public class Bullet : MonoBehaviour
{
    public SightUIPrinter sightUIPrinter;
    public EnemyController enemyController;

    private void OnTriggerEnter(Collider other)
    {
        /*子弹碰到物体就会销毁自身，如果碰到的是怪物就会攻击怪物*/
        if (other.name != "PointOfView")//如果子弹撞到怪物视线应该直接穿过
        {
            if (other.transform.tag == "Enemy")
            {
                Transform enemy = other.transform;
                enemyController.beAttack(other.transform.name);
                sightUIPrinter.hit();
            }
            Destroy(gameObject);
        }
    }
}