using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class WeaponItem : Item {
        public bool isEquipable;
        public int atk;

        public bool TryEquipe() {
            return true;
        }

        public void OnEquipe() {

        }
        
    }
}
