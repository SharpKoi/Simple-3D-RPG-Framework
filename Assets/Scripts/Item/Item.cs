using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public abstract class Item {
        public enum ItemType {
            COMSUMABLE,
            EQUIPMENT,
            QUEST
        }

        public string name;
        public string nickname;
        public ItemType type;
        public string description;
        public int amount;
        public int maxStackAmount;
        public bool isAbandonable;

        public CompoundTag metadata;

        public override bool Equals(object obj) {
            if(obj is Item) {
                Item target = obj as Item;
                if(this.name == target.name && this.nickname == target.nickname && 
                    this.type == target.type && this.description == target.description &&
                    this.isAbandonable == target.isAbandonable /*&&
                    CompoundTag Comparation*/) {
                        return true;
                }
            }

            return false;
        }
     }
}
