using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class Equipable : MonoBehaviour {
        //將裝備物品穿戴在身上各個部位
        //雙手 雙腳 上身 下身 頭部

        public bool TryEquipe(Item equipment) {
            if(equipment.type != Item.ItemType.EQUIPMENT) return false;

            return true;
        }
    }
}
