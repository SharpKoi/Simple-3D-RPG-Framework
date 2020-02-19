using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace SoulBreeze {
    public class UIManager : MonoBehaviour{

        private Stack<IGameUI> ui_Stack;

        void Start() {
            ui_Stack = new Stack<IGameUI>();
        }

        void Update()
        {
            
        }

        public void SwitchUI(IGameUI ui) {
            if(ui_Stack.Count > 0) DisableUI(ui_Stack.Peek());
            ui_Stack.Push(ui);
            //TODO: play audio(pop up)
            //GameManager.Instance().GetAudioManager().PlaySE(AudioManager.SEPath() + "menu-popup");
            EnableUI(ui);
        }

        public void ReturnBack() {
            if(ui_Stack.Count == 0) {
                Debug.Log("The current UI stack is of size: 0\n Nothing can return back.");
                return;
            }

            IGameUI topUI = ui_Stack.Pop();
            DisableUI(topUI);

            /***VERSION: DOTween***/
            // topUI.TransitionOut().OnComplete(()=>DisableUI(topUI));
            
            //TODO: play audio(disappear)
            if(ui_Stack.Count > 0) {
                IGameUI lastUI = ui_Stack.Peek();
                EnableUI(lastUI);
            }else {
                GameManager.Instance().GetMovementInput().canMove = true;
            }
        }

        public void CloseAllUI() {
            IGameUI topUI = ui_Stack.Peek();
            /***VERSION: DOTween***/
            // topUI.TransitionOut().OnComplete(()=>DisableUI(topUI));

            DisableUI(topUI);

            ui_Stack.Clear();
            GameManager.Instance().GetMovementInput().canMove = true;
        }

        public void DisableUI(IGameUI ui) {
            /***VERSION: Standard***/
            ui.TransitionOut();
            
            /***VERSION: DOTween***/
            // ui.GetGameObject().SetActive(false);
        }

        public void EnableUI(IGameUI ui) {
            /***VERSION: Standard***/
            ui.TransitionIn();
            
            /***VERSION: DOTween***/
            // ui.GetGameObject().SetActive(true);
        }

        public bool IsEmpty() {
            return ui_Stack.Count == 0;
        }
    }
}
