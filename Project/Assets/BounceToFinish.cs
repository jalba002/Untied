using com.kpg.ggj2022.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BounceToFinish : MonoBehaviour
{
    public GameObject FadeIn;
    public UnityEvent OnActivation = new UnityEvent();

    public void Start()
    {
        FadeIn.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<PlayerController>().SetVelocity(new Vector2(0.0f, 50.0f));
        OnActivation.Invoke();
        FadeIn.SetActive(true);
    }
}
