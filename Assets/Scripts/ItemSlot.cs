using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace SoulBreeze {

    // public enum SlotState {
    //     NORMAL, 
    //     SELECTED,
    //     EMPTY
    // }

    public class ItemSlot : MonoBehaviour, ISelectHandler, IDeselectHandler {
        public int index;
        public bool isSelected;
        public bool isSelectable;

        [SerializeField] private Image bg;
        [SerializeField] private Image itemIcon;

        [Header("Normal State")]
        public Color itemNormalColor;
        public Color bgNormalColor;
        
        [Header("Selected State")]
        public Color bgSelectedColor;
        public Color itemSelectedColor;
        
        [Header("Empty State")]
        public Color itemEmptyColor;
        public Color bgEmptyColor;

        void Update() {
            
        }

        public void OnDeselect(BaseEventData eventData) {
            itemIcon.DOColor(itemNormalColor, 0.2f);
            bg.DOColor(bgNormalColor, 0.2f);
            isSelected = false;
        }

        public void OnSelect(BaseEventData eventData) {
            itemIcon.DOColor(itemSelectedColor, 0.2f);
            bg.DOColor(bgSelectedColor, 0.2f);
            isSelected = true;
        }

        // public void SetState(SlotState state) {
        //     if(state == SlotState.EMPTY) {

        //     }
        // }
    }
}
