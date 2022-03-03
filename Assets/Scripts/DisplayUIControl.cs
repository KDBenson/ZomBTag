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
    [SerializeField] GameObject messageDisplayUI;
    [SerializeField] Text messageDisplayText;

    [SerializeField] TimerLogic gameTimer;
    
    
    private void Awake()
    {
        if(messageDisplayText!=null)
        {
            messageDisplayText.text = "";
        }
        ShowHideUI(false);
    }

    public void ShowHideUI(bool visible)
    {
        messageDisplayUI.SetActive(visible);
    }

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
