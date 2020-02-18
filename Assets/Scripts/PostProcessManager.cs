using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

namespace SoulBreeze {
    public class PostProcessManager : MonoBehaviour {

        public PostProcessProfile screenBlurProfile;

        // Start is called before the first frame update
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {
            
        }

        public void ScreenBlur(bool on) {
            DepthOfField blur = null;
            screenBlurProfile.TryGetSettings(out blur);
            if(on) {
                DOTween.To(()=>blur.aperture.value, x=>blur.aperture.value = x, 6, 1f);
            }else {
                DOTween.To(()=>blur.aperture.value, x=>blur.aperture.value = x, 15, 1f);
            }
        }

        void OnApplicationQuit() {
            Camera.main.GetComponent<PostProcessVolume>().profile = null;
        }
    }
}
