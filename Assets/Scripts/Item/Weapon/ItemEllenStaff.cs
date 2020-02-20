using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class ItemEllenStaff : WeaponItem {

        //TODO: name is fixed and is not changeable. The structure of item class need to be modified.
        public ItemEllenStaff(string name, ItemType type, int iconID, string nickname, string description)
         : base(name, type, iconID, nickname, description) {}

        public override string GetName() {
            return "艾倫的手杖";
        }

    }
}
