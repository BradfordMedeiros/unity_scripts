using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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

        private Camera camera;
        private bool shouldJump;
        private Vector3 currentMovementVector = Vector3.zero;
        private CharacterController characterController;
        private bool lastFrameGrounded;
        private bool isJumping;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            camera = Camera.main;
            isJumping = false;
            m_MouseLook.Init(transform, camera.transform);
        }

        private void Update()
        {
            m_MouseLook.LookRotation(transform, camera.transform);
            if (!shouldJump)
            {
                shouldJump = CrossPlatformInputManager.GetButton("Jump") && characterController.isGrounded;
            }

            if (!lastFrameGrounded && characterController.isGrounded)
            {
                currentMovementVector.y = 0f;
                isJumping = false;
            }
            if (!characterController.isGrounded && !isJumping && lastFrameGrounded)
            {
                currentMovementVector.y = 0f;
            }
            lastFrameGrounded = characterController.isGrounded;
        }

        private void FixedUpdate()
        {
            float speed;
            Vector2 motionVector;

            GetSpeedAndSetMInputFromInput(out speed, out motionVector);

            Vector3 desiredMove = transform.forward * motionVector.y + transform.right * motionVector.x;
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo,
                               characterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            currentMovementVector.x = desiredMove.x * speed;
            currentMovementVector.z = desiredMove.z * speed;


            if (characterController.isGrounded)
            {
                currentMovementVector.y = -stickToGroundForce;

                if (shouldJump)
                {
                    currentMovementVector.y = jumpSpeed;
                    shouldJump = false;
                    isJumping = true;
                }
            }
            else
            {
                currentMovementVector += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
            }
            characterController.Move(currentMovementVector * Time.fixedDeltaTime);
        }

        private void GetSpeedAndSetMInputFromInput(out float speed, out Vector2 motionVector)
        {
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            motionVector = new Vector2(horizontal, vertical);
            motionVector.Normalize();

            speed = !Input.GetKey(KeyCode.LeftShift) ? walkSpeed : runSpeed;
        }
    }
}
