using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateHandGrabber : MonoBehaviour
{
    public HandGrabberController handGrabber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        handGrabber.TriggerEnter2D(collision);
    }
}
