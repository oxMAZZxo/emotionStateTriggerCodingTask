using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField]private Light2D globalLight;
    [SerializeField]private GameObject playerLight;

    private void OnEmotionChanged(object sender, EmotionChangeEventArgs e)
    {
        playerLight.SetActive(e.PlayerLightEnabled);
        globalLight.color = e.GlobalLightColor;
        globalLight.intensity = e.GlobalLightIntensity;
    }

    void OnEnable()
    {
        StateChangeTrigger.OnEmotionChangeTriggered += OnEmotionChanged;
    }

    void OnDisable()
    {
        StateChangeTrigger.OnEmotionChangeTriggered -= OnEmotionChanged;
        
    }

    
}
