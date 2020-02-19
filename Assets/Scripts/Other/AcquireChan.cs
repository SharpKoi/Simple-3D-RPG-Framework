using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator), typeof(MovementInput))]
public class AcquireChan : MonoBehaviour
{
    private Animator anim = null;
    private MovementInput input = null;

    [SerializeField] private GameObject katana = null;
    [SerializeField] private ParticleSystem slashTrail = null;


    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        input = GetComponent<MovementInput>();

        if(slashTrail == null) slashTrail = katana.GetComponentInChildren<ParticleSystem>();

        Landing();

        LockCursor();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            FreeCursor();
        }

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Hit01")) {
            if(anim.IsInTransition(0)) {
                Debug.Log("Transition");
                input.canMove = true;
            }
        }

        if(Input.GetButtonDown("Hit")) {
            input.canMove = false;
            anim.SetInteger("HitCombo", 1);
        }
    }

    public void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void FreeCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void Landing() {
        input.canMove = false;
        anim.SetTrigger("Landing");
    }

    public void CompleteLanding() {
        input.canMove = true;
    }

    public void Slash() {
        slashTrail.Play();
    }

    public void FinishSlash() {
        anim.SetInteger("HitCombo", 0);
        slashTrail.Clear();
        slashTrail.Stop();
    }
}
