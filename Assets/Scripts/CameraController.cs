using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 300f; // Tốc độ quay camera
    public Transform playerBody; // Transform của đối tượng người chơi

    float xRotation = 0f; // Điều khiển góc xoay theo trục X (lên xuống)
    float yRotation = 180f; // Góc xoay ban đầu theo trục Y (quay về hướng 180 độ)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Khóa chuột vào giữa màn hình

        // Xoay camera về hướng 180 độ ngay khi bắt đầu game
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    void Update()
    {
        // Nhận thông tin di chuyển chuột
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Cập nhật góc xoay trục Y khi di chuyển chuột ngang (trái/phải)
        yRotation += mouseX;

        // Cập nhật góc xoay trục X khi di chuyển chuột dọc (lên/xuống)
        xRotation -= mouseY;

        // Giới hạn góc nhìn lên/xuống trong khoảng -90 đến 90 độ để tránh lật ngược camera
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Xoay camera theo trục X và Y
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
