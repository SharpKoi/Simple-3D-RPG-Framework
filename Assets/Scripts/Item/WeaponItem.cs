using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class WeaponItem : Item {

        public string objectPreferencePath;

        public bool isEquipable;
        public int atk;

        public WeaponItem(string name, ItemType type, int iconID, string nickname, string description)
         : base(name, type, iconID, nickname, description){}

        public bool TryEquipeOn() {

            return true;
        }

        public void OnEquipe() {
            
        }

        public override string GetName() {
            return name;
        }
    }
}
