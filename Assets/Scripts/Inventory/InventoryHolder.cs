using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class InventoryHolder : MonoBehaviour
    {
        public List<Inventory> inventories;

        void Awake() {
            inventories = new List<Inventory>();
        }

        // Start is called before the first frame update
        void Start() {
            inventories.Add(new WeaponInventory(100));
            inventories[0].AddItem(WeaponItem.GetWeaponItem(WeaponItem.ID_ELLEN_STAFF));
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

