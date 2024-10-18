using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorRaycast : MonoBehaviour
{
    [SerializeField] private int rayLength = 10000;
    [SerializeField] private LayerMask layerMaskInteract;
    [SerializeField] private string excludeLayerName = null;
    
    private DoorController raycastedObj0;
    private DoorAltroller raycastedObj1;
    
    [SerializeField] private KeyCode openDoorKey = KeyCode.Mouse0;
    [SerializeField] private Image crosshair = null;
    
    private bool isCrosshairActive;
    private bool doOnce;
    private const string interactableTag0 = "InteractiveObject";
    private const string interactableTag1 = "InteractiveEntity";

    private void Update()
    {
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        int mask = 1 << LayerMask.NameToLayer(excludeLayerName) | layerMaskInteract.value;
        
        Debug.DrawRay(transform.position, fwd * rayLength, Color.magenta);

        if (Physics.Raycast(transform.position, fwd, out hit, rayLength, mask)) {
            if (hit.collider.CompareTag(interactableTag0))
            {
                raycastedObj0 = hit.collider.gameObject.GetComponent<DoorController>();
                CrosshairChange(true);

                isCrosshairActive = true;
                doOnce = true;
                
                if (Input.GetKeyDown(openDoorKey))
                {
                    raycastedObj0.PlayAnimation();
                }
            }
            else if (hit.collider.CompareTag(interactableTag1))
            {
                raycastedObj1 = hit.collider.gameObject.GetComponent<DoorAltroller>();
                CrosshairChange(true);

                isCrosshairActive = true;
                doOnce = true;
                
                if (Input.GetKeyDown(openDoorKey))
                {
                    raycastedObj1.PlayAnimation();
                }
            }
        }
        else
        {
            if (isCrosshairActive)
            {
                CrosshairChange(false);
                doOnce = false;
            }
        }
    }

    void CrosshairChange(bool on)
    {
        if (on && !doOnce)
        {
            crosshair.color = Color.magenta;
        }
        else
        {
            crosshair.color = Color.white;
            isCrosshairActive = false;
        }
    }
}