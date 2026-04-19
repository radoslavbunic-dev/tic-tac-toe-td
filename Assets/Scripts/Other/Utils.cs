using System;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static string ConvertTimeToSeconds(float secondsLeft)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(secondsLeft);
        string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        return formattedTime;
    }

    public static string ConvertTimeToMilliSeconds(float secondsLeft)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(secondsLeft);
        string formattedTime = string.Format("{0:D2}:{1:D2}.{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
        return formattedTime;
    }
}