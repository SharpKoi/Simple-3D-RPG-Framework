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
            registeredWeapons[ID_ELLEN_STAFF] = 
                new WeaponItem("艾倫的手杖", ItemType.EQUIPMENT, ICON_STAFF, "艾倫的手杖", "泛著藍光的手杖。", 
                    "", 10);
            //TODO: register more weapon.
        }

        public static WeaponItem GetWeaponItem(int weaponID) {
            return registeredWeapons[weaponID];
        }

        public string objectAddress;
        private GameObject weaponObject;
        private bool isLoaded = false;

        private bool isEquipable;
        public int atk;

        public WeaponItem(string name, ItemType type, int iconID, string nickname, string description)
         : base(name, type, iconID, nickname, description){
             this.objectAddress = name;
             LoadWeaponObject();
        }

        public WeaponItem(string name, ItemType type, int iconID, string nickname, string description, 
            string objPrefPath, int atk)
         : base(name, type, iconID, nickname, description) {
                this.objectAddress = objPrefPath;
                LoadWeaponObject();
                this.atk = atk;
        }

        public void LoadWeaponObject() {
            if(weaponObject == null) {
                // AsyncOperationHandle<GameObject> res;
                Addressables.LoadAssetAsync<GameObject>(objectAddress).Completed += 
                    (res=> {weaponObject = res.Result; isLoaded = true;});
            }
        }

        public GameObject GetWeaponObject() {
            return weaponObject;
        }

        public bool IsEquipable() {
            return isEquipable && isLoaded && weaponObject != null;
        }

        // public bool TryEquipeOn() {

        //     return true;
        // }

        public void OnEquipe() {
            
        }

        public override string GetName() {
            return name;
        }
    }
}
