using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class Player : MonoBehaviour {

        public Camera cam;
        public CharacterController controller;
        public Animator anim;
        public MovementInput input;
        public GroundChecker groundChecker;

        public float moveVector;

        public float moveSpeed;
        public float rotateSpeed;

        public float gravity = Mathf.Abs(Physics.gravity.y);
	    public float gravityModifier = 0.2f;

        public bool isGrounded;
        public bool canMove;

        void Awake() {
            if(cam == null) cam = Camera.main;
        }

        void Start() {
            
        }

        void Update() {
            anim.SetBool(input.VAR_GROUNDED_HASH, isGrounded);
        }

        void FixedUpdate() {
            if (!canMove) {
                moveVector = 0;
                anim.SetFloat("Speed", moveVector);
                return;
            }

            //Physically move player
            if (moveVector > input.moveThreshold) {
                PlayerMoveAndRotation ();
            }

            //Because of the big value of slopOffset, the charactor will seems to be floating on flat ground.
            //To fix this, we slightly decrease the distance frome charactor to ground at each frame.
            if(isGrounded) {
                if (groundChecker.IsFloating()) {
                    if(input.verticalVel <= 0) {
                        float modification = groundChecker.slopeOffset - groundChecker.distToGround + 0.1f;
                        controller.Move(new Vector3(0, -modification / 2, 0));
                    }
                } else {
                    input.verticalVel = 0;
                }
            }else {
                input.verticalVel += (- gravity) * gravityModifier * Time.deltaTime;
                if(Mathf.Approximately(input.verticalVel, 0)) input.verticalVel = 0;
                anim.SetFloat("VerticalWeight", input.verticalVel * gravity * 2f);
            }

            controller.Move (new Vector3 (0, input.verticalVel, 0));
        }

        void PlayerMoveAndRotation() {
            var forward = cam.transform.forward.normalized;
            var right = cam.transform.right.normalized;

            forward.y = 0f;
            right.y = 0f;

            Vector3 desiredMoveDirection = forward * input.InputZ + right * input.InputX;
            desiredMoveDirection.Normalize();

            transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), rotateSpeed);
            controller.Move(desiredMoveDirection * Time.deltaTime * moveSpeed * moveVector);
        }

        public void RotateToCamera(Transform t) {
            var camera = Camera.main;
            var forward = cam.transform.forward;
            var right = cam.transform.right;

            Vector3 desiredMoveDirection = forward;

            t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotateSpeed);
        }

        public void RotateTowards(Transform t) {
            transform.rotation = Quaternion.LookRotation(t.position - transform.position);
        }
    }
}
