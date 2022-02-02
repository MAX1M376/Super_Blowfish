using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWater : MonoBehaviour
{
    [SerializeField]
    [Header("Object to detect :")]
    private new GameObject gameObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == gameObject.name)
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().isOnWater = true;

            var force = collision.gameObject.GetComponent<PlayerBehaviour>().force;
            force.y = -3f;
            collision.gameObject.GetComponent<PlayerBehaviour>().force = force;

            var velo = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            velo.y = -3f;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = velo;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == gameObject.name)
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().isOnWater = false;
        }
    }
}
