using System;
using UnityEngine;


public class EmotionChangeEventArgs : EventArgs
{
    public Color GlobalLightColor {get;}
    public float GlobalLightIntensity {get;}
    public bool PlayerLightEnabled {get;}

    public EmotionChangeEventArgs(Color globalLightColor, float globalLightIntensity, bool playerLightEnabled)
    {
        GlobalLightColor = globalLightColor;
        GlobalLightIntensity = globalLightIntensity;
        PlayerLightEnabled = playerLightEnabled;
    }
}
