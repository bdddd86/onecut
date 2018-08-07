using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TestNPC : NPCObject 
{

    private void Start()
    {
        SetNPCName();
    }

    private void OnMouseDown()
    {
        int gameMode = (int)GameManager.instance.currentGameMode;
        gameMode++;
        if(gameMode == Enum.GetNames(typeof(GameManager.eGameMode)).Length)
        {
            gameMode = 0; 
        }
        GameManager.instance.ChangeGameMode((GameManager.eGameMode)gameMode);
    }

    public void SetNPCName()
    {
        lblName.text = GameManager.instance.currentGameMode.ToString();    
    }
}
