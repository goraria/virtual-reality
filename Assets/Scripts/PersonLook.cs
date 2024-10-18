using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;  // Gán thân người chơi

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Khóa con trỏ
    }

    void Update()
    {
        // Đọc giá trị chuột
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Điều chỉnh góc xoay camera theo trục X (tránh quay quá 90 độ)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Xoay camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Xoay thân người chơi theo trục Y của chuột
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
