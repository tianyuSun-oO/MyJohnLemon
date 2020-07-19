using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightUIPrinter : MonoBehaviour
{/*
  * 准星有红白两种颜色
  * 平时是白色，击中怪物时是红色
  */
    public Texture2D[] textures;
    public CircularProgress circularProgress;

    float timeInterval;
    public enum DrawCursorStatu{
        NONE,WHITE,RED
    }

    DrawCursorStatu isDrawCursor = DrawCursorStatu.NONE;
    void drawCursor()
    {/*
      * 绘制准星
      */
        switch(isDrawCursor)
        {
            case DrawCursorStatu.WHITE:
                Rect rect = new Rect(Input.mousePosition.x - (textures[0].width / 10),

                Screen.height - Input.mousePosition.y - (textures[0].height / 10),

                textures[0].width / 5, textures[0].height / 5);

                GUI.DrawTexture(rect, textures[0]);
                Cursor.visible = false;
                break;

            case DrawCursorStatu.RED:
                rect = new Rect(Input.mousePosition.x - (textures[1].width / 10),

                Screen.height - Input.mousePosition.y - (textures[1].height / 10),

                textures[1].width / 5, textures[1].height / 5);

                GUI.DrawTexture(rect, textures[1]);
                Cursor.visible = false;
                break;
        }
    }

    public void draw()//绘制白色准星
    {
        isDrawCursor = DrawCursorStatu.WHITE;
        circularProgress.startFill();
    }

    public void hit()//绘制红色准星
    {
        isDrawCursor = DrawCursorStatu.RED;
        timeInterval = 0.1f;
    }

    void OnGUI()
    {
        drawCursor();
    }

    void Update()
    {//红色准星会在0.1s后自动变回白色
        if(isDrawCursor == DrawCursorStatu.RED)
        {
            if (timeInterval > 0)
            {
                timeInterval -= Time.deltaTime;
            }
            else
            {
                draw();
                timeInterval = 0.1f;
            }
        }
    }
}
