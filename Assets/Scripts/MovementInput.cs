using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	[Header("Grounded Check")]
	public bool isGrounded;
	public float slopeOffset = 0.3f;		//set big to detect correctly on slope
	public float distToGround = 0.01f;		//set small to modify the distance frome charactor to ground.
	[SerializeField] private Transform left_foot = null;
	[SerializeField] private Transform right_foot = null;
	[SerializeField] private Transform grl;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    public float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    public float VerticalAnimTime = 0.2f;
    [Range(0,1f)]
    public float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    public float StopAnimTime = 0.15f;


    public float verticalVel;			//set public to debug
    private Vector3 moveVector;
    public bool canMove;			//set public to debug

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator> ();
		cam = Camera.main;
		controller = this.GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		AnimatorStateInfo currentStateInfo = anim.GetCurrentAnimatorStateInfo(0);

		anim.SetBool("Grounded", CheckGrounded());
		
        if (!canMove) {
			Speed = 0;
			anim.SetFloat("Speed", Speed);
			return;
		}

		if(Input.GetButtonDown("Jump")) {
			JumpUp();
		}

		if(currentStateInfo.shortNameHash == Animator.StringToHash("Idle") || currentStateInfo.shortNameHash == Animator.StringToHash("Run")) {
			
		}

        InputMagnitude ();

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
			anim.SetFloat("VerticalWeight", verticalVel * gravity * 2f);
		}
		
		moveVector = new Vector3 (0, verticalVel, 0);
		controller.Move (moveVector);
	}

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

    public void RotateToCamera(Transform t) {
        var camera = Camera.main;
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        desiredMoveDirection = forward;

        t.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
    }

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
	
	public void JumpUp() {
		if(isGrounded && anim.GetBool("Grounded")) {
			verticalVel = 0;							//when player is on ground, ignore the extra vertical velocity.
			anim.SetFloat("VerticalWeight", 0);
			verticalVel += jumpForce;
			isGrounded = false;
		}
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

		//Physically move player
		if (Speed > moveThreshold) {
			//anim.SetFloat ("InputMagnitude", Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation ();
		} else if (Speed < moveThreshold) {
			//anim.SetFloat ("InputMagnitude", Speed, StopAnimTime, Time.deltaTime);
		}
	}
}
