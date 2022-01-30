#pragma warning disable CS0168

using com.kpg.ggj2022.player;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class TriggerArea : MonoBehaviour
{
    public Color debugColor = Color.red;
    [ShowInInspector] protected bool trapEnabled;

    public UnityEvent OnTriggerActivation = new UnityEvent();

    public void Start()
    {
        // OnTriggerActivation.AddListener(ChangeColor);
        trapEnabled = true;
    }

    public void ChangeColor()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        Color tempColor = spriteRenderer.color;
        spriteRenderer.color = debugColor;
        debugColor = tempColor;
    }

    [Button("Force invoke")]
    public void Test()
    {
        OnTriggerActivation.Invoke();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            if(pc != null)
            {
                // Enable the trigger
                OnTriggerActivation.Invoke();
                ChangeColor();
            }
        }
        catch (Exception e)
        {

        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        try
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            if (pc != null)
            {
                ChangeColor();
            }
        }
        catch (Exception e)
        {

        }
    }
}
