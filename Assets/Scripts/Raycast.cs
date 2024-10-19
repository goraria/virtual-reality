using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Raycast : MonoBehaviour {
    [Header("Rayvast Feature")]
    [SerializeField]
    private float rayLength = 5.0f;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;
    [SerializeField] private Canvas bookCanvas;
    private bool isBookOpen = false;  // Trạng thái mở sách
    private Camera _camera;


    private DoorController raycastedObj0;
    private DoorAltroller raycastedObj1;
    private FanSwitchController fanController;
    private LightSwitchController lightController;
    private NoteController _noteController;

    [Header("Pickup Settings")]
    [SerializeField]
    private Transform holdArea;

    private GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Physics Parameters")]
    [SerializeField]
    private float pickupRange = 5.0f;

    [SerializeField] private float pickupForce = 150.0f;

    [Header("Input Key")]
    [SerializeField]
    private KeyCode interactKey = KeyCode.Mouse0;
    [Header("Input Key")]
    [SerializeField]
    private KeyCode interactKey1 = KeyCode.Mouse1;
    [Header("Input Key")]
    [SerializeField]
    private KeyCode interactKey2 = KeyCode.Mouse2;

    [Header("Crosshair")]
    [SerializeField]
    private Image crosshair = null;

    private bool isCrosshairActive;
    private bool doOnce;
    private const string interactableTag0 = "InteractiveObject";
    private const string interactableTag1 = "InteractiveEntity";
    private const string switchTag = "FanSwitch";
    private const string lightTag = "LightSwitch";
    private const string interactableTag = "Untagged";

    void Start() {
        _camera = GetComponent<Camera>();
    }

    private void Update() {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        Debug.DrawRay(transform.position, fwd * rayLength, Color.magenta);

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask)) {
            if (hit.collider.CompareTag(interactableTag0)) {
                raycastedObj0 = hit.collider.gameObject.GetComponent<DoorController>();
                CrosshairChange(true);

                isCrosshairActive = true;
                doOnce = true;

                if (Input.GetKeyDown(interactKey)) {
                    raycastedObj0.PlayAnimation();
                }
            } else if (hit.collider.CompareTag(interactableTag1)) {
                raycastedObj1 = hit.collider.gameObject.GetComponent<DoorAltroller>();
                CrosshairChange(true);

                isCrosshairActive = true;
                doOnce = true;

                if (Input.GetKeyDown(interactKey)) {
                    raycastedObj1.PlayAnimation();
                }
            } else if (hit.collider.CompareTag(switchTag)) {
                if (!doOnce) {
                    fanController = hit.collider.gameObject.GetComponent<FanSwitchController>();
                    CrosshairChange(true);
                }

                isCrosshairActive = true;
                doOnce = true;

                if (Input.GetKeyDown(interactKey)) {
                    fanController.ToggleFan();
                }
            } else if (hit.collider.CompareTag(lightTag)) {
                if (!doOnce) {
                    lightController = hit.collider.gameObject.GetComponent<LightSwitchController>();
                    CrosshairChange(true);
                }

                isCrosshairActive = true;
                doOnce = true;

                if (Input.GetKeyDown(interactKey)) {
                    lightController.InteractSwitch();
                }
            }

            if (Input.GetKeyDown(interactKey)) {
                ToggleBookCanvas();
            }
        } else if (Physics.Raycast(_camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f)), transform.forward, out hit, rayLength)) {
            var readableItem = hit.collider.GetComponent<NoteController>();
            if (readableItem != null) {
                _noteController = readableItem;
                HighlightCrosshair(true);
            } else {
                ClearNote();
            }
        } else {
            if (isCrosshairActive) {
                CrosshairChange(false);
                doOnce = false;
            }
            ClearNote();
        }

        if (Input.GetKeyDown(interactKey1)) // Input.GetMouseButtonDown(1)
        {
            if (heldObj == null) {
                if (Physics.Raycast(transform.position, fwd, out hit, rayLength)) {
                    PickupObject(hit.transform.gameObject);
                }
            } else {
                DropObject();
            }
        }

        if (heldObj != null) {
            MoveObject();
        }

        if (_noteController != null) {
            if (Input.GetKeyDown(interactKey)) {
                _noteController.ShowNote();
            }
        }
    }

    private void ClearInteraction() {
        if (lightController != null) {
            CrosshairChange(false);
            lightController = null;
        }
    }

    void ToggleBookCanvas() {
        // Đổi trạng thái mở/đóng canvas
        isBookOpen = !isBookOpen;
        bookCanvas.gameObject.SetActive(isBookOpen);

        if (isBookOpen) {
            Debug.Log("Book opened.");
        } else {
            Debug.Log("Book closed.");
        }
    }

    void PickupObject(GameObject pickedObject) {
        if (pickedObject.GetComponent<Rigidbody>()) {
            heldObjRB = pickedObject.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;
            heldObjRB.transform.parent = holdArea;
            heldObj = pickedObject;
        }
    }

    void DropObject() {
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;
        heldObj.transform.parent = null;
        heldObj = null;
    }

    void MoveObject() {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f) {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    void CrosshairChange(bool on) {
        if (on && doOnce) {
            crosshair.color = Color.magenta;
        } else {
            crosshair.color = Color.white;
            isCrosshairActive = false;
        }
    }

    void ClearNote() {
        if (_noteController != null) {
            HighlightCrosshair(false);
            _noteController = null;
        }
    }

    void HighlightCrosshair(bool on) {
        if (on) {
            crosshair.color = Color.red;
        } else {
            crosshair.color = Color.white;
        }
    }
}
