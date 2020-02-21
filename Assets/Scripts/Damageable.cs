using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SoulBreeze.MessageSystem;

namespace SoulBreeze {
    public partial class Damageable : MonoBehaviour {
        public int maxHealthPoints;             //the max hp
        public int currentHealthPoints;         //the current hp
         public float invulnerabiltyTime;        //the invulnerable time
        public bool isInvulnerable;             //whether the game object is invulnerable
        public Vector3 spawnPosition;           //where the game object spawn at

        public UnityEvent OnDamaged, OnDeath, OnHitInInvulnerable, OnBecomeVulnerable, OnRespwan;
        private float damageInterval;           //the time since last damaged
        System.Action schedule;                 //to avoid race condition when objects kill each other at the same time; 
 
        public List<IMessageReceiver> onDamageMessageReceivers;

        public List<IMessageReceiver> onRespawnMessageReceivers;

        void OnEnable() {
            onDamageMessageReceivers = new List<IMessageReceiver>();
            onRespawnMessageReceivers = new List<IMessageReceiver>();
        }

        // Start is called before the first frame update
        void Start() {
            if(spawnPosition == null) spawnPosition = gameObject.transform.position;
        }

        // Update is called once per frame
        void Update() {
            if(isInvulnerable) {
                damageInterval += Time.deltaTime;
                if(damageInterval >= invulnerabiltyTime) {
                    isInvulnerable = false;
                    OnBecomeVulnerable.Invoke();
                }
            }
        }

        public bool ReceiveDamageData(DamageDataPack data) {
            if(isInvulnerable) {
                //if there's a damager hit this then cancel the damage.
                if(data.damager != null){
                    OnHitInInvulnerable.Invoke();
                    return false;
                }
            }
            
            if(IsDead()) return false;

            isInvulnerable = true;
            currentHealthPoints -= data.value;

            if(IsDead()) schedule += OnDeath.Invoke; else OnDamaged.Invoke();

            MessageType msgType = IsDead()? MessageType.DEAD : MessageType.DAMAGED;

            for(int i = 0; i < onDamageMessageReceivers.Count; i++) {
                onDamageMessageReceivers[i].OnMessageReceive(msgType, this, data);
            }

            return true;
        }

        public bool IsDead() {
            return currentHealthPoints <= 0;
        }

        public void Respawn() {
            
        }

        public void RespawnAt(Vector3 position) {
            currentHealthPoints = maxHealthPoints;
            OnRespwan.Invoke();
            
            for(int i = 0; i < onRespawnMessageReceivers.Count; i++) {
                RespwanDataPack respwanData = new RespwanDataPack(position);
                onRespawnMessageReceivers[i].OnMessageReceive(MessageType.RESPAWN, this, respwanData);
            }
        }
    }
}
