using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private float speed = 35f;
    Rigidbody rigidBody;
    Vector3 velocity;
    Renderer ballRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        ballRenderer = GetComponent<Renderer>();
        Invoke("Launch", 0.5f);
    }

    private void Launch()
    {
        rigidBody.velocity = Vector3.up * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (speed <= 34)
        {
            speed = 35;
        }

        rigidBody.velocity = rigidBody.velocity.normalized * speed;
        velocity = rigidBody.velocity;

        if (!ballRenderer.isVisible)
        {
            GameManager.Instance.Balls--;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rigidBody.velocity = Vector3.Reflect(velocity, collision.contacts[0].normal);
    }
}
