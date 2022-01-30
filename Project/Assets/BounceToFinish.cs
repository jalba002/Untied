using com.kpg.ggj2022.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceToFinish : MonoBehaviour
{
    public GameObject FadeIn;

    public void Start()
    {
        FadeIn.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<PlayerController>().SetVelocity(new Vector2(0.0f, 50.0f));
        FadeIn.SetActive(true);
    }
}
