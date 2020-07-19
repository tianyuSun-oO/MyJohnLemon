using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public KeyCode[] keyCodes;

    public bool m_IsFade = false;
    public float m_StayTime = 3f;
    public float m_Speed = 1f;
    Text m_Text;
    string m_TextString;
    void Awake()
    {
        m_Text = GetComponent<Text>();
        m_TextString = m_Text.text;
    }

    private void OnEnable()
    {
        m_Text.text = "";
    }

    void Update()
    {
        if (m_IsFade)//判断字体是否需要渐隐
        {//字体存在3s后慢慢消失
            if(m_Speed > 0)
            {
                m_Speed -= Time.deltaTime;
                m_Text.color = new Color(m_Text.color.r, m_Text.color.g, m_Text.color.b, m_Speed);
            }
        }
        else
        {
            bool timeOver = false;
            if (m_StayTime > 0)
            {
                m_StayTime -= Time.deltaTime;
            }
            else
            {
                timeOver = true;
            }
            m_IsFade = timeOver | m_IsFade;
        }
    }
    public void doFade(bool fast = false)//渐隐文字，参数为true立即渐隐文字
    {
        m_IsFade = true;
        if (fast) m_Speed = 0.01f;
    }
    public void doPaint()//绘制文字
    {
        m_Text.text = m_TextString;
        m_Text.color = new Color(m_Text.color.r, m_Text.color.g, m_Text.color.b, 255);
        
        m_IsFade = false;
        m_StayTime = 3f;
        m_Speed = 1f;
    }

}
