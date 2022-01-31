using com.kpg.ggj2022.player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabberController : RestartableObject
{
    public BoxCollider2D boxCollider2d;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerEnter2D(Collider2D collision)
    {
        // If the player is found while de swipe, then enable the   
        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

        if (pc != null)
        {
            if (!pc.IsVisible()) return;

            DisableGrab();
            animator.SetTrigger("Grab");
        }
    }

    public void KillPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(boxCollider2d.bounds.center, 4f);
        List<Collider2D> list = new List<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            list.Add(col);
            Debug.Log(col.gameObject.name);
        }
        try
        {
            PlayerController pc = list.Find(x => x.gameObject.GetComponent<PlayerController>() != null).GetComponent<PlayerController>(); pc.Kill();
            pc.StopMovement();
            // this.gameObject.GetComponent<Animator>().enabled = false;
            BallerinaAnimationController a = pc.gameObject.GetComponentInChildren<BallerinaAnimationController>();
            a.Kill();
            a.transform.parent = boxCollider2d.transform;
            a.gameObject.transform.localPosition = new Vector3(20, -10, 0);
            a.enabled = false;
        }
        catch (NullReferenceException)
        {
            animator.SetTrigger("Fail");
            return;
        }
    }

    public void EndGame()
    {
        GameManager.GM.RestartGame();
    }

    public void EnableGrab()
    {
        boxCollider2d.enabled = enabled;
    }

    public void DisableGrab()
    {
        boxCollider2d.enabled = false;
    }

    public void StartGrab()
    {
        animator.SetTrigger("Seek");
    }

    public override void Restart()
    {
        //base.Restart();
        animator.Rebind();
        animator.Update(0f);
    }
}
