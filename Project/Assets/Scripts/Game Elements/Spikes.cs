#pragma warning disable CS0168
using com.kpg.ggj2022.player;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Spikes : TriggerArea
{
    public bool debugMode = false;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // base.OnTriggerEnter2D(collision);
        if (!trapEnabled) return;

        try
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc != null)
            {
                // Enable the trigger
                OnTriggerActivation.Invoke();
                if (debugMode)
                    ChangeColor();
                // Kills instantly?
                pc.Respawn(); // 

                // Kills the pc instantly. Dies.
            }
        }
        catch (Exception e)
        {

        }
    }

    public void EnableTrap()
    {
        trapEnabled = true;
    }

    public void DisableTrap()
    {
        trapEnabled = false;
    }
}
