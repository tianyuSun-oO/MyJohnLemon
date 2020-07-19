using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{/*
  * 游戏结束
  * 玩家逃离播放逃离动画
  * 玩家死亡播放玩家死亡动画
  * 并播放失败动画，按任意键重新开始
  */
    public float fadeDuration = 5f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;
    public Text text;

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;
    bool m_HasAudioPlayed;
    float m_Texttime = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void Update()
    {
        if (m_IsPlayerAtExit)
        {
            EndLevel(exitBackgroundImageCanvasGroup, true, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            EndLevel(caughtBackgroundImageCanvasGroup, false, caughtAudio);
        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doQuit, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }

        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doQuit)
            {
                Application.Quit();
            }
            else
            {
                if (m_Texttime < 1)
                {
                    m_Texttime += Time.deltaTime;
                    text.color = new Color(text.color.r, text.color.g, text.color.b, m_Texttime);
                }
                if (m_Texttime >= 1)
                {
                    if (Input.anyKey)
                    {
                        SceneManager.LoadScene(0);
                    }

                }
            }
            
        }
    }
}