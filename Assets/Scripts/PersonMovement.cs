using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    public float baseSpeed = 10f;         // Tốc độ di chuyển cơ bản
    public float slowSpeed = 5f;          // Tốc độ di chuyển chậm khi nhấn Shift
    public float fastSpeed = 20f;         // Tốc độ di chuyển nhanh khi nhấn Ctrl
    public float verticalSpeed = 6f;      // Tốc độ di chuyển lên/xuống (dùng cho Space/C)

    public float jumpHeight = 2f;         // Chiều cao khi nhảy
    public float gravity = -9.81f;        // Lực hấp dẫn

    public float rotationSpeed = 100f;    // Tốc độ xoay của camera (độ/giây)
    public float minY = -60f;             // Góc xoay tối thiểu theo trục X (lên/xuống)
    public float maxY = 60f;              // Góc xoay tối đa theo trục X (lên/xuống)

    private float yaw = 180f;              // Góc xoay quanh trục Y (trái/phải)
    private float pitch = -10f;           // Góc xoay quanh trục X (lên/xuống)

    private bool isGrounded = false;      // Kiểm tra nếu đang đứng trên mặt đất
    public LayerMask groundLayer;         // Lớp dùng để phát hiện mặt đất
    public float groundDistance = 0.5f;   // Khoảng cách để kiểm tra mặt đất

    private Vector3 velocity;             // Vận tốc (dùng để xử lý lực hấp dẫn và nhảy)
    private CharacterController controller;

    void Start()
    {
        // Khởi tạo thành phần CharacterController
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
    }

    void Update()
    {
        // Kiểm tra nếu nhân vật đang đứng trên mặt đất (chạm mặt đất)
        isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundLayer);

        // Đặt lại vận tốc theo chiều dọc (trục Y) khi nhân vật chạm đất
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Đặt giá trị nhỏ để giữ nhân vật trên mặt đất
        }

        // Nhận input điều khiển di chuyển (WASD)
        Vector3 moveDirection = Vector3.zero;
        float currentSpeed = baseSpeed;  // Tốc độ di chuyển mặc định

        // Điều chỉnh tốc độ di chuyển dựa trên phím Shift hoặc Ctrl
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = slowSpeed;  // Di chuyển chậm khi nhấn Shift
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = fastSpeed;  // Di chuyển nhanh khi nhấn Ctrl
        }

        // Di chuyển theo trục ngang (WASD)
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += transform.forward;  // Di chuyển về phía trước
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection -= transform.forward;  // Di chuyển về phía sau
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection -= transform.right;    // Di chuyển sang trái
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += transform.right;    // Di chuyển sang phải
        }

        // Nhảy khi nhấn phím Space và đang chạm đất
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Tính toán vận tốc theo chiều dọc để nhảy lên
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Áp dụng di chuyển ngang
        controller.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);

        // Áp dụng lực hấp dẫn nếu không chạm đất
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // Áp dụng di chuyển theo chiều dọc (lực hấp dẫn và nhảy)
        controller.Move(velocity * Time.deltaTime);

        // Xử lý xoay camera (góc yaw/pitch)
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            yaw -= rotationSpeed * Time.deltaTime;  // Xoay sang trái
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            yaw += rotationSpeed * Time.deltaTime;  // Xoay sang phải
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            pitch -= rotationSpeed * Time.deltaTime;  // Xoay lên
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            pitch += rotationSpeed * Time.deltaTime;  // Xoay xuống
        }

        // Giới hạn góc xoay theo trục X để tránh lật camera
        pitch = Mathf.Clamp(pitch, minY, maxY);

        // Áp dụng góc xoay cho camera
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);
    }
}
