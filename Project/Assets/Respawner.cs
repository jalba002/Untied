using com.kpg.ggj2022.player;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var pc = collision.gameObject.GetComponent<PlayerController>();

        if (pc != null)
        {
            pc.Respawn();
        }
    }
}
