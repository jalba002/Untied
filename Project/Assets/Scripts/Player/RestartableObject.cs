﻿using UnityEngine;

public class RestartableObject : MonoBehaviour
{
    protected Vector3 startingPosition;
    protected Quaternion startingRotation;

    protected MeshRenderer meshRenderer;

    public void Awake()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public virtual void Restart()
    {
        this.gameObject.SetActive(true);
        meshRenderer.enabled = true;
        
        transform.position = startingPosition;
        transform.rotation = startingRotation;
    }

    public void SetSpawnSettings(Vector3 position, Quaternion rotation)
    {
        this.startingPosition = position;
        this.startingRotation = rotation;
    }

    public Vector3 GetStartingPos()
    {
        return startingPosition;
    }
}
