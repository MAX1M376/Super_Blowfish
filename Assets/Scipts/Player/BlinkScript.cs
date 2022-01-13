using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkScript : MonoBehaviour
{
    private float lastBlink;
    private float randomDelay;

    [Header("Blink :")]
    public bool isBlinking;
    public bool blink;

    [Header("Property :")]
    [SerializeField]
    private float blinkDelay = 2f;

    [SerializeField]
    private bool addRandomness = false;

    [SerializeField]
    private float delayBetweenBackTop = 0f;

    [SerializeField]
    private float blinkTime = 0.5f;

    [Header("Sprites :")]
    public Sprite BackEyeOpen;
    public Sprite BackEyeClose;
    public Sprite TopEyeOpen;
    public Sprite TopEyeClose;

    [Header("Sprites Renders :")]
    public SpriteRenderer TopEye;
    public SpriteRenderer BackEye;

    private void Start()
    {
        lastBlink = Time.time;
        randomDelay = addRandomness ? Random.Range(0f, 3f) : 0f;
    }

    private void Update()
    {
        if (!isBlinking && blink)
        {
            lastBlink = Time.time - (blinkDelay + randomDelay);
            blink = false;
        }

        if ((Time.time - lastBlink) >= (blinkDelay + randomDelay))
        {
            BackEye.sprite = BackEyeClose;
            isBlinking = true;
        }

        if ((Time.time - lastBlink) >= (blinkDelay + randomDelay + delayBetweenBackTop) && isBlinking)
        {
            TopEye.sprite = TopEyeClose;
        }

        if ((Time.time - lastBlink) >= (blinkDelay + randomDelay + blinkTime) && isBlinking)
        {
            BackEye.sprite = BackEyeOpen;
        }

        if ((Time.time - lastBlink) >= (blinkDelay + randomDelay + blinkTime + delayBetweenBackTop) && isBlinking)
        {
            TopEye.sprite = TopEyeOpen;
            isBlinking = false;
            lastBlink = Time.time;
            randomDelay = addRandomness ? Random.Range(0f, 3f) : 0f;
        }
    }
}
