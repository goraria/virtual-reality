using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAltroller : MonoBehaviour
{
    private Animator doorAnim;
    private bool doorOpen = false;

    private void Awake()
    {
        doorAnim = gameObject.GetComponent<Animator>();
    }

    public void PlayAnimation()
    {
        if (!doorOpen)
        {
            doorAnim.Play("door1Open", 0, 0.0f);
            doorOpen = true;
        }
        else
        {
            doorAnim.Play("door1Close", 0, 0.0f);
            doorOpen = false;
        }
    }
}
