using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

namespace SoulBreeze {
    public class UIInventory : MonoBehaviour, IGameUI
    {
        [SerializeField] private GameObject slotPref;   //the prefab of item slot(UI_ItemSlot->frame->back, item)

        private GameObject u_content;                   //which slots be drew under
        private List<RectTransform> u_itemSlots;        //store the slots on ui
        private int slotAmount;                         //the amount of item slots
        public float slotsDist;                         //the distance of any two slots (120)
        public int rowLength;                           //how much slots in one row (4)

        private Camera cam;
        [SerializeField] private PostProcessProfile screenBlurProfile;
        [SerializeField] private Image panel_bg;
        [SerializeField] private Image magic_ring_img;
        [SerializeField] private Image goddess_img;
        [SerializeField] private GameObject sideOptionTags;
        [SerializeField] private GameObject itemPreview;
        [SerializeField] private GameObject itemContent;

        public UIOption optionUI;

        void OnEnable() {
            cam = Camera.main;
        }

        // Start is called before the first frame update
        void Start() {
            //u_content
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void DrawSlotsFrom(Inventory inventory) {
            foreach(Item item in inventory.GetItems()) {
                
            }
        }

        public void SelectSlotAt(int index) {
            
        }

        public GameObject GetGameObject() {
            return gameObject;
        }

        public void TransitionIn() {
            gameObject.SetActive(true);
            PostProcessVolume volume = cam.GetComponent<PostProcessVolume>();
            volume.profile = screenBlurProfile;
            StartCoroutine(TransitionInEffect());
        }

        IEnumerator TransitionInEffect() {
            cam.gameObject.GetComponent<PostProcessVolume>().weight = 1;
            DepthOfField blur = null;
            screenBlurProfile.TryGetSettings(out blur);
            DOTween.To(()=>blur.aperture.value, x=>blur.aperture.value = x, 6, 1f);
            
            panel_bg.DOFade(150.00f/255.00f, 1f);

            magic_ring_img.DOFade(50.00f/255.00f, 1f);
            magic_ring_img.transform.DORotate(Vector3.zero, 1f);

            goddess_img.DOFade(50.00f/255.00f, 1f);
            goddess_img.rectTransform.DOAnchorPosX(928f, 1f);

            sideOptionTags.GetComponent<RectTransform>().DOAnchorPosX(0, 1f);
            itemPreview.GetComponent<RectTransform>().DOAnchorPosX(-660f, 1f);
            itemContent.GetComponent<RectTransform>().DOAnchorPosX(0, 1f);

            yield return new WaitForSeconds(1f);
            Debug.Log("Transition in done.");
        }

        public void TransitionOut() {
            panel_bg.color = new Color(panel_bg.color.r, panel_bg.color.g, panel_bg.color.b, 0);

            magic_ring_img.DOFade(0, 1f);
            magic_ring_img.transform.DORotate(new Vector3(0, 0, 135), 1f);

            goddess_img.DOFade(0, 1f);
            goddess_img.rectTransform.DOAnchorPosX(1222f, 1f);

            sideOptionTags.GetComponent<RectTransform>().DOAnchorPosX(-560f, 1f);
            itemPreview.GetComponent<RectTransform>().DOAnchorPosX(700f, 1f);
            itemContent.GetComponent<RectTransform>().DOAnchorPosX(1360, 1f);

            DepthOfField blur = null;
            screenBlurProfile.TryGetSettings(out blur);
            DOTween.To(()=>blur.aperture.value, x=>blur.aperture.value = x, 15, 1f);
            cam.GetComponent<PostProcessVolume>().weight = 0;
            gameObject.SetActive(false);
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
}
