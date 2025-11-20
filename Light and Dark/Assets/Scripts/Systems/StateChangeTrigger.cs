using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D),typeof(Rigidbody2D))]
public class StateChangeTrigger : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Color globalLightColour = Color.white;
    [SerializeField] private float globalLightIntensity = 1;
    [SerializeField] private bool enablePlayerLight = false;
    public static event EventHandler<EmotionChangeEventArgs> OnEmotionChangeTriggered;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log(gameObject.name);
            OnEmotionChangeTriggered?.Invoke(this, new EmotionChangeEventArgs(globalLightColour,globalLightIntensity, enablePlayerLight));
        }
    }


    void OnDrawGizmos()
    {
        if(boxCollider == null) {return;}
        Gizmos.color = Color.purple;
        Gizmos.DrawWireCube(transform.position, boxCollider.size);
    }
}
