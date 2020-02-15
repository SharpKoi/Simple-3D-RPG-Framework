using UnityEngine;

namespace SoulBreeze {
    public partial class Damageable : MonoBehaviour {
        public class DamageDataPack
        {
            public GameObject damager;
            public int value;
            public Vector3 direction;
            public Transform damageSource;
            //public bool throwing;

            public bool stopCamera;

            public DamageDataPack(GameObject damager, int value, Vector3 direction, Transform damageSource) {
                this.damager = damager;
                this.value = value;
                this.direction = direction;
                this.damageSource = damageSource;
            }
        }

        public class RespwanDataPack {
            Vector3 position;

            public RespwanDataPack(Vector3 position) {
                this.position = position;
            }
        }
    }
}
