using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float movementSmooothing = 0.1f;
    [SerializeField] private NPCState state;
    [SerializeField] private PatrolPoint nextPatrolPoint;

    [Header("Detection Radius")]
    [SerializeField] private float detectionRadius = 2;
    [SerializeField] private LayerMask detectionLayers;
    private Rigidbody2D rb;
    private Vector3 refVelocity;
    private HashSet<GameObject> currentObjectsInDetection;
    private GameObject target;

    // I use this instead of the detection function for patrol points because I want the enemy to only switch patrol points,
    // only after the Enemy comes into physical contact with a patrol point.
    void OnTriggerEnter2D(Collider2D collision) 
    {
        if (state == NPCState.Patrolling && collision.TryGetComponent(out PatrolPoint point) && point == nextPatrolPoint)
        {
            nextPatrolPoint = point.NextPatrolPoint;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentObjectsInDetection = new HashSet<GameObject>();
        state = NPCState.Patrolling;
    }

    void FixedUpdate()
    {
        if (state == NPCState.Idle) { return; }
        switch (state)
        {
            case NPCState.Patrolling:
                MoveTo(nextPatrolPoint.transform.position);
                break;
            case NPCState.Chasing:
                MoveTo(target.transform.position);
                break;
        }

        Detect();
    }

    /// <summary>
    /// Detects and handles exit and entering logic from a distance.
    /// Can be expanded to handle interaction with NPCs etc.
    /// The detection logic can slightly hinder performance when multiple Enemies might be active, however 
    /// it can be easily optimised by adding small delays to when detection should happen, without hindering the realism behind the detection logic. 
    /// </summary>
    private void Detect()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayers);

        HashSet<GameObject> newDetected;

        HandleEnterColliders(colliders, out newDetected);

        HandleExitColliders(newDetected);
    }

    /// <summary>
    /// Handles logic relating to when objects have left the detection radius of this Enemy, and assignes the newly detected objects as the currently detected objects.
    /// </summary>
    /// <param name="newDetected">All the newly detected objects.</param>
    private void HandleExitColliders(HashSet<GameObject> newDetected)
    {
        foreach (GameObject oldTarget in currentObjectsInDetection)
        {
            if (oldTarget.CompareTag("Player") && !newDetected.Contains(oldTarget)) // meaning this enemy lost the player.
            {
                Debug.Log($"{gameObject.name} lost the player");
                state = NPCState.Patrolling;
                target = null;
            }
        }
        currentObjectsInDetection = newDetected;
    }

    /// <summary>
    /// Handles logic relating to objects that have just entered the detection radius of this Enemy.
    /// </summary>
    /// <param name="colliders"></param>
    /// <param name="newDetected"></param>
    private void HandleEnterColliders(Collider2D[] colliders, out HashSet<GameObject> newDetected)
    {
        newDetected = new HashSet<GameObject>();
        foreach (Collider2D collider in colliders)
        {
            newDetected.Add(collider.gameObject);

            if (currentObjectsInDetection.Contains(collider.gameObject)) 
            { 
                continue; 
            }
            
            if (collider.CompareTag("Player")) // meaning this enemy detected the player.
            {
                Debug.Log($"{gameObject.name} detected the player");
                target = collider.gameObject;
                state = NPCState.Chasing;
                continue;
            }
        }
    }

    private void MoveTo(Vector2 location)
    {
        float moveDir = CalculateMoveDirection(location);
        Vector3 targetVelocity = new Vector2(moveDir * moveSpeed * 10 * Time.fixedDeltaTime, rb.linearVelocity.y);
        rb.linearVelocity = Vector3.SmoothDamp(rb.linearVelocity, targetVelocity, ref refVelocity, movementSmooothing);
    }

    /// <summary>
    /// Calculates the move direction based on a target location.
    /// </summary>
    /// <param name="targetLocation">The target location to move towards.</param>
    /// <returns></returns>
    private float CalculateMoveDirection(Vector2 targetLocation)
    {
        if (targetLocation.x > transform.position.x)
        {
            return 1;
        }

        return -1;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

[Serializable]
public enum NPCState
{
    Idle,
    Patrolling,
    Chasing
}
