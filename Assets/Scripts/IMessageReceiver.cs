using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulBreeze {

    namespace MessageSystem {
        public enum MessageType {
            DAMAGED,
            DEAD,
            RESPAWN
        }

        public interface IMessageReceiver {
            void OnMessageReceive(MessageType type, object sender, object msg);
        }
    }
    
}
