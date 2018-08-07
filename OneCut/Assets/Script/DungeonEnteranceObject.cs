using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonEnteranceObject : Gimmick 
{
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(this.gameObject.transform.position.x < other.gameObject.transform.position.x)
            {
                GameManager.instance.ChangeGameMode(GameManager.eGameMode.eLevelup);
            }
            else if( this.gameObject.transform.position.x > other.gameObject.transform.position.x)
            {                    
                GameManager.instance.ChangeGameMode(GameManager.eGameMode.eAdventure);
            }
        }
    }
}
