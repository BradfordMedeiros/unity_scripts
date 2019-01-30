using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof(CharacterController))]
    public class FPS : MonoBehaviour
    {
        public float walkSpeed;
        public float runSpeed;
        public float jumpSpeed;
        public float stickToGroundForce;
        public float gravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;

        private Camera m_Camera;
        private bool shouldJump;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController characterController;
        private CollisionFlags m_CollisionFlags;
        private bool lastFrameGrounded;
        private bool isJumping;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            isJumping = false;
            m_MouseLook.Init(transform, m_Camera.transform);
        }

        private void Update()
        {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
            if (!shouldJump)
            {
                shouldJump = CrossPlatformInputManager.GetButton("Jump") && characterController.isGrounded;
            }

            if (!lastFrameGrounded && characterController.isGrounded)
            {
                m_MoveDir.y = 0f;
                isJumping = false;
            }
            if (!characterController.isGrounded && !isJumping && lastFrameGrounded)
            {
                m_MoveDir.y = 0f;
            }
            lastFrameGrounded = characterController.isGrounded;
        }

        private void FixedUpdate()
        {
            float speed;
            GetSpeedAndSetMInputFromInput(out speed);

            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo,
                               characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;


            if (characterController.isGrounded)
            {
                m_MoveDir.y = -stickToGroundForce;

                if (shouldJump)
                {
                    m_MoveDir.y = jumpSpeed;
                    shouldJump = false;
                    isJumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
            }
            m_CollisionFlags = characterController.Move(m_MoveDir * Time.fixedDeltaTime);

        }

        private void GetSpeedAndSetMInputFromInput(out float speed)
        {
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool isWalking = !Input.GetKey(KeyCode.LeftShift);
            speed = isWalking ? walkSpeed : runSpeed; 

            m_Input = new Vector2(horizontal, vertical);

            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }
        }
    }
}
