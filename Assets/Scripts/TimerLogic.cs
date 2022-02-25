using UnityEngine;
using UnityEngine.UI;

public class TimerLogic : MonoBehaviour
{
    public Text timerText;
    float curTime = 0f;
    bool timerActive = true;
    
    //game score variables
    float goalTime = 60f;

    private void Start()
    {
        timerText.text = "0";       
    }
    public void SetTimerActive(bool _isActive)
    {
        timerActive = _isActive;
    }
    private void Update()
    {
        if (timerActive)
        {
            curTime += Time.deltaTime;
            timerText.text = System.Math.Round(curTime, 2).ToString();
        }        
    }

    public bool GameOverCondition()
    {
        bool _winGame = false;
        if (curTime >= goalTime)
        {
            _winGame = true;
        }
        return _winGame;
    }

}