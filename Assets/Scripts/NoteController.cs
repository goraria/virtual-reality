using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NoteController : MonoBehaviour {
    [Header("Input")]
    [SerializeField] private KeyCode closeKey;

    //[Space(10)]
    //[SerializeField] private PersonMovement player;

    [Header("UI Text")]
    [SerializeField] private GameObject noteCanvas;
    // [SerializeField] private TMP_Text noteTextAreaUI;

    //[Space(10)]
    //[SerializeField]
    //[TextArea] private string noteText;

    [Space(10)]
    [SerializeField]
    private UnityEvent openEvent;

    private bool isOpen = false;

    public void ShowNote() {
        // noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);
        openEvent.Invoke();
        // DisablePlayer(true);
        isOpen = true;
    }

    void DisableNote() {
        noteCanvas.SetActive(false);
        // DisablePlayer(false);
        isOpen = false;
    }

    private void Update() {
        if (isOpen) {
            if (Input.GetKeyDown(closeKey)) {
                DisableNote();
            }
        }
    }
}
