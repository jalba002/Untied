#pragma warning disable CS0168
using com.kpg.ggj2022.player;
using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Spikes : TriggerArea
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerEnter2D(collision);
    }
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
                //if (debugMode)
                //    ChangeColor();
                // Kills instantly?
                //pc.Respawn(); 
                StabPlayer(pc);

                // Kills the pc instantly. Dies.
            }
        }
        catch (Exception e)
        {

        }
    }

    private void StabPlayer(PlayerController pc)
    {
        // Get player visuals, center on a point, play death animation.
        if (pc.IsDead) return;
        Debug.Log("Stabbing");
        pc.Kill();
        pc.StopMovement();
        this.gameObject.GetComponent<Animator>().enabled = false;
        BallerinaAnimationController a = pc.gameObject.GetComponentInChildren<BallerinaAnimationController>();
        a.transform.parent = this.transform;
        a.Kill("Death_Spike");
        a.gameObject.transform.localPosition = Vector2.zero + Vector2.up;
        a.enabled = false;
        // Maybe respawn it after a while.
    }

    public void EnableTrap()
    {
        trapEnabled = true;
    }

    public void DisableTrap()
    {
        trapEnabled = false;
    }

    public override void Restart()
    {
        try
        {
            this.gameObject.GetComponent<Animator>().enabled = true;
        }
        catch (MissingComponentException)
        {

        }
    }
}
