using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnWater : MonoBehaviour
{
    [SerializeField]
    [Header("Object to detect :")]
    private new GameObject gameObject;

    [SerializeField]
    private InflateDeflate body;

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

            // Gonflement du poisson
            body.InflateLevel = Mathf.Lerp(body.InflateLevel, 1, Time.deltaTime * 5);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == gameObject.name)
        {
            collision.gameObject.GetComponent<PlayerBehaviour>().isOnWater = false;
        }

        // Dégonflement du poisson
        body.InflateLevel = Mathf.Lerp(body.InflateLevel, 0, Time.deltaTime * 0.001f);
    }
}
