using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jang : MonoBehaviour
{
    [SerializeField] private float rayLength = 5.0f;    // Độ dài của raycast
    [SerializeField] private LayerMask layerMaskInteract; // Lớp để raycast tương tác
    [SerializeField] private string excludeLayerName = null;

    // Tương tác với các đối tượng
    private DoorController doorController;
    private FanSwitchController fanController;
    private LightSwitchController lightController;

    // Cài đặt nhặt đồ
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdArea;
    private GameObject heldObj;
    private Rigidbody heldObjRB;
    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 10.0f;
    [SerializeField] private float pickupForce = 150.0f;

    [SerializeField] private KeyCode interactKey = KeyCode.Mouse0;
    [SerializeField] private Image crosshair = null;

    private bool isCrosshairActive;
    private bool doOnce;

    private void Update() {
        // Bắn raycast
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;

        // Debug raycast để xem đường ray
        Debug.DrawRay(transform.position, fwd * rayLength, Color.magenta);

        // Nếu raycast trúng một đối tượng
        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask)) {
            // Kiểm tra từng loại đối tượng theo Tag hoặc Component

            // 1. Tương tác với cửa
            if (hit.collider.CompareTag("InteractiveObject")) {
                doorController = hit.collider.GetComponent<DoorController>();
                CrosshairChange(true);

                if (Input.GetKeyDown(interactKey) && doorController != null) {
                    doorController.PlayAnimation(); // Mở cửa
                }
            }
            // 2. Tương tác với quạt
            else if (hit.collider.CompareTag("FanSwitch")) {
                if (!doOnce) {
                    fanController = hit.collider.GetComponent<FanSwitchController>();
                    CrosshairChange(true);
                }

                if (Input.GetKeyDown(interactKey) && fanController != null) {
                    fanController.ToggleFan(); // Bật tắt quạt
                }
            }
            // 3. Tương tác với công tắc đèn
            else if (hit.collider.CompareTag("LightSwitch")) {
                if (!doOnce) {
                    lightController = hit.collider.GetComponent<LightSwitchController>();
                    CrosshairChange(true);
                }

                if (Input.GetKeyDown(interactKey) && lightController != null) {
                    lightController.InteractSwitch(); // Bật tắt đèn
                }
            }
            // 4. Nhặt đồ vật
            else if (hit.collider.CompareTag("Untagged") && hit.collider.GetComponent<Rigidbody>()) {
                heldObj = hit.collider.gameObject;
                CrosshairChange(true);

                if (Input.GetKeyDown(interactKey)) {
                    if (heldObj == null) {
                        PickupObject(heldObj);
                    } else {
                        DropObject();
                    }
                }
            }
        } else {
            // Nếu raycast không trúng gì thì đặt lại trạng thái crosshair
            if (isCrosshairActive) {
                CrosshairChange(false);
                doOnce = false;
            }
        }

        // Nếu đang nhặt đồ thì di chuyển theo vị trí holdArea
        if (heldObj != null) {
            MoveObject();
        }
    }

    // Logic nhặt đồ vật
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

    // Thả đồ vật
    void DropObject() {
        if (heldObj != null) {
            heldObjRB.useGravity = true;
            heldObjRB.drag = 1;
            heldObjRB.constraints = RigidbodyConstraints.None;
            heldObj.transform.parent = null;
            heldObj = null;
        }
    }

    // Di chuyển đồ vật
    void MoveObject() {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f) {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    // Thay đổi màu của crosshair khi raycast trúng đối tượng tương tác
    void CrosshairChange(bool on) {
        if (on && !doOnce) {
            crosshair.color = Color.magenta;
            isCrosshairActive = true;
            doOnce = true;
        } else {
            crosshair.color = Color.white;
            isCrosshairActive = false;
        }
    }
}
