using com.kpg.ggj2022.player;
using UnityEngine;

public class BallerinaAnimationController : MonoBehaviour
{
    private PlayerController pc;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        pc = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pc != null)
        {
            Vector3 vel = pc.GetVelocity();

            if (vel.y > 1f)
            {
                animator.ResetTrigger("Land");
                animator.SetBool("Jump", true);
            }

            if (pc.GetGrounded() && animator.GetBool("Jump"))
            {
                animator.SetTrigger("Land");
                animator.SetBool("Jump", false);
            }

            vel.y = 0f;
            animator.SetFloat("Movement", Mathf.Abs(vel.magnitude));

            animator.SetBool("Hide", !pc.IsVisible());
        }
    }

    public void Kill(string deathMode = "Death")
    {
        animator.SetTrigger(deathMode);
    }

    public void Restart()
    {
        animator.Rebind();
        animator.Update(0f);
    }

    public void Respawn()
    {
        GameManager.GM.RespawnPlayer();
        Restart();
        this.gameObject.transform.parent = GameManager.GM.player.transform;
        this.gameObject.transform.localPosition = Vector3.zero;
        this.gameObject.transform.localRotation = Quaternion.identity;
        this.enabled = true;
        animator.enabled = true;
    }
}
