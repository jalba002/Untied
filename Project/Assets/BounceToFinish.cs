using com.kpg.ggj2022.player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceToFinish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
       GetComponent<PlayerController>().SetVelocity(new Vector2(0.0f, 90.0f));
    }
}
