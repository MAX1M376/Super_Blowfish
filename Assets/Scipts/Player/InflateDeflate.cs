using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflateDeflate : MonoBehaviour
{
    private float maxScaleY = 1.15f;
    private PlayerBehaviour playerBehaviour;

    [Header("Property")]
    [Range(0f, 1f)]
    [SerializeField]
    public float InflateLevel = 1f;

    [Header("Sprite Render :")]
    [SerializeField]
    private SpriteRenderer body;

    [Header("Sprites :")]
    [SerializeField]
    private Sprite deflateBody;

    [SerializeField]
    private Sprite normalBody;

    [SerializeField]
    private Sprite inflateBody;

    [Header("Collider :")]
    [SerializeField]
    private CircleCollider2D circ;

    [SerializeField]
    private float lowRadius;

    [SerializeField]
    private float mediumRadius;

    [SerializeField]
    private float highRadius;

    private void Start()
    {
        playerBehaviour = gameObject.transform.parent.parent.GetComponent<PlayerBehaviour>();
    }

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
        }

        // Second tier
        if (InflateLevel >= (1f / 3f) && InflateLevel < (2f / 3f))
        {
            scaleY = (InflateLevel - 1f / 3f) / (1f / 3f) * (maxScaleY - 1f) + 1f;
            body.sprite = normalBody;
            circ.radius = mediumRadius;
        }

        // Dernier tier
        if (InflateLevel >= (2f / 3f))
        {
            scaleY = (InflateLevel - 2f / 3f) / (1f / 3f) * (maxScaleY - 1f) + 1f;
            body.sprite = inflateBody;
            circ.radius = highRadius;
        }

        // Application du résultat
        var scale = body.gameObject.transform.localScale;
        scale.y = scaleY;
        body.gameObject.transform.localScale = scale;
    }
}
