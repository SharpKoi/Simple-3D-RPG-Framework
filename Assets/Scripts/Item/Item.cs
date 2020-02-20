using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoulBreeze {
    public abstract class Item {
        public enum ItemType {
            COMSUMABLE,
            EQUIPMENT,
            QUEST
        }

        public readonly int iconID; 
        protected string name;
        public string nickname;
        public ItemType type;
        public string description;
        public int amount;
        public int maxStackAmount;
        public bool isAbandonable;

        public static Dictionary<int, string> iconDictionary;

        //Item icon id
        public readonly static int ICON_SWORD_1H = 1;
        public readonly static int ICON_SWORD_2H = 2;
        public readonly static int ICON_STAFF = 3;
        public readonly static int ICON_SHIELD = 4;
        public readonly static int ICON_POTION = 5;
        public readonly static int ICON_SCROLL = 6;
        public readonly static int ICON_BOOK = 7;

        public readonly static string ICON_FILE_PATH = "UI/ItemIcon/";

        public CompoundTag metadata;

        public Item(string name, ItemType type, int iconID, string nickname, string description){
            this.name = name;
            this.type = type;
            this.iconID = iconID;
            this.nickname = nickname;
            this.description = description;
        }

        public static void Init() {
            iconDictionary = new Dictionary<int, string>();
            iconDictionary.Add(ICON_SWORD_1H, "");
            iconDictionary.Add(ICON_SWORD_2H, "");
            iconDictionary.Add(ICON_STAFF, "T_8_staff_");
            iconDictionary.Add(ICON_SHIELD, "");
            iconDictionary.Add(ICON_POTION, "");
            iconDictionary.Add(ICON_SCROLL, "");
        }

        public abstract string GetName();

        public Sprite GetIcon() {
            string iconName = null;
            iconDictionary.TryGetValue(iconID, out iconName);
            return Resources.Load(ICON_FILE_PATH + iconName) as Sprite;
        }

        public string GetTypeName() {
            switch(type) {
                case ItemType.COMSUMABLE:
                    return "消費アイテム";
                case ItemType.EQUIPMENT:
                    return "装備アイテム";
                case ItemType.QUEST:
                    return "任務アイテム";
                default: return "";
            }
        }

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
