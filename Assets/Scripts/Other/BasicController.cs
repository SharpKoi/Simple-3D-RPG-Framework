using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator), typeof(MovementInput))]
public class BasicController : MonoBehaviour
{
    Animator anim = null;

    void Awake() {
        DOTween.Init();
    }

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();

        Landing();
    }

    public void Landing() {
        // input.canMove = true;
        anim.SetTrigger("Landing");
    }

    public void CompleteLanding() {
        // input.canMove = true;
    }
}
