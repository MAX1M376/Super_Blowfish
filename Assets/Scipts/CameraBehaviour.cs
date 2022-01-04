using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Vector3 offset;

    [Header("Object to follow :")]
    [SerializeField]
    private Transform gameObject;

    [Header("Behaviour :")]
    [Range(0f, 1f)]
    [SerializeField]
    private float smoothSpeed = 0.125f;

    private void Start()
    {
        offset = transform.position - gameObject.position;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, offset + gameObject.position, smoothSpeed);
    }
}
