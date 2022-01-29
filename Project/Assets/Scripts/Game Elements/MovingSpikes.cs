using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingSpikes : Spikes
{
    Rigidbody2D rb2d;

    [Header("Movement")]
    public bool enableMovement;

    [Header("Settings")]
    public Vector2 direction;
    public float delayBetweenMovements = 5f;
    public float maxHeight = 1f;

    private new void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
    }

    //IEnumerator AutoMovement()
    //{
    //    while(enableMovement)
    //    {
    //        Vector2 distance = direction;
    //        distance.y = Mathf.Min(maxHeight, direction.y);

    //        Vector2 newPos = Vector2.SmoothDamp(rb2d.velocity, rb2d.position + distance, ref direction, 1f);
    //        rb2d.MovePosition(newPos);
            
    //    }
    //}
}
