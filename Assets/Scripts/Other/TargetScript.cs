using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public float visibleDist = 10;
    private bool visible;
    public Material getHitMat = null;
    private List<Material> originMatList = new List<Material>();
    CombatController combat = null;

    public bool isDead = false;

    void Start()
    {
        combat = FindObjectOfType<CombatController>();
        SkinnedMeshRenderer[] targetSkinMeshList = transform.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach(SkinnedMeshRenderer smr in targetSkinMeshList) {
            originMatList.Add(smr.material);
        }
    }

    void Update() {
        if (!combat.GetScreenTargets().Contains(transform)) {
            if(Vector3.Magnitude(transform.parent.position - combat.transform.position) <= visibleDist
                && isDead == false && visible) {
                    combat.GetScreenTargets().Add(transform);
            }
        }else {
            
        }
    }

    private void OnBecameVisible()
    {
        visible = true;
    }

    private void OnBecameInvisible()
    {
        if(combat.GetScreenTargets().Contains(transform)) {
            combat.GetScreenTargets().Remove(transform);
        }
        visible = false;
    }

    public void GetHit(int damage) {
        GetComponentInParent<Animator>().SetTrigger("hit");
        GetComponentInParent<Enemy>().hp -= damage;
        // SkinnedMeshRenderer[] targetSkinMeshList = transform.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
        // foreach(SkinnedMeshRenderer smr in targetSkinMeshList) {
        //     smr.material = getHitMat;
        // }
        if(GetComponentInParent<Enemy>().hp <= 0) {
            isDead = true;
            GetComponentInParent<Animator>().SetBool("death", true);
        }
        //StartCoroutine(ReMatRender());
    }

    IEnumerator ReMatRender() {
        yield return new WaitForSeconds(.6f);
        SkinnedMeshRenderer[] targetSkinMeshList = transform.parent.GetComponentsInChildren<SkinnedMeshRenderer>();
        for(int i = 0; i < targetSkinMeshList.Length; i++) {
            targetSkinMeshList[i].material = originMatList[i];
        }
    }

    public float GetDistanceFromPlayer(CombatController player) {
        return Vector3.Magnitude(Vector3.ProjectOnPlane(transform.position - player.transform.position, Vector3.up));
    }
}
