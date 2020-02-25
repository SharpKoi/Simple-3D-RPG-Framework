using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public abstract class Inventory {

        private List<Item> itemList = new List<Item>();
        public int maxSlots;
        public InventoryHolder owner;

        public Inventory(int maxSlots) {
            this.maxSlots = maxSlots;
        }

        public List<Item> GetItems() {
            return itemList;
        }

        public Item GetItem(int index) {
            return itemList[index];
        }

        public bool CanAccept(Item item) {
            if(itemList.Contains(item)) {
                for(int i = 0; i < itemList.Count; i++) {
                    Item target = itemList[i];
                    if(target.Equals(item) && (target.amount < target.maxStackAmount)) {
                        int remain = target.maxStackAmount - target.amount;
                        item.amount -= Mathf.Min(item.amount, remain);
                        if(item.amount <= 0) return true;
                    }
                }
            }

            int neededSlotsNum = item.amount / item.maxStackAmount;

            if(maxSlots - itemList.Count > neededSlotsNum) {
                return true;
            }else {
                return false;
            }
        }

        public void AddItem(Item item) {
            if(itemList.Contains(item)) {
                for(int i = 0; i < itemList.Count; i++) {
                    Item target = itemList[i];
                    if(target.Equals(item) && (target.amount < target.maxStackAmount)) {
                        int moveCount = Mathf.Min(item.amount, target.maxStackAmount - target.amount);
                        item.amount -= moveCount;
                        target.amount += moveCount;
                        if(item.amount <= 0) return;
                    }
                }
            }

            while(item.amount > 0) {
                Item itemStack = item.DeepCopyByExpressionTree();
                itemStack.amount = Mathf.Min(item.amount, item.maxStackAmount);
                item.amount -= itemStack.amount;
                itemList.Add(item);
            }
        }
    }
}
