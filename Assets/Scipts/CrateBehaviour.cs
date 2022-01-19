using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBehaviour : MonoBehaviour
{
    private float lastHit;
    private SpriteRenderer crrd;

    [Header("Property :")]
    [SerializeField] public int totalLives = 2;

    [SerializeField] public int actualLives = 2;

    [SerializeField] private float recoveryTime = 0.3f;

    [SerializeField] private float timeToDestroy = 2f;

    [Header("Crates sprites :")]
    [SerializeField] private Sprite normalCrate;

    [SerializeField] private Sprite semiBrokenCrate;

    [SerializeField] private Sprite brokenCrate;

    private void Start()
    {
        crrd = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void Hit(int damage)
    {
        var timeBeforceHit = Time.time - lastHit;
        if (timeBeforceHit >= recoveryTime)
        {
            actualLives -= damage;
        }

        if (actualLives > 0)
        {
            lastHit = Time.time;
        }

        if (actualLives == totalLives)
        {
            crrd.sprite = normalCrate;
        }

        if (actualLives < totalLives && actualLives > 0)
        {
            crrd.sprite = semiBrokenCrate;
        }

        if (actualLives <= 0)
        {
            crrd.sprite = brokenCrate;
            crrd.color = Color.Lerp(crrd.color, new Color(crrd.color.r, crrd.color.g, crrd.color.b, 0), Time.deltaTime * (1 / timeToDestroy));
            Destroy(gameObject, timeToDestroy);
        }
    }
}
