using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WarpController : MonoBehaviour
{
    [SerializeField] private CombatController combat = null;

    [Space]

    public float warpDuration = .5f;

    [Space]

    private Vector3 swordOrigRot;
    private Vector3 swordOrigPos;

    [HideInInspector]
    public Vector3 forwardBeforeWarp;
        
    // Start is called before the first frame update
    void Start()
    {
        swordOrigPos = combat.sword.localPosition;
        swordOrigRot = combat.sword.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(combat.GetScreenTargets().Count >= 1 && combat.isLocked && combat.GetMovementInput().canMove) {
                combat.GetMovementInput().RotateTowards(combat.GetTarget());
                combat.GetMovementInput().canMove = false;
                forwardBeforeWarp = transform.forward;
                forwardBeforeWarp.y = 0;
                combat.swordParticle.Play();

                combat.ShowSword(true);

                combat.GetAnimator().SetTrigger("Slash");
            }
        }
    }

    public void Warp()
    {
        combat.canHit = false;
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
        Destroy(clone.GetComponent<CombatController>().sword.gameObject);
        Destroy(clone.GetComponent<Animator>());
        Destroy(clone.GetComponent<CombatController>());
        Destroy(clone.GetComponent<MovementInput>());
        Destroy(clone.GetComponent<CharacterController>());

        SkinnedMeshRenderer[] skinMeshList = clone.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            Material[] matList = smr.materials;
            for(int i = 0; i < matList.Length; i++) {
                matList[i] = combat.glowMaterial;
                matList[i].DOFloat(0.02f, "_AlphaThreshold", 1f).OnComplete(()=>Destroy(clone));
            }
            smr.materials = matList;
        }

        combat.ShowBody(false);
        combat.GetAnimator().speed = 0;
        combat.GetAnimator().applyRootMotion = false;

        Time.timeScale = 0.7f;
        transform.DOMove(combat.GetTarget().position, warpDuration).SetEase(Ease.InExpo).OnComplete(()=>FinishWarp());

        combat.sword.parent = null;

        combat.GlowAmount(combat.sword.gameObject, 30);
        DOVirtual.Float(30, 0, .8f, (x)=>combat.GlowAmount(combat.sword.gameObject, x));

        combat.sword.DOMove(combat.GetTarget().position + Vector3.up, warpDuration/1.2f);
        combat.sword.DOLookAt(combat.GetTarget().position, warpDuration/10f, AxisConstraint.None);

        //Particles
        combat.blueTrail.Play();
        combat.whiteTrail.Play();

        //Lens Distortion
        DOVirtual.Float(0, -80, .2f, combat.DistortionAmount);
        DOVirtual.Float(1, 2f, .2f, combat.ScaleAmount);
    }

    void FinishWarp()
    {
        combat.ShowBody(true);
        Time.timeScale = 1;

        combat.sword.parent = combat.swordHand;
        combat.sword.localPosition = swordOrigPos;
        combat.sword.localEulerAngles = swordOrigRot;

        combat.GlowAmount(gameObject, 30);
        DOVirtual.Float(30, 0, .8f, (x)=>combat.GlowAmount(gameObject, x));

        Instantiate(combat.hitParticle, combat.sword.position, Quaternion.identity);

        combat.GetTarget().GetComponent<TargetScript>().GetHit(20);
        combat.GetTarget().parent.DOMove(combat.GetTarget().position + forwardBeforeWarp, .5f);

        StartCoroutine(HideSword());
        StartCoroutine(PlayAnimation());
        StartCoroutine(StopParticles());

        combat.LockInterface(false);
        combat.aim.color = Color.clear;

        //Shake
        combat.ScreenImpulse(Vector3.right);

        //Lens Distortion
        DOVirtual.Float(-80, 0, .2f, combat.DistortionAmount);
        DOVirtual.Float(2f, 1, .1f, combat.ScaleAmount);
    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(.2f);
        combat.GetAnimator().applyRootMotion = true;
        combat.GetAnimator().speed = 1;
    }

    IEnumerator StopParticles()
    {
        yield return new WaitForSeconds(.2f);
        combat.blueTrail.Stop();
        combat.whiteTrail.Stop();
    }

    IEnumerator HideSword()
    {
        yield return new WaitForSeconds(1.3f);
        combat.swordParticle.Play();

        GameObject swordClone = Instantiate(combat.sword.gameObject, combat.sword.position, combat.sword.rotation);

        // swordMesh.enabled = false;

        combat.ShowSword(false);

        MeshRenderer swordMR = swordClone.GetComponentInChildren<MeshRenderer>();
        Material[] materials = swordMR.materials;

        for (int i = 0; i < materials.Length; i++)
        {
            Material m = combat.glowMaterial;
            materials[i] = m;
        }

        swordMR.materials = materials;

        for (int i = 0; i < swordMR.materials.Length; i++)
        {
            swordMR.materials[i].DOFloat(1, "_AlphaThreshold", .3f).OnComplete(() => Destroy(swordClone));
        }

        combat.GetMovementInput().canMove = true;
        combat.canHit = true;
    }
}
