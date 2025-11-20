using UnityEngine;

public class PatrolPoint : MonoBehaviour
{

    [field: SerializeField] public PatrolPoint NextPatrolPoint { get; private set; }
    [SerializeField]private BoxCollider2D boxCollider;


    void OnDrawGizmos()
    {
        if(boxCollider == null) {return;}
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position,boxCollider.size);
    }
}
