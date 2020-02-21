using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class ComsumableItem : Item
    {
        public ComsumableItem(string name, ItemType type, int iconID, string nickname, string description)
         : base(name, type, iconID, nickname, description) {}

        public override string GetName() {
            return name;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
