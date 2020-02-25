using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class Equipable : MonoBehaviour {

        private Transform equipPosition;
        //將裝備物品穿戴在身上各個部位
        //雙手 雙腳 上身 下身 頭部

        private GameObject _weapon;
        public GameObject weapon
        {
            get {return _weapon;}
        }

        public bool TryEquipe(Item equipment) {
            return TryEquipe(equipment, Vector3.zero);
        }

        public bool TryEquipe(Item equipment, Vector3 offset) {
            if(equipment.type != Item.ItemType.EQUIPMENT) return false;

            if(equipment is WeaponItem) {
                WeaponItem weaponItem = equipment as WeaponItem;
                GameObject weaponObj = weaponItem.GetWeaponObject();
                _weapon = GameObject.Instantiate(weaponObj, equipPosition);
                _weapon.transform.position = offset;
            }
            return true;
        }
    }
}
