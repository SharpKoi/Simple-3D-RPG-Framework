using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class UIMenu : MonoBehaviour, IGameUI
{
    [SerializeField] private Image bg = null;
    // [SerializeField] private Button b_ability = null;
    // [SerializeField] private Button b_equip = null;
    // [SerializeField] private Button b_backpack = null;
    // [SerializeField] private Button b_option = null;
    // [SerializeField] private Button b_quit = null;

    [SerializeField] private UIOption optionUI = null;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    public void TransitionIn() {
        
    }

    public void TransitionOut() {
        
    }

    public void OnOptionButtonClicked() {
        GameManager.Instance().GetUIManager().SwitchUI(optionUI);
    }
}
