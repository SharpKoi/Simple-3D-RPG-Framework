using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public abstract class Inventory {

        private List<Item> itemList = new List<Item>();
        public int maxSlots;
        public InventoryHolder owner;

        public List<Item> GetItems() {
            return itemList;
        }

        public Item GetItem(int index) {
            return itemList[index];
        }

        public bool AddItem(Item item) {
            if(itemList.Contains(item)) {
                Item targetSlot = itemList.Find(x=>x.Equals(item));
                if(targetSlot.amount < targetSlot.maxStackAmount) {
                    targetSlot.amount += 1;
                    return true;
                }
            }

            if(itemList.Count > maxSlots) return false;

            itemList.Add(item);
            return true;
        }
    }
}
