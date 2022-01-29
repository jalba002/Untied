#pragma warning disable CS0168
using com.kpg.ggj2022.player;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Spikes : TriggerArea
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // base.OnTriggerEnter2D(collision);
        try
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc != null)
            {
                // Enable the trigger
                OnTriggerActivation.Invoke();
                ChangeColor();
                // Kills instantly?
                pc.Respawn(); // 
            }
        }
        catch (Exception e)
        {

        }
    }
}
