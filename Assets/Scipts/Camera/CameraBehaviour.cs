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
    private Vector3 bottomLeftScreenWorld;

    [Header("Object to follow :")]
    [SerializeField] private Transform player;

    [Header("Behaviour :")]
    [SerializeField, Range(0f, 1f)] private float smoothSpeed = 0.125f;

    [Header("Map :")]
    [SerializeField] private Vector2 topLeft;
    [SerializeField] private Vector2 bottomRight;

    private void Start()
    {
        cam = GetComponent<Camera>();
        offset = transform.position - player.position;
        screenWorldWidth = cam.ScreenToWorldPoint(Vector3.right * cam.pixelWidth).x - cam.ScreenToWorldPoint(Vector3.zero).x;
        screenWorldHeight = cam.ScreenToWorldPoint(Vector3.down * cam.pixelHeight).y - cam.ScreenToWorldPoint(Vector3.zero).y;
        bottomLeftScreenWorld = cam.ScreenToWorldPoint(Vector3.zero) - transform.position;
        transform.position = CameraPosition();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, CameraPosition(), smoothSpeed * Time.deltaTime * 300);
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 pos = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            pos.x += Random.Range(-1.0f, 1.0f) * magnitude * Time.deltaTime;
            pos.y += Random.Range(-1.0f, 1.0f) * magnitude * Time.deltaTime;

            pos.x = Mathf.Clamp(bottomLeftScreenWorld.x + pos.x, topLeft.x, bottomRight.x - screenWorldWidth) - bottomLeftScreenWorld.x;
            pos.y = Mathf.Clamp(bottomLeftScreenWorld.y + pos.y, bottomRight.y, topLeft.y + screenWorldHeight) - bottomLeftScreenWorld.y;

            elapsed += Time.deltaTime;
            transform.position = pos;
            yield return null;
        }
    }

    private Vector3 CameraPosition()
    {
        Vector3 pos = player.position + offset;

        pos.x = Mathf.Clamp(bottomLeftScreenWorld.x + pos.x, topLeft.x, bottomRight.x - screenWorldWidth) - bottomLeftScreenWorld.x;
        pos.y = Mathf.Clamp(bottomLeftScreenWorld.y + pos.y, bottomRight.y, topLeft.y + screenWorldHeight) - bottomLeftScreenWorld.y;

        return pos;
    }
}
