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
        bg.DOFade((255.00f/255.00f), 0.6f);
    }

    public TweenerCore<Color, Color, ColorOptions> TransitionOut() {
        return bg.DOFade(0, 0.6f);
    }

    public void OnOptionButtonClicked() {
        GameManager.Instance().GetUIManager().SwitchUI(optionUI);
    }
}
