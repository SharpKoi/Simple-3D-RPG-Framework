using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryUI : MonoBehaviour, IGameUI
{
    [SerializeField] private GameObject slotPref;   //the prefab of item slot(UI_ItemSlot->frame->back, item)
    private GameObject u_content;                   //which slots be drew under
    private List<RectTransform> u_itemSlots;        //store the slots on ui
    public float slotsDist;                         //the distance of any two slots (120)
    public int rowLength;                           //how much slots in one row (4)
    public Button test;
    public UIOption optionUI;
    
    public void DrawSlots() {

    }

    void OnEnable() {

    }

    // Start is called before the first frame update
    void Start() {
        //u_content
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HighlightSlotAt(int index) {
         
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    public void TransitionIn() {
        
    }

    public void TransitionOut() {
        
    }

    public void OnAbilityButtonClicked() {

    }

    public void OnStatusButtonClicked() {

    }

    public void OnInventoryButtonClicked() {
        
    }

    public void OnOptionButtonClicked() {
        GameManager.Instance().GetUIManager().SwitchUI(optionUI);
    }

    public void OnQuitButtonClicked() {
        GameManager.Instance().GetUIManager().ReturnBack();
    }
}
