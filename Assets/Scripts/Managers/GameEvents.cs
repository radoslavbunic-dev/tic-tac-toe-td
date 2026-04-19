using System;
using System.Collections.Generic;

public static class GameEvents
{
    public static Action<string> PlaySFX;
    public static Action<string> PlayMusic;
    public static Action StopMusic;
    public static Action<bool> SetMusicOnOff;
    public static Action<bool> SetSFXOnOff;
}