using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDispay : UIelement
{
    [Tooltip("The text UI to use for display")]
    public Text displayText = null;

    public void DisplayTime()
    {
        if (displayText != null && GameManager.instance)
        {
            string formattedTime = SecondsToMmSs(GameManager.time);
            displayText.text = formattedTime;
        }
        if (Time.deltaTime > GameManager.instance.phase4)
        {
            displayText.text = "";
        }
    }

    static string SecondsToMmSs(float totalSeconds)
    {
        int minutes = (int)(totalSeconds / 60);
        int seconds = (int)totalSeconds % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    /// <summary>
    /// Description:
    /// Overides the virtual UpdateUI function and uses the DisplayScore to update the score display
    /// Inputs:
    /// none
    /// Returns:
    /// void (no return)
    /// </summary>
    public override void UpdateUI()
    {
        // This calls the base update UI function from the UIelement class
        base.UpdateUI();

        // The remaining code is only called for this sub-class of UIelement and not others
        DisplayTime();
    }
}
