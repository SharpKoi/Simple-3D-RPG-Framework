using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
//This script requires you to have setup your animator with 3 parameters, "InputMagnitude", "InputX", "InputZ"
//With a blend tree to control the inputmagnitude and allow blending between animations.
	[RequireComponent(typeof(CharacterController))]
	public class MovementInput : MonoBehaviour {

		public float velocity = 12;
		public float gravity = Mathf.Abs(Physics.gravity.y);
		public float gravityModifier = 0.2f;
		public float jumpForce = 0.4f;
		public float rotationSpeed = 0.1f;
		public float moveThreshold = 0.05f;
		public bool blockRotationPlayer;
		[Space]

		public float InputX;
		public float InputZ;
		public Vector3 desiredMoveDirection;
		public float Speed;

		[Header("Components")]
		public Camera cam;
		public CharacterController controller;
		public Animator anim;
		Equipable equipable;

		[Header("Grounded Check")]
		public bool isGrounded;
		public float slopeOffset = 0.3f;		//set big to detect correctly on slope
		public float distToGround = 0.01f;		//set small to modify the distance frome charactor to ground.
		[SerializeField] private Transform left_foot = null;
		[SerializeField] private Transform right_foot = null;
		[SerializeField] private Transform grl;

		// [Header("Animation Smoothing")]
		// [Range(0, 1f)]
		// public float HorizontalAnimSmoothTime = 0.2f;
		// [Range(0, 1f)]
		// public float VerticalAnimTime = 0.2f;
		// [Range(0,1f)]
		// public float StartAnimTime = 0.3f;
		// [Range(0, 1f)]
		// public float StopAnimTime = 0.15f;

		public float verticalVel;			//set public to debug
		private Vector3 moveVector;
		public bool inputBlock;
		public bool canMove;			//set public to debug

		AnimatorStateInfo currentState;
		AnimatorStateInfo nextState;

		/*Animator Hash*/
		public readonly int STATE_IDLE_HASH = Animator.StringToHash("Idle");
		public readonly int STATE_RUN_HASH = Animator.StringToHash("Run");
		public readonly int STATE_AIRBORNE_HASH = Animator.StringToHash("AirborneSM");
		public readonly int STATE_LANDING_HASH = Animator.StringToHash("Landing");

		public readonly int STATE_MELEECOMBAT_HASH = Animator.StringToHash("MeleeCombatSM");
		public readonly int STATE_COMBO1_HASH = Animator.StringToHash("Combo1");
		public readonly int STATE_COMBO2_HASH = Animator.StringToHash("Combo2");
		public readonly int STATE_COMBO3_HASH = Animator.StringToHash("Combo3");
		public readonly int STATE_COMBO4_HASH = Animator.StringToHash("Combo4");

		public readonly int VAR_GROUNDED_HASH = Animator.StringToHash("Grounded");
		public readonly int VAR_MELEE_ATTACK_HASH = Animator.StringToHash("MeleeAttack");
		public readonly int VAR_COMBO_STATE_TIME_HASH = Animator.StringToHash("ComboStateTime");
		public readonly int VAR_VERTICAL_WEIGHT_HASH = Animator.StringToHash("VerticalWeight");

		// Use this for initialization
		void Start () {
			anim = this.GetComponent<Animator> ();
			equipable = GetComponent<Equipable>();
			cam = Camera.main;
			controller = this.GetComponent<CharacterController> ();
		}


		/*****TODO: move to Player class*****/
		void FixedUpdate () {
			if (!canMove) {
				Speed = 0;
				anim.SetFloat("Speed", Speed);
				return;
			}

			//Physically move player
			if (Speed > moveThreshold) {
				//anim.SetFloat ("InputMagnitude", Speed, StartAnimTime, Time.deltaTime);
				PlayerMoveAndRotation ();
			} else if (Speed < moveThreshold) {
				//anim.SetFloat ("InputMagnitude", Speed, StopAnimTime, Time.deltaTime);
			}

			//Because of the big value of slopOffset, the charactor will seems to be floating on flat ground.
			//To fix this, we slightly decrease the distance frome charactor to ground at each frame.
			if(isGrounded) {
				if (Physics.Raycast(grl.position, Vector3.down, distToGround, Physics.AllLayers, QueryTriggerInteraction.Ignore)) {
					verticalVel = 0;
				} else {
					if(verticalVel <= 0)
					controller.Move(new Vector3(0, -(slopeOffset - distToGround + 0.1f), 0));
				}
			}else {
				verticalVel += (- gravity) * gravityModifier * Time.deltaTime;
				if(Mathf.Approximately(verticalVel, 0)) verticalVel = 0;
				anim.SetFloat(VAR_VERTICAL_WEIGHT_HASH, verticalVel * gravity * 2f);
			}
			
			moveVector = new Vector3 (0, verticalVel, 0);
			controller.Move (moveVector);
		}

		void Update() {
			currentState = anim.GetCurrentAnimatorStateInfo(0);
			nextState = anim.GetNextAnimatorStateInfo(0);

			if(equipable != null) anim.SetBool("Equipped", equipable.isEquipped);

			CheckWeaponActive();

			// anim.ResetTrigger(VAR_MELEE_ATTACK_HASH);
			anim.SetFloat(VAR_COMBO_STATE_TIME_HASH, Mathf.Repeat(currentState.normalizedTime, 1f));

			anim.SetBool("Grounded", CheckGrounded());

			if(inputBlock) return;
			
			if(Input.GetButtonDown("Jump")) {
				if(isGrounded && anim.GetBool("Grounded")) {
					if(currentState.shortNameHash == STATE_IDLE_HASH || currentState.shortNameHash == STATE_RUN_HASH)
						JumpInput();
				}
			}

			if(Input.GetButtonDown("Attack")) {
				anim.SetTrigger(VAR_MELEE_ATTACK_HASH);
			}

			InputMagnitude ();
		}


		/*****TODO: move to Player class*****/
		void PlayerMoveAndRotation() {
			var camera = Camera.main;
			var forward = cam.transform.forward.normalized;
			var right = cam.transform.right.normalized;

			forward.y = 0f;
			right.y = 0f;

			desiredMoveDirection = forward * InputZ + right * InputX;
			desiredMoveDirection.Normalize();

			if (blockRotationPlayer == false) {
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (desiredMoveDirection), rotationSpeed);
				controller.Move(desiredMoveDirection * Time.deltaTime * velocity * Speed);
			}
		}

		/*****TODO: move to Player class*****/
		public void RotateToCamera(Transform t) {
			var camera = Camera.main;
			var forward = cam.transform.forward;
			var right = cam.transform.right;

			desiredMoveDirection = forward;

			t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
		}

		/*****TODO: move to Player class*****/
		public void RotateTowards(Transform t) {
			transform.rotation = Quaternion.LookRotation(t.position - transform.position);

		}
		
		void OnControllerColliderHit(ControllerColliderHit colliderHit) {
			if(colliderHit.normal.y + slopeOffset < 0) {
				isGrounded = true;
			}
		}

		public bool CheckGrounded() {
			//controller.isGrounded
			bool controllerTest = controller.isGrounded;
			//double foot line raycast
			bool feetRayTest = Physics.Raycast(left_foot.position, Vector3.down, slopeOffset) || 
							Physics.Raycast(right_foot.position, Vector3.down, slopeOffset, Physics.AllLayers, QueryTriggerInteraction.Ignore);
			bool rayTest = Physics.Raycast(grl.position, Vector3.down, slopeOffset, Physics.AllLayers, QueryTriggerInteraction.Ignore); 

			return (isGrounded = (controllerTest || feetRayTest || rayTest));
		}

		public void MeleeAttackStart() {
			if(equipable != null) {
				GameObject weapon = equipable.weapon;
				if(weapon != null) {
					weapon.GetComponent<MeleeWeapon>().StartAttack();
				}
			}
		}

		public void MeleeAttackEnd() {
			if(equipable != null) {
				GameObject weapon = equipable.weapon;
				if(weapon != null) {
					weapon.GetComponent<MeleeWeapon>().EndAttack();
				}
			}
		}

		public bool IsInCombo() {
			bool inCombo = nextState.shortNameHash == STATE_COMBO1_HASH || currentState.shortNameHash == STATE_COMBO1_HASH;
				inCombo |= nextState.shortNameHash == STATE_COMBO2_HASH || currentState.shortNameHash == STATE_COMBO2_HASH;
				inCombo |= nextState.shortNameHash == STATE_COMBO3_HASH || currentState.shortNameHash == STATE_COMBO3_HASH;
				inCombo |= nextState.shortNameHash == STATE_COMBO4_HASH || currentState.shortNameHash == STATE_COMBO4_HASH;

			return inCombo;
		}

		public void CheckWeaponActive() {
			if(equipable != null) {
				if(equipable.isEquipped) equipable.weapon.SetActive(IsInCombo());
			}
		}
		
		public void JumpInput() {
			verticalVel = 0;							//when player is on ground, ignore the extra vertical velocity.
			anim.SetFloat("VerticalWeight", 0);
			verticalVel += jumpForce;
			isGrounded = false;
		}

		void InputMagnitude() {
			//Calculate Input Vectors
			InputX = Input.GetAxis ("Horizontal");
			InputZ = Input.GetAxis ("Vertical");

			//anim.SetFloat ("InputZ", InputZ, VerticalAnimTime, Time.deltaTime * 2f);
			//anim.SetFloat ("InputX", InputX, HorizontalAnimSmoothTime, Time.deltaTime * 2f);

			//Calculate the Input Magnitude
			Speed = Mathf.Clamp01(new Vector2(InputX, InputZ).sqrMagnitude);
			anim.SetFloat("Speed", Speed);
		}
	}
}
