using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class Equipable : MonoBehaviour {

        private Transform equipPosition;
        //將裝備物品穿戴在身上各個部位
        //雙手 雙腳 上身 下身 頭部

        public bool TryEquipe(Item equipment) {
            return TryEquipe(equipment, Vector3.zero);
        }

        public bool TryEquipe(Item equipment, Vector3 offset) {
            if(equipment.type != Item.ItemType.EQUIPMENT) return false;

            if(equipment is WeaponItem) {
                WeaponItem weapon = equipment as WeaponItem;
                GameObject _weaponObj = weapon.GetWeaponObject();
                GameObject weaponObject = GameObject.Instantiate(_weaponObj, equipPosition);
                weaponObject.transform.position = offset;
            }
            return true;
        }
    }
}
