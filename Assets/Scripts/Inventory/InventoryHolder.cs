using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class InventoryHolder : MonoBehaviour
    {
        public List<Inventory> inventories;


        // Start is called before the first frame update
        void Start() {
            inventories = new List<Inventory>();
            inventories.Add(new WeaponInventory(100));
            inventories[0].AddItem(new ItemEllenStaff());
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

