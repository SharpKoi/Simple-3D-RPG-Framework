using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using TMPro;

namespace SoulBreeze {

    public enum SlotState {
        NORMAL, 
        SELECTED,
        EMPTY
    }

    public class ItemSlot : MonoBehaviour, ISelectHandler, IDeselectHandler {
        public int index;
        public SlotState state = SlotState.EMPTY;
        public bool isSelected;
        public bool isSelectable;

        [SerializeField] private Image img_bg;
        [SerializeField] private Image img_itemIcon;
        [SerializeField] private TMP_Text t_amount;

        public UnityEvent OnSelectEvent, OnDeselectEvent;


        [Header("Normal State")]
        public Color itemNormalColor;
        public Color bgNormalColor;
        
        [Header("Selected State")]
        public Color bgSelectedColor;
        public Color itemSelectedColor;
        
        [Header("Empty State")]
        public Color itemEmptyColor;
        public Color bgEmptyColor;

        public void SetItemIcon(Sprite icon) {
            img_itemIcon.sprite = icon;
        }

        public void Test() {
            Debug.Log("Test");
        }

        public void OnDeselect(BaseEventData eventData) {
            if(state == SlotState.EMPTY) return;

            OnDeselectEvent.Invoke();
            img_itemIcon.DOColor(itemNormalColor, 0.2f);
            img_bg.DOColor(bgNormalColor, 0.2f);
            isSelected = false;
        }

        public void OnSelect(BaseEventData eventData) {
            if(state == SlotState.EMPTY) return;

            OnSelectEvent.Invoke();
            img_itemIcon.DOColor(itemSelectedColor, 0.2f);
            img_bg.DOColor(bgSelectedColor, 0.2f);
            isSelected = true;
        }

        public void SetState(SlotState state) {
            switch(state) {
                case SlotState.EMPTY:
                    state = SlotState.EMPTY;
                    img_bg.color = bgEmptyColor;
                    img_itemIcon.gameObject.SetActive(false);
                    t_amount.gameObject.SetActive(false);
                    break;
                case SlotState.NORMAL:
                    state = SlotState.NORMAL;
                    img_bg.color = bgNormalColor;
                    img_itemIcon.gameObject.SetActive(true);
                    img_itemIcon.color = itemNormalColor;
                    t_amount.gameObject.SetActive(true);
                    break;
                case SlotState.SELECTED:
                    state = SlotState.SELECTED;
                    img_bg.color = bgSelectedColor;
                    img_itemIcon.color = itemSelectedColor;
                    break;
            }
        }
    }
}
