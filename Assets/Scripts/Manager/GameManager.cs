using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance() {
        return instance;
    }

    bool cursorLocked = true;

    [Header("Manager")]
    [SerializeField] private AudioManager audioManager = null;
    [SerializeField] private UIManager uiManager = null;
    [SerializeField] private MovementInput input = null;

    [Header("UI")]
    public MonoBehaviour menuUI;

    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        LockCursor();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(cursorLocked == true) FreeCursor();
        }

        if(Input.GetMouseButtonDown(0)) {
            if(cursorLocked == false && GameManager.Instance().GetUIManager().IsEmpty()) LockCursor();
        }

        if(Input.GetKeyDown(KeyCode.E)) {
            if(uiManager.IsEmpty()) {
                uiManager.SwitchUI(menuUI as IGameUI);
                FreeCursor();
                input.canMove = false;
            }else {
                uiManager.CloseAllUI();
                LockCursor();
                input.canMove = true;
            }
        }
    }

    public void LockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLocked = true;
    }

    public void FreeCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursorLocked = false;
    }

    public AudioManager GetAudioManager() {
        return audioManager;
    }

    public UIManager GetUIManager() {
        return uiManager;
    }
}
