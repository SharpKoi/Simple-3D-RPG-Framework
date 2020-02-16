using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace SoulBreeze {
    public class ItemSlot : MonoBehaviour, ISelectHandler, IDeselectHandler {
        public int index;

        [SerializeField] private Image bg;
        [SerializeField] private Image itemIcon;
        public Color itemSelectedColor;
        public Color itemDeselectedColor;
        public Color bgSelectedColor;
        public Color bgDeselectedColor;

        void Update() {
            
        }

        public void OnDeselect(BaseEventData eventData)
        {
            // itemIcon.CrossFadeColor(itemDeselectedColor, 0.1f, false, true);
            // itemIcon.color = itemDeselectedColor;
            itemIcon.DOColor(itemDeselectedColor, 0.2f);
            // bg.CrossFadeColor(bgDeselectedColor, 0.1f, false, true);
            // bg.color = bgDeselectedColor;
            bg.DOColor(bgDeselectedColor, 0.2f);
        }

        public void OnSelect(BaseEventData eventData)
        {
            // itemIcon.CrossFadeColor(itemSelectedColor, 0.1f, false, true);
            // itemIcon.color = itemSelectedColor;
            itemIcon.DOColor(itemSelectedColor, 0.2f);
            // bg.CrossFadeColor(bgSelectedColor, 0.1f, false, true);
            // bg.color = bgSelectedColor;
            bg.DOColor(bgSelectedColor, 0.2f);
        }
    }
}
