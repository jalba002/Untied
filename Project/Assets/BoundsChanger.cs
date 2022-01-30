using Cinemachine;
using UnityEngine;

public class BoundsChanger : MonoBehaviour
{
    public CompositeCollider2D defaultCollider;

    public CinemachineConfiner cinemachineConfiner;

    public void ChangeBounds(CompositeCollider2D collider)
    {
        cinemachineConfiner.m_BoundingShape2D = collider;
    }

    public void SetDefault()
    {
        ChangeBounds(defaultCollider);
    }
}
