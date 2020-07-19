using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Transform rightHand;
    public TextController boxTextController;//文字控制
    public TextController weaponTextController;//武器文字控制

    public PlayerController player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name == "JohnLemon")
        {
            boxTextController.doPaint();
            openBox(other);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.name == "JohnLemon")
        {
            openBox(other);
        }
    }
    void OnTriggerExit(Collider other)
    {
        boxTextController.doFade();//渐隐文字
    }

    private void openBox(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))//f键打开宝箱
        {
            GetComponent<AudioSource>().Play();

            foreach (BoxCollider boxCollider in transform.GetComponents<BoxCollider>())
            {
                Destroy(boxCollider);//销毁宝箱的碰撞体
            }

            Transform weapon = transform.Find("Healmatic500");
            GetComponent<Animator>().SetBool("IsOpen", true);//改变宝箱动画

            if(weapon != null)//如果箱子内有武器
            {
                boxTextController.doFade(true);
                weaponTextController.doPaint();
                weapon.tag = "Weapon";
                player.getWeapon();

                //把武器放在右手位置
                weapon.parent = rightHand.parent;
                weapon.localPosition = new Vector3(-0.3f, -0.1f, 0f);
                weapon.localRotation = Quaternion.Euler(0, 90, 180);
            }
            
            //播放人物动作
            if (other.GetComponent<Animator>().GetBool("GetWeapon"))
            {
                other.GetComponent<Animator>().Play("Fight_BoxOpen");
            }
            else
            {
                other.GetComponent<Animator>().Play("BoxOpen");
            }
        }
    }
}
