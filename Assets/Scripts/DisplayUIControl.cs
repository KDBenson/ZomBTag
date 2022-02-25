using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            _msg += "You won Zombie tag!";
        }
        else
        {            
            _msg += "You lost Zombie Tag.";
        }
        messageDisplayText.text = _msg;
    }

    

}
