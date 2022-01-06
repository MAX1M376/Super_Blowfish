using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraBehaviour : MonoBehaviour
{
    private Camera cam;
    private Vector3 offset;
    private float screenWorldWidth;
    private float screenWorldHeight;
    private Vector3 topRightScreenWorld;

    [Header("Object to follow :")]
    [SerializeField]
    private new Transform gameObject;

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
        screenWorldHeight = cam.ScreenToWorldPoint(Vector3.down * cam.pixelHeight).y - cam.ScreenToWorldPoint(Vector3.zero).y;
        topRightScreenWorld = cam.ScreenToWorldPoint(Vector3.zero) - transform.position;
    }

    private void Update()
    {
        var minX = (tlmap.size.x + tlmap.transform.parent.position.x) * tlmap.cellSize.x / -2f;
        var maxX = (tlmap.size.x + tlmap.transform.parent.position.x) * tlmap.cellSize.x / 2f;
        var minY = (tlmap.size.y + tlmap.transform.parent.position.y) * tlmap.cellSize.y / -2f;
        var maxY = (tlmap.size.y + tlmap.transform.parent.position.y) * tlmap.cellSize.y / 2f;
        var pos = offset + gameObject.position;

        pos.x = Mathf.Clamp(topRightScreenWorld.x + pos.x, minX, maxX - screenWorldWidth) - topRightScreenWorld.x;
        pos.y = Mathf.Clamp(topRightScreenWorld.y + pos.y, maxY + screenWorldHeight, minY) - topRightScreenWorld.y;

        transform.position = Vector3.Lerp(transform.position, pos, smoothSpeed * Time.deltaTime * 300);
    }
}
