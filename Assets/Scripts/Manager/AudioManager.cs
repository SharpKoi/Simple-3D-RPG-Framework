using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource bgmPlayer;
    public AudioSource sePlayer;

    public float bgm_vol;
    public float se_vol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static string BGMPath() {
        return "Audios/bgm/";
    }

    public static string SEPath() {
        return "Audios/se/";
    }

    public void PlaySE(string path) {
        AudioClip clip = Resources.Load<AudioClip>(path);
        sePlayer.PlayOneShot(clip);
    }

    public void PlayBGM(string path) {
        AudioClip clip = Resources.Load<AudioClip>(path);
        bgmPlayer.clip = clip;
        bgmPlayer.Play();
    }

    public void SetBGMVolume(float value) {
        bgm_vol = Mathf.Clamp01(value);
        bgmPlayer.volume = bgm_vol;
    }

    public void SetSEVolume(float value) {
        se_vol = Mathf.Clamp01(value);
        sePlayer.volume = se_vol;
    }
}
