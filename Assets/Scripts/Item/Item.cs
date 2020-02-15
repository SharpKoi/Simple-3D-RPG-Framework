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
        public ItemType type;
        public string description;
        public bool isAbandonable;

        public CompoundTag metadata;
    }
}
