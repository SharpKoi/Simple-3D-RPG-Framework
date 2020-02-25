using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;

namespace SoulBreeze {
    public class UIInventory : MonoBehaviour, IGameUI
    {
        [SerializeField] private GameObject slotPref;   //the prefab of item slot(UI_ItemSlot->frame->back, item)

        [SerializeField] private GameObject u_content;                   //which slots be drew under
        private List<ItemSlot> u_itemSlots;        //store the slots on ui

        [SerializeField] private InventoryHolder viewer;
        private Inventory currentInventory;
        private int slotAmount;                         //the amount of item slots
        public float slotsDist;                         //the distance of any two slots (120)
        public int rowLength;                           //how much slots in one row (4)

        private Camera cam;
        private PostProcessManager postProcessManager;

        [SerializeField] private Image panel_bg = null;
        [SerializeField] private Image magic_ring_img = null;
        [SerializeField] private Image goddess_img = null;

        [Header("Side Option Tags")]
        [SerializeField] private GameObject sideOptionTags;

        [Header("Item Preview")]
        [SerializeField] private GameObject itemPreview;
        [SerializeField] private TMP_Text t_itemName;
        [SerializeField] private TMP_Text t_itemType;
        [SerializeField] private TMP_Text t_itemDetail;

        [Header("Item Content")]
        [SerializeField] private GameObject itemContent;

        public UIOption optionUI;

        void Awake() {
            u_itemSlots = new List<ItemSlot>();
        }

        void OnEnable() {
            cam = Camera.main;
            postProcessManager = GameManager.Instance().GetPostProcessManager();
        }

        // Start is called before the first frame update
        void Start() {
            InitializeGUI();
        }

        // Update is called once per frame
        void Update() {
            
        }

        public void InitializeGUI() {
            //set the basic slot requirements
            int index = -1;
            int row_num = 100/4;
            for(int i = 0; i < row_num; i++) {
                for(int j = 0; j < rowLength; j++) {
                    index ++;
                    GameObject slot = GameObject.Instantiate(slotPref, u_content.transform);
                    Vector2 slotPos = new Vector2(slotsDist * j, - slotsDist * i);
                    slot.GetComponent<RectTransform>().anchoredPosition = slotPos;

                    ItemSlot itemSlot = slot.GetComponentInChildren<ItemSlot>();
                    itemSlot.index = index;
                    itemSlot.OnSelectEvent.AddListener(() => ShowItemPreview(index));
                    u_itemSlots.Add(itemSlot);
                }
            }
        }

        public void DrawItemSlotsFrom(Inventory inventory) {
            List<Item> itemList = inventory.GetItems();
            for(int i = 0; i < itemList.Count; i++) {
                u_itemSlots[i].SetItemIcon(itemList[i].GetIcon());
                u_itemSlots[i].SetState(SlotState.NORMAL);
            }
            currentInventory = inventory;
        }

        public void ShowItemPreview(int index) {
            Item item = currentInventory.GetItem(index);
            t_itemName.SetText(item.GetName());
            t_itemType.SetText(item.GetTypeName());
            t_itemDetail.SetText(item.description);
        }

        public GameObject GetGameObject() {
            return gameObject;
        }

        public void TransitionIn() {
            gameObject.SetActive(true);
            PostProcessVolume volume = cam.GetComponent<PostProcessVolume>();
            volume.profile = postProcessManager.screenBlurProfile;
            StartCoroutine(TransitionInEffect());
        }

        IEnumerator TransitionInEffect() {
            cam.gameObject.GetComponent<PostProcessVolume>().weight = 1;
            postProcessManager.ScreenBlur(true);
            
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

            postProcessManager.ScreenBlur(false);
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
