using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CircularProgress : MonoBehaviour
{
    public Image progressCircle;

    //设置目标进度与当前进度
    float m_TargetProgress = 100.0f;
    float m_CurrentProgress;
    bool m_IsFill = false;

    void Update()
    {
        if(m_IsFill)
            ToChange();
    }

    void ToChange()
    {
        if (m_CurrentProgress < m_TargetProgress)
        {
            m_CurrentProgress += Time.deltaTime * 100;
            if (m_CurrentProgress >= m_TargetProgress)
            {
                m_CurrentProgress = 100.0f;
            }
            progressCircle.fillAmount = m_CurrentProgress / 100;
        }
        else
        {
            m_IsFill = false;
        }
    }

    public void fill()
    {
        m_IsFill = true;
        m_CurrentProgress = 0f;
    }

    public void startFill()
    {
        progressCircle.color = new Color(progressCircle.color.r, progressCircle.color.g, progressCircle.color.b, 255);
    }
}