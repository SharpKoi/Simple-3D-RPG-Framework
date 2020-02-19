using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cinemachine;
using System;
using UnityEngine.Rendering.PostProcessing;

public class CombatController : MonoBehaviour
{
    private MovementInput input = null;
    private Animator anim = null;
    public AnimatorStateInfo currentState;

    public bool isLocked;

    public CinemachineFreeLook cameraFreeLook;
    private CinemachineImpulseSource impulse;
    private PostProcessVolume postVolume;
    private PostProcessProfile postProfile;

    [Space]

    private List<Transform> screenTargets = new List<Transform>();
    private Transform target;

    [Space]

    public Transform sword;
    public Transform swordHand;

    [Space]
    [SerializeField] private GameObject rightLeg = null;
    [SerializeField] private GameObject rightFist = null;

    [Space]
    
    [Header("Materials")]
    public Material glowMaterial;

    [Space]

    private MeshRenderer[] swordMeshArr;

    [Space]

    [Header("Particles")]
    public ParticleSystem blueTrail;
    public ParticleSystem whiteTrail;
    public ParticleSystem swordParticle;

    [Space]

    [Header("Trails")]
    [SerializeField]private TrailRenderer rightFootTrail = null;
    [SerializeField]private TrailRenderer rightFistTrail = null;

    [Space]

    [Header("Prefabs")]
    public GameObject hitParticle;

    [Space]

    [Header("Canvas")]
    public Image aim;
    public Image lockAim;
    public GameObject enemyHpBar;
    public Vector2 uiOffset;

    [Space]

    public int comboHits;
    public bool canHit;
    public bool isFirstHit = true;

    // Start is called before the first frame update
    void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        canHit = true;
        comboHits = 0;

        input = GetComponent<MovementInput>();
        anim = GetComponent<Animator>();
        impulse = cameraFreeLook.GetComponent<CinemachineImpulseSource>();
        postVolume = Camera.main.GetComponent<PostProcessVolume>();
        postProfile = postVolume.profile;
        rightFootTrail.emitting = false;
        rightFistTrail.emitting = false;

        swordMeshArr = sword.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer mesh in swordMeshArr) {
            mesh.enabled = false;
        }
        
        Landing();
    }

    // Update is called once per frame
    void Update() {
        //跑步動畫(已移至MovementInput)
        // anim.SetFloat("Speed", input.Speed);
        
        //將距離大於15m或已死亡的敵人從目標集合中移除
        for(int i = 0; i < screenTargets.Count; i++) {
            Transform target = screenTargets[i];
            if(Vector3.Magnitude(target.parent.position - transform.position) > 15
                || target.GetComponentInChildren<TargetScript>().isDead) {
                screenTargets.RemoveAt(i);
                i--;
            }
        }

        //取得當前動畫狀態
        currentState = anim.GetCurrentAnimatorStateInfo(0);

        //玩家發動攻擊時瞬移至敵人面前
        if(Input.GetButtonDown("Hit")) {
            if(screenTargets.Count >= 1) {
                if(target.GetComponent<TargetScript>().GetDistanceFromPlayer(this) <= 7) {
                    input.RotateTowards(target);
                }
            }
            ComboStart();
        }

        //
        if(currentState.IsName("Hit01") || currentState.IsName("Hit03")) {
            if(anim.IsInTransition(0)) {
                isFirstHit = true;
            }else {
                if(isFirstHit) {
                    Collider collider = rightLeg.GetComponent<BoxCollider>();
                    HitCheck(collider, 5);
                    isFirstHit = false;
                }
            }
        }else if(currentState.IsName("Hit02")) {
            if(anim.IsInTransition(0)) {
                isFirstHit = true;
            }else {
                if(isFirstHit) {
                    Collider collider = rightFist.GetComponent<BoxCollider>();
                    HitCheck(collider, 5);
                    isFirstHit = false;
                }
            }
        }

        if(currentState.IsName("Hit03") && anim.IsInTransition(0)) {
            anim.SetInteger("HitCombo", 0);
            rightFootTrail.emitting = false;
            
            input.canMove = true;
            canHit = true;
            comboHits = 0;
        }

        if (!isLocked && input.canMove && screenTargets.Count >= 1) {
            target = screenTargets[targetIndex()];
        }

        //玩家介面更新
        UserInterface();

        if (!input.canMove)
            return;

        if (screenTargets.Count < 1)
            return;

        if (Input.GetMouseButtonDown(1)) {
            LockInterface(true);
        }

        if (Input.GetMouseButtonUp(1) && input.canMove) {
            LockInterface(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Cursor.visible = !Cursor.visible;
            if(Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.None;
            }else {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private void UserInterface()
    {
        if(target == null) {
            aim.color = Color.clear;
            return;
        }

        aim.transform.position = Camera.main.WorldToScreenPoint(target.position + (Vector3)uiOffset);
        
        if(screenTargets.Count >= 1 && target.parent != null) {
            Enemy enemy = target.parent.GetComponent<Enemy>();
            enemyHpBar.GetComponent<Slider>().value = (float)enemy.hp / (float)enemy.maxHp;
            if(!enemyHpBar.activeSelf) {
                enemyHpBar.SetActive(true);
            }
        }else {
            enemyHpBar.SetActive(false);
        }

        if (!input.canMove)
            return;

        Color c = screenTargets.Count < 1 ? Color.clear : Color.white;
        aim.color = c;
    }

    public void LockInterface(bool state)
    {
        float size = state ? 1 : 2;
        float fade = state ? 1 : 0;
        lockAim.DOFade(fade, .15f);
        lockAim.transform.DOScale(size, .15f).SetEase(Ease.OutBack);
        lockAim.transform.DORotate(Vector3.forward * 180, .15f, RotateMode.FastBeyond360).From();
        aim.transform.DORotate(Vector3.forward * 90, .15f, RotateMode.LocalAxisAdd);

        isLocked = state;
    }

    public void ComboStart() {
        input.canMove = false;

        if(canHit) {
            comboHits++;
        }

        if(comboHits == 1) {
            anim.SetInteger("HitCombo", 1);
            if(target.GetComponent<TargetScript>().GetDistanceFromPlayer(this) <= 7) {
                CloseToTarget();
            }
        }
    }

    public void AnimateHit() {
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        
        if(currentState.IsName("Hit01") || currentState.IsName("Hit03")) {
            rightFootTrail.emitting = true;
        }
        else if(currentState.IsName("Hit02")) {
            rightFistTrail.emitting = true;
        }
    }

    public void ComboCheck() {
        canHit = false;

        if(currentState.IsName("Hit01")) {
            if(comboHits > 1) {
                anim.SetInteger("HitCombo", 2);
            }else {
                input.canMove = true;
                anim.SetInteger("HitCombo", 0);
                comboHits = 0;
            }
            canHit = true;
            rightFootTrail.emitting = false;
        }else if(currentState.IsName("Hit02")) {
            if(comboHits > 2) {
                anim.SetInteger("HitCombo", 3);
            }
            else {
                input.canMove = true;
                anim.SetInteger("HitCombo", 0);
                comboHits = 0;
            }
            canHit = true;
            rightFistTrail.emitting = false;
        }
    }

    public void HitCheck(Collider col, int damage) {
        if(col.bounds.Intersects(target.parent.GetComponent<CapsuleCollider>().bounds)) {
            target.GetComponent<TargetScript>().GetHit(damage);
            impulse.GenerateImpulse();
        }
    }

    public void ShowBody(bool state)
    {
        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.enabled = state;
        }
    }

    public void GlowAmount(GameObject obj, float x)
    {
        if(obj.layer == 10) {
            SkinnedMeshRenderer[] skinMeshList = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (SkinnedMeshRenderer smr in skinMeshList) {
                smr.material.SetVector("_FresnelAmount", new Vector4(x, x, x, x));
            }
        }else {
            MeshRenderer[] meshList = obj.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer smr in meshList)
            {
                foreach(Material mt in smr.materials) {
                    mt.SetColor("_GlowColor", Color.red);
                    mt.SetVector("_FresnelAmount", new Vector4(x, x, x, x));
                }
            }
        }
    }

    public void DistortionAmount(float x)
    {
        postProfile.GetSetting<LensDistortion>().intensity.value = x;
    }
    public void ScaleAmount(float x)
    {
        postProfile.GetSetting<LensDistortion>().scale.value = x;
    }

    public int targetIndex()
    {
        float[] distances = new float[screenTargets.Count];

        for (int i = 0; i < screenTargets.Count; i++)
        {
            distances[i] = Vector2.Distance(Camera.main.WorldToScreenPoint(screenTargets[i].position), new Vector2(Screen.width / 2, Screen.height / 2));
        }

        float minDistance = Mathf.Min(distances);
        int index = 0;

        for (int i = 0; i < distances.Length; i++)
        {
            if (minDistance == distances[i])
                index = i;
        }

        return index;

    }

    void CloseToTarget() {
        anim.applyRootMotion = false;
        transform.DOMove(target.position - Vector3.Normalize((target.position - transform.position)), .1f).OnComplete(()=>anim.applyRootMotion = true);
    }

    public int getCurrentCombo() {
        if(currentState.IsName("Hit01")) {
            return 1;
        }else if(currentState.IsName("Hit02")) {
            return 2;
        }else if(currentState.IsName("Hit03")) {
            return 3;
        }else {
            return 0;
        }
    }

    public MovementInput GetMovementInput() {
        return input;
    }

    public Animator GetAnimator() {
        return anim;
    }

    public List<Transform> GetScreenTargets() {
        return screenTargets;
    }

    public Transform GetTarget() {
        if(target == null && !isLocked) {
            target = screenTargets[targetIndex()];
        }
        return target;
    }

    public void ShowSword(bool state) {
        foreach(MeshRenderer mesh in swordMeshArr) {
            mesh.enabled = state;
        }
    }

    public void ScreenImpulse(Vector3 velocity) {
        impulse.GenerateImpulse(velocity);
    }

    public void Landing() {
        input.canMove = false;
        anim.SetTrigger("Landing");
    }

    public void CompleteLanding() {
        input.canMove = true;
    }
}
