using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace SoulBreeze {
    public class MeleeWeapon : MonoBehaviour
    {
        [System.Serializable]
        public class AttackPoint {
            public Transform point;
            public float radius;
        }

        private string weaponName {get;}       //the original name of the weapon
        public string nickname;         //the nickname of the weapon which can be rename by player
        public GameObject owner;        //design a type for the owner
        public int attackValue;         //the ATK value of the weapon
        public bool isInAttack;         //whether the weapon is in attack
        public LayerMask targetLayers;  //in which layer the weapon can hurt

        public GameObject[] swishEffects;
        public AttackPoint[] attackPoints = new AttackPoint[0];         //the valid attack points
        private Vector3[] previousPoints = null;                        //the positions of the attack points in the last frame
        private RaycastHit[] raycastHitCache = new RaycastHit[32];      //the cache of the hits that attack trail go through

        //public List<Enchantment> enchantments;        //the enchantments on the weapon

        public void InitializeBy() {
            
        }

        void OnEnable() {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void StartAttack() {
            isInAttack = true;
            
            previousPoints = new Vector3[attackPoints.Length];
            for(int i = 0; i < attackPoints.Length; i++) {
                Vector3 attackPointPos = attackPoints[i].point.position;
                previousPoints[i] = attackPointPos;
            }
        }

        public void EndAttack() {
            isInAttack = false;
            previousPoints = null;
        }

        // Update is called once per frame
        void FixedUpdate() {
            if(isInAttack) {
                //TODO: draw a ray to check what is scanned.
                for(int i = 0; i < attackPoints.Length; i++) {
                    Vector3 movePath = attackPoints[i].point.position - previousPoints[i];
                    Vector3 moveDirection = movePath.normalized;

                    Ray ray = new Ray(attackPoints[i].point.position, moveDirection);
                    int hits = Physics.SphereCastNonAlloc(ray, attackPoints[i].radius, 
                        raycastHitCache, movePath.magnitude, ~0, QueryTriggerInteraction.Ignore);
                    
                    for(int j = 0; j < hits; j++) {
                        Damageable d = raycastHitCache[j].collider.GetComponent<Damageable>();
                        if(d != null) ApplyDamageOn(d);
                    }

                    previousPoints[i] = attackPoints[i].point.position;
                }
            }
        }

        public void EquipOn(GameObject owner) {
            this.owner = owner;
        }

        //apply damage on the target hit.
        //return true if successfully apply a valid damage number on the target, return false otherwise.
        //parameter: target
        private bool ApplyDamageOn(Damageable entity) {
            if(entity.gameObject == owner) return true;

            Vector3 damageDirection = Vector3.Normalize(owner.transform.position - 
                                                            entity.gameObject.transform.position);
            Damageable.DamageDataPack damageData = new Damageable.DamageDataPack(owner, attackValue, damageDirection, owner.transform);
            return entity.ReceiveDamageData(damageData);
        }
    }
}
