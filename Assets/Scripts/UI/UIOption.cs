﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class UIOption : MonoBehaviour, IGameUI {

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;


    public GameObject GetGameObject() {
        return gameObject;
    }

    public void TransitionIn() {
        
    }

    public void TransitionOut() {
        
    }

    void Start() {
        
    }

    void Update() {
        
    }

    public void OnMusicValueChanged() {
        GameManager.Instance().GetAudioManager().SetBGMVolume(musicSlider.value);
    }

    public void OnSoundValueChanged() {
        GameManager.Instance().GetAudioManager().SetSEVolume(soundSlider.value);
    }

    public void OnBackButtonPressed() {
        GameManager.Instance().GetUIManager().ReturnBack();
    }
}
