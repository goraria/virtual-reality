using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelingFanRotation : MonoBehaviour
{
    // public GameObject objectToClick;       // Đối tượng bạn nhấp vào
    // public GameObject objectToRotate;      // Đối tượng sẽ quay
    //
    // public float rotationSpeed = 60f;      // Tốc độ quay ban đầu
    // private float speedIncrement = 60f;    // Mỗi lần nhấn tăng tốc 60
    // private int clickCount = 0;            // Đếm số lần nhấn
    // private int maxClicks = 6;             // Sau 6 lần nhấn sẽ dừng quay
    //
    // private bool isRotating = false;       // Kiểm tra xem đối tượng có đang quay không
    //
    // void Update()
    // {
    //     // Kiểm tra xem có nhấp chuột không
    //     if (Input.GetMouseButtonDown(0))  // Nút chuột trái
    //     {
    //         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //         RaycastHit hit;
    //
    //         // Raycast để phát hiện xem objectToClick có được nhấp không
    //         if (Physics.Raycast(ray, out hit))
    //         {
    //             if (hit.collider.gameObject == objectToClick)
    //             {
    //                 clickCount++;
    //
    //                 if (!isRotating && clickCount <= maxClicks)
    //                 {
    //                     isRotating = true;  // Bắt đầu quay
    //                 }
    //
    //                 // Tăng tốc độ quay mỗi lần nhấn (tối đa 6 lần)
    //                 if (clickCount <= maxClicks)
    //                 {
    //                     rotationSpeed += speedIncrement;
    //                 }
    //
    //                 // Dừng quay nếu đã nhấn 6 lần
    //                 if (clickCount == maxClicks)
    //                 {
    //                     isRotating = false;
    //                 }
    //             }
    //         }
    //     }
    //
    //     // Xử lý logic quay
    //     if (isRotating)
    //     {
    //         // Quay đối tượng với tốc độ hiện tại
    //         objectToRotate.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    //     }
    // }
}