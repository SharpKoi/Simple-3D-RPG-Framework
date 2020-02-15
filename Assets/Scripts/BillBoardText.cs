using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class BillBoardText : MonoBehaviour {

    private Transform camPos = null;

    // Start is called before the first frame update
    void Start() {
        camPos = Camera.main.transform;
    }

    // Update is called once per frame
    void Update() {
        Billboard();
    }

    void Billboard() {
        transform.LookAt(camPos);
        transform.Rotate(0, 180, 0);
    }
}
