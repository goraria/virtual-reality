using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanSwitchController : MonoBehaviour
{
    [SerializeField] private Animator doorAnim = null;
    private bool doorOpen = false;
    [SerializeField] private int waitTimer = 1;
    [SerializeField] private bool pauseInteraction = false;
    
    private IEnumerator PauseDoorInteraction()
    {
        pauseInteraction = true;
        yield return new WaitForSeconds(waitTimer);
        pauseInteraction = false;
    }

    public void ToggleFan()
    {
        if (!doorOpen && !pauseInteraction)
        {
            doorAnim.Play("fan1Rotate", 0, 0.0f);
            doorOpen = true;
            StartCoroutine(PauseDoorInteraction());
        }
        else if (doorOpen && !pauseInteraction)
        {
            doorAnim.Play("fan1Stop", 0, 0.0f);
            doorOpen = false; 
            StartCoroutine(PauseDoorInteraction());
        }
    }
}
