using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool isGrounded;
	public float slopeOffset = 0.3f;		//set big to detect correctly on slope
	public float distToGround = 0.01f;		//set small to modify the distance frome charactor to ground to this value.

    private CharacterController controller;

	[SerializeField] private Transform left_foot = null;
	[SerializeField] private Transform right_foot = null;
	[SerializeField] private Transform grl; //ground detect ray launcher

    void Awake() {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() {
        CheckAnimatorGrounded();
    }

    public bool CheckAnimatorGrounded() {
		//controller.isGrounded
		bool controllerTest = controller.isGrounded;
		//double foot line raycast
		bool feetRayTest = true;
        if(left_foot != null && right_foot != null) {
            feetRayTest = Physics.Raycast(left_foot.position, Vector3.down, slopeOffset) || 
						Physics.Raycast(right_foot.position, Vector3.down, slopeOffset, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        }
		bool rayTest = Physics.Raycast(grl.position, Vector3.down, slopeOffset, Physics.AllLayers, QueryTriggerInteraction.Ignore); 

		return (isGrounded = (controllerTest || feetRayTest || rayTest));
	}

    public bool IsFloating() {
        return isGrounded && !Physics.Raycast(grl.position, Vector3.down, distToGround, Physics.AllLayers, QueryTriggerInteraction.Ignore);
    }
}
