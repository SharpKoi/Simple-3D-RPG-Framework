using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    ///<summary>
    ///@Form: MonoBehaviour
    ///@Description: A class with three basic inv. dealing with the new item obtaining and item classifing
    ///@TODO: More generally such that the API users can design a custom InventoryHolder with the custom classify function.
    ///       Maybe apply the Labels System?
    ///</sumary>
    public class InventoryHolder : MonoBehaviour {

        private WeaponInventory _weaponInv;
        public WeaponInventory weaponInventory 
        {
            get {return _weaponInv;}
        }

        private ArmorInventory _armorInv;
        public ArmorInventory armorInventory
        {
            get {return _armorInv;}
        }

        private ConsumableInventory _consumableInv;
        public ConsumableInventory consumableInventory
        {
            get {return _consumableInv;}
        }

        private QuestInventory _questInv;
        public QuestInventory questInventory
        {
            get {return _questInv;}
        }

        // public List<Inventory> inventories = new List<Inventory>();

        void Awake() {
            // inventories = new List<Inventory>();
            _weaponInv = new WeaponInventory(100);
            _armorInv = new ArmorInventory(100);
            _consumableInv = new ConsumableInventory(100);
            _questInv = new QuestInventory(100);
        }

        // Start is called before the first frame update
        void Start() {
            _weaponInv.AddItem(WeaponItem.GetWeaponItem(WeaponItem.ID_ELLEN_STAFF));
        }

        public bool CanAccept(Item item) {
            if(item is WeaponItem) {
                return _weaponInv.CanAccept(item);
            }else if(item is ConsumableItem) {
                return _consumableInv.CanAccept(item);
            }else if(item is ArmorItem) {
                return _armorInv.CanAccept(item);
            }
            return false; 
        }

        public void OnGetRawItem(Item item) {
            Classify(item);
        }

        private void Classify(Item item) {
            if(item is WeaponItem && _weaponInv.CanAccept(item)) {
                _weaponInv.AddItem(item);
            }else if(item is ConsumableItem && _consumableInv.CanAccept(item)) {
                _consumableInv.AddItem(item);
            }else if(item is ArmorItem && _armorInv.CanAccept(item)) {
                _armorInv.AddItem(item);
            }
        }

        // Update is called once per frame
        void Update() {
            
        }
    }
}

