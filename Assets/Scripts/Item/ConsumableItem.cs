﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class ConsumableItem : Item
    {
        public ConsumableItem(string name, ItemType type, int iconID, string nickname, string description)
         : base(name, type, iconID, nickname, description) {}

        public override string GetName() {
            return name;
        }

        public override void OnUse(InventoryHolder user) {

        }
    }
}
