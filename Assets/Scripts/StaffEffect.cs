using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {
    public class StaffEffect : StateMachineBehaviour {
        public int effectIndex;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Equipable equipable = animator.GetComponent<Equipable>();

            GameObject swish = equipable.weapon.GetComponent<MeleeWeapon>().swishEffects[effectIndex];
            swish.SetActive(true);
            Animation animation = swish.GetComponent<Animation>();
            if(animation) animation.Play();
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            Equipable equipable = animator.GetComponent<Equipable>();

            GameObject swish = equipable.weapon.GetComponent<MeleeWeapon>().swishEffects[effectIndex];
            swish.SetActive(false);
        }
    }
}
