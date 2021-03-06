using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiScript : MonoBehaviour
{
    GUIStyle labelStyle;
    string message;

    void Start()
    {
        message = "これはサンプルメッセージです。";
    }

    void Update() { }

    void GuiSetup()
    {
        labelStyle = new GUIStyle();
        labelStyle.fontSize = 24;
        labelStyle.normal.textColor = Color.white;
    }

    void OnGUI()
    {
        GuiSetup();
        GUI.Box(new Rect(10, 10, 500, 200), "Message");
        GUI.Label(new Rect(35, 50, 700, 50),
            message, labelStyle);
        if (GUI.Button(new Rect(150, 120, 200, 50), "OK!"))
        {
            message = "クリックしました！";
        }
    }

}