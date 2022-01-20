using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflateDeflate : MonoBehaviour
{
    private float maxScaleY = 1.15f;

    [Header("Property")]
    [SerializeField, Range(0f, 1f)] public float InflateLevel = 1f;

    [Header("Sprite Render :")]
    [SerializeField] private SpriteRenderer body;

    [Header("Sprites :")]
    [SerializeField] private Sprite deflateBody;
    [SerializeField] private Sprite normalBody;
    [SerializeField] private Sprite inflateBody;

    [Header("Collider :")]
    [SerializeField] private CircleCollider2D circ;
    [SerializeField] private float lowRadius;
    [SerializeField] private Vector2 lowOffset;
    [SerializeField] private float mediumRadius;
    [SerializeField] private Vector2 mediumOffset;
    [SerializeField] private float highRadius;
    [SerializeField] private Vector2 highOffset;

    void Update()
    {
        // Résultat
        float scaleY = 0f;

        // Premier tier
        if (InflateLevel < (1f / 3f))
        {
            scaleY = InflateLevel / (1f / 3f) * (maxScaleY - 1f) + 1f;
            body.sprite = deflateBody;
            circ.radius = lowRadius;
            transform.parent.transform.localPosition = new Vector2(transform.parent.transform.localEulerAngles.y == 0 ? -lowOffset.x : lowOffset.x, -lowOffset.y);
        }

        // Second tier
        if (InflateLevel >= (1f / 3f) && InflateLevel < (2f / 3f))
        {
            scaleY = (InflateLevel - 1f / 3f) / (1f / 3f) * (maxScaleY - 1f) + 0.9f;
            body.sprite = normalBody;
            circ.radius = mediumRadius;
            transform.parent.transform.localPosition = new Vector2(transform.parent.transform.localEulerAngles.y == 0 ? -mediumOffset.x : mediumOffset.x, -mediumOffset.y);
        }

        // Dernier tier
        if (InflateLevel >= (2f / 3f))
        {
            scaleY = (InflateLevel - 2f / 3f) / (1f / 3f) * (maxScaleY - 1f) + 0.8f;
            body.sprite = inflateBody;
            circ.radius = highRadius;
            transform.parent.transform.localPosition = new Vector2(transform.parent.transform.localEulerAngles.y == 0 ? -highOffset.x : highOffset.x, -highOffset.y);
        }

        // Application du résultat
        var scale = body.gameObject.transform.localScale;
        scale.y = scaleY;
        body.gameObject.transform.localScale = scale;
    }
}
