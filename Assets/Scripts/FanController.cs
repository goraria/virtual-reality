using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanController : MonoBehaviour
{
    private bool isFanOn = false; // Trạng thái của quạt (tắt hoặc bật)
    private Animator fanAnimator;

    private void Awake()
    {
        fanAnimator = gameObject.GetComponent<Animator>(); // Lấy Animator của quạt
    }

    // Phương thức bật/tắt quạt
    public void ToggleFan()
    {
        if (!isFanOn)
        {
            fanAnimator.Play("fan1Rotate", 0, 0.0f); // Chơi animation bật quạt
            isFanOn = true;
        }
        else
        {
            fanAnimator.Play("fan1Stop", 0, 0.0f); // Chơi animation tắt quạt
            isFanOn = false;
        }
    }
}
