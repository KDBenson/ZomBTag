using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* DisplayUIController handles gameplay logic for displaying game over conditions to user
 * UI begins hidden in the scene so it is only diplayed on game over condition.
 * requires a reference to the timerlogic in use for the scene.
 */
public class DisplayUIControl : MonoBehaviour
{
    //in editor assign this object to itself
    [SerializeField] GameObject messageDisplayUI;
    [SerializeField] Text messageDisplayText;
    //in editor assign the object canvas with the timer+timerlogic on it
    [SerializeField] TimerLogic gameTimer;
    
    
    private void Awake()
    {
        if(messageDisplayText!=null)
        {
            messageDisplayText.text = "";
        }
        ShowHideUI(false);
    }

    //toggle UI object according to boolean t=on,f=off.
    public void ShowHideUI(bool visible)
    {
        messageDisplayUI.SetActive(visible);
    }

    //checks game over condition from timer, sets appropriate message
    public void SetGameOverMessage()
    {
        string _msg = "Game Over\n";
        if(gameTimer.GameOverCondition())
        {
            _msg += "you survived zombie tag!";
        }
        else
        {            
            _msg += "the zombies won this time.";
        }
        messageDisplayText.text = _msg;
    }

    

}
