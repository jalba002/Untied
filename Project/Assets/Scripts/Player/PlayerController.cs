using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using Interfaces;
using UnityEngine.Events;

namespace com.kpg.ggj2022.player
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerController : RestartableObject
    {
        [Title("Properties")] [Tooltip("Instantiate properties.")]
        public bool InstantiateProperties = true;
        public PlayerProperties Properties;

        [Header("Private Speeds")]
        Vector2 m_Direction;
        Vector2 m_Inertia;
        bool ResetInertiaOnGround;

        #region DebugZone

        [Title("Debug")] public bool debugMode = false;

        #endregion

        #region Inputs

        // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Interactions.html#multiple-controls-on-an-action

        #endregion

        #region Game-Specific Mechanics

        // All about the 2D stuff.
        // Rigidbody2D.
        // BoxCollider2D
        [Header("2D Game")]
        Rigidbody2D rb2d;
        BoxCollider2D bc2d;
        SpriteRenderer visualSprite;

        bool IsGrounded = false;
        public LayerMask groundLayers;
        public const float k_GroundDetectionRadius = 0.5f;
        public Transform groundSpot;
        [Header("Visuals")]
        [Tooltip("Child gameobject that contains the model and the animations.")]
        public Transform visuals;
        bool visibleByEnemies = true;
        bool ableToMove = true;



        private bool m_FacingRight = true;

        #endregion

        public UnityEvent OnLandEvent = new UnityEvent();
        public UnityEvent OnFreezeEvent = new UnityEvent();

        bool controlsEnabled = true;
        bool IsFrozen = false;

        #region Unity_Calls
        private new void Awake()
        {
            base.Awake();
            rb2d = GetComponent<Rigidbody2D>();
            bc2d = GetComponent<BoxCollider2D>();
            visualSprite = GetComponentInChildren<SpriteRenderer>();

            Properties = InstantiateProperties ? Instantiate(Properties) : Properties;
        }

        private void Start()
        {
            ableToMove = true;
            visibleByEnemies = true;
        }

        private void FixedUpdate()
        {
            CheckGround();
            ApplyVerticalVelocity();
            MoveCharacter();
            CheckFlip();
        }

        #endregion

        private void CheckGround()
        {
            Collider2D[] groundCheck = Physics2D.OverlapCircleAll(groundSpot.position, k_GroundDetectionRadius, groundLayers);
            foreach(Collider2D collider in groundCheck)
            {
                if(collider != bc2d)
                {
                    if (!IsGrounded)
                        OnLandEvent.Invoke();
                    IsGrounded = true;
                    return;
                }
            }
            IsGrounded = false;
        }

        void ApplyVerticalVelocity()
        {
            if (IsGrounded)
            {
                if (m_Inertia.magnitude > 0f && ResetInertiaOnGround)
                {
                    m_Inertia = Vector3.zero; // Resets the m_Inertia from the flying position when touching the ground.
                    ResetInertiaOnGround = false;
                }
            }
            else
            {
                m_Direction.y -= Properties.GravitySpeed*Time.deltaTime;

                if (!ResetInertiaOnGround) ResetInertiaOnGround = true;
            }
        }

        void MoveCharacter()
        {
            Vector2 m_TargetVel = new Vector2();
            m_TargetVel.x = m_Direction.x * Properties.WalkingSpeed;
            m_TargetVel.y = m_Direction.y;

            LimitVelocity(ref m_TargetVel);

            rb2d.velocity = m_TargetVel;
        }

        void CheckFlip()
        {
            if( (m_Direction.x > 0f && !m_FacingRight) || (m_Direction.x < 0f && m_FacingRight) ) 
            {
                Flip();
            }
        }

        void Flip()
        {
            m_FacingRight = !m_FacingRight;
            // Whatever, mostly flip the sprite. Maybe just the sprite.
            Vector3 newScale = transform.localScale;
            newScale.x *= -1;
            transform.localScale = newScale;
        }

        void LimitVelocity(ref Vector2 vel)
        {
            if (IsGrounded)
            {
                vel.x = Mathf.Clamp(vel.x, -Properties.MaxWalkingSpeed, Properties.MaxWalkingSpeed);
            }

            if (Properties.MaxGravity >= 0f)
                m_Direction.y = Mathf.Max(m_Direction.y, -Properties.MaxGravity);
        }

        #region Public Methods

        public void SetVelocity(Vector2 Velocity)
        {
            // Stuff for movement with platforms.
            this.m_Direction = Velocity;
            rb2d.velocity = Velocity;
        }

        public void SetInertia(Vector3 Inertia)
        {
            this.m_Inertia = Inertia;
            ResetInertiaOnGround = false;
        }

        #endregion

        #region Limiters

        private float LimitValue(float OriginalValue, float ComparisonValue, float Threshold = 1.2f)
        {
            // idea here is to divide the original value by the comparison one. 
            // If it is superior than the threshold then return comparison.
            if (Math.Abs(ComparisonValue) < 0.0001f) return OriginalValue;
            return (OriginalValue / ComparisonValue) > Threshold ? ComparisonValue : OriginalValue;
        }

        #endregion

        #region Callbacks

        public void Jump(InputAction.CallbackContext context)
        {
            if (!controlsEnabled || !ableToMove) return;
            // Debug.Log("Jumping input detected.");
            // Force remove from ground.
            if (IsGrounded && context.performed)
            {
                //LastJumpingSpeed = LimitValue(Properties.JumpingSpeed * Time.deltaTime, LastJumpingSpeed);
                m_Direction.y = Properties.JumpingSpeed;
                IsGrounded = false;
            }
        }

        //  private void JumpSpeed()
        //  {
        //      // USED WHEN KILLING ZOMBIES. DELETE AFTER
        //      Velocity.y = Properties.JumpingSpeed;
        //  }

        public void Move(InputAction.CallbackContext context)
        {
            if (!controlsEnabled || !ableToMove) return;
            var movement = context.ReadValue<Vector2>();
            //LastWalkingSpeed = LimitValue(Properties.WalkingSpeed * Time.deltaTime, LastWalkingSpeed, 1.1f);
            m_Direction.x = movement.x;
            // Velocity.z = movement.y;
        }

        public void Freeze(InputAction.CallbackContext context)
        {
            if (!controlsEnabled) return;
            // if (!context.performed) return;

            Debug.Log("Freezing mechanic!");
            if(context.performed)
            {
                // Change sprite color
                visualSprite.color = Color.blue;
                ableToMove = false;
                visibleByEnemies = false;
                SetVelocity(Vector2.zero);
            }
            else
            {
                // Return to original.x
                visualSprite.color = Color.green;
                ableToMove = true;
                visibleByEnemies = true;
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!enabled) return;
            // TODO Check here if the object is "Collidable" so we can treat collisions with the CharacterController.
            try
            {
                IPlayerCollide go = hit.gameObject.GetComponent<IPlayerCollide>();
                //if (go != null)
                //{
                //    go.Collide(gameObject, hit.point);
                //    if ((Controller.collisionFlags & CollisionFlags.Above) != 0)
                //    {
                //        go.CollideTop();
                //    }
                //    else if ((Controller.collisionFlags & CollisionFlags.Below) != 0)
                //    {
                //        if (go.CollideBottom(gameObject.transform.position))
                //            JumpSpeed();
                //    }
                //}

                //Debug.Log("Player detected collision with " + hit.gameObject.name);
            }
            catch (NullReferenceException)
            {
            }
        }

        #endregion

        #region Getters

        public Vector3 GetVelocity()
        {
            return m_Direction;
        }

        public bool GetFrozen()
        {
            return IsFrozen;
        }

        #endregion

        #region Enablers

        public void Kill()
        {
            this.controlsEnabled = false;
            //this.enabled = false;
        }

        public void ToggleControls(bool enable)
        {
            controlsEnabled = enable;
        }

        #endregion

        #region Debug

#if UNITY_EDITOR

        private void OnGUI()
        {
            if (!debugMode) return;
            EditorGUILayout.Vector3Field("Velocity", m_Direction);
            // EditorGUILayout.Vector3Field("Inertia", m_Inertia);
        }

        // private void OnDrawGizmosSelected()
        // {
        //     Gizmos.DrawSphere(this.gameObject.transform.position + Vector3.down * (Controller.height * 0.5f), 0.2f);
        // }
#endif

        #endregion

        #region Interfaces / Heritage

        public void Respawn()
        {
            GameManager.GM.RespawnPlayer();
        }

        #endregion
    }
}