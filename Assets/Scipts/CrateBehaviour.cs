using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateBehaviour : MonoBehaviour
{
    private float lastHit;

    [Header("Property :")]
    [SerializeField] public int totalLives = 2;

    [SerializeField] public int actualLives = 2;

    [SerializeField] private float recoveryTime = 0.3f;

    public void Hit(int damage)
    {
        var timeBeforceHit = Time.time - lastHit;
        if (timeBeforceHit >= recoveryTime)
        {
            actualLives -= damage;
            Debug.Log($"Crate take {damage} damage{(damage > 1 ? "s" : "")}");
        }

        if (actualLives <= 0)
        {
            Destroy(gameObject);
        }

        if (actualLives > 0)
        {
            lastHit = Time.time;
        }
    }
}
