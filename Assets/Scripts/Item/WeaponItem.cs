using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SoulBreeze {
    public class WeaponItem : Item {

        private static List<WeaponItem> registeredWeapons;

        public readonly static int ID_ELLEN_STAFF = 0;
        //TODO: more id for weapons

        public static void RegisterAll() {
            if(registeredWeapons == null) {
                registeredWeapons = new List<WeaponItem>(1024);
            }
            registeredWeapons.Add(
                new WeaponItem("艾倫的手杖", ItemType.EQUIPMENT, ICON_STAFF, "艾倫的手杖", "泛著藍光的手杖。", 
                    "EllenStaff", 10));
            //TODO: register more weapon.
        }

        public static WeaponItem GetWeaponItem(int weaponID) {
            return registeredWeapons[weaponID];
        }

        public string objectAddress;
        private GameObject weaponObject = null;
        private bool isLoaded = false;

        private bool isEquipable = true;
        public int atk;

        public WeaponItem(string name, ItemType type, int iconID, string nickname, string description)
         : this(name, type, iconID, nickname, description, name, 0) {}

        public WeaponItem(string name, ItemType type, int iconID, string nickname, string description, 
            string objPrefPath, int atk)
         : this(name, type, iconID, nickname, description, objPrefPath, atk, 1) {}

        public WeaponItem(string name, ItemType type, int iconID, string nickname, string description, 
            string objPrefPath, int atk, int amount)
         : this(name, type, iconID, nickname, description, objPrefPath, atk, amount, 99) {}

        public WeaponItem(string name, ItemType type, int iconID, string nickname, string description, 
            string objPrefPath, int atk, int amount, int maxStackSize)
         : base(name, type, iconID, nickname, description) {
                this.objectAddress = objPrefPath;
                LoadWeaponObject();
                this.atk = atk;
                this.amount = amount;
                this.maxStackAmount = maxStackSize;
        }

        public void LoadWeaponObject() {
            if(weaponObject == null) {
                // AsyncOperationHandle<GameObject> res;
                Addressables.LoadAssetAsync<GameObject>(objectAddress).Completed += 
                    (res=> 
                    {
                        weaponObject = res.Result;
                        isLoaded = true;
                        Debug.Log("Weapon loaded done!\n" + weaponObject.name);
                    });
            }
        }

        public GameObject GetWeaponObject() {
            return weaponObject;
        }

        public bool IsEquipable() {
            return isEquipable && isLoaded && weaponObject != null;
        }

        public override void OnUse(InventoryHolder user) {
            Equipable equipable = user.GetComponent<Equipable>();
            if(equipable != null) {
                equipable.TryEquipe(this);
            }
        }

        public override string GetName() {
            return name;
        }
    }
}
