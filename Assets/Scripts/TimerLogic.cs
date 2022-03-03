using UnityEngine;
using UnityEngine.UI;
/*This timer logic script handles displaying how long the scene is running for
 * and returning the game over condition ie; did at least one human survive zombie tag for the goal time.
 */
public class TimerLogic : MonoBehaviour
{
    public Text timerText;
    float curTime = 0f;
    bool timerActive = true;    
    //time game will run for can be set in inspector
    public float goalTime = 60f;

    //initialize text
    private void Start()
    {
        timerText.text = "0";       
    }
    //update timer display every frame timer is 'on' for
    private void Update()
    {
        if (timerActive)
        {
            curTime += Time.deltaTime;
            timerText.text = System.Math.Round(curTime, 2).ToString();
        }        
    }
    //timer on/off t=on, f=off.
    public void SetTimerActive(bool _isActive)
    {
        timerActive = _isActive;
    }
    //returns false if there's still time on the clock (player loses) or true if they've run out the clock
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