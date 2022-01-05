using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraBehaviour : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private float screenWorldWidth;
    private Vector3 topRightScreenWorld;

    [Header("Object to follow :")]
    [SerializeField]
    private Transform gameObject;

    [Header("Behaviour :")]
    [Range(0f, 1f)]
    [SerializeField]
    private float smoothSpeed = 0.125f;

    [Header("Map :")]
    [SerializeField]
    private Tilemap tlmap;

    private void Start()
    {
        cam = GetComponent<Camera>();
        offset = transform.position - gameObject.position;
        screenWorldWidth = cam.ScreenToWorldPoint(Vector3.right * cam.pixelWidth).x - cam.ScreenToWorldPoint(Vector3.zero).x;
        topRightScreenWorld = cam.ScreenToWorldPoint(Vector3.zero) - transform.position;
    }

    private void Update()
    {
        var minX = (tlmap.size.x + tlmap.transform.parent.position.x) * tlmap.cellSize.x / -2f;
        var maxX = (tlmap.size.x + tlmap.transform.parent.position.x) * tlmap.cellSize.x / 2f;
        var pos = offset + gameObject.position;
        
        pos.x = Mathf.Clamp(topRightScreenWorld.x + pos.x, minX, maxX - screenWorldWidth) + screenWorldWidth / 2f;
        transform.position = Vector3.Lerp(transform.position, pos, smoothSpeed * Time.deltaTime * 300);
    }
}
