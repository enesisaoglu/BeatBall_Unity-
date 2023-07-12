using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    public int hits = 1;
    public int points = 100;
    public Vector3 rotator;
    public Material hitMaterial;

    Material orgMaterial;
    Renderer brickRenderer;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
        brickRenderer = GetComponent<Renderer>();
        orgMaterial = brickRenderer.sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        hits--;
        //Score points...
        if (hits <= 0)
        {
            GameManager.Instance.Score += points;
            Destroy(gameObject);
        }
        brickRenderer.sharedMaterial = hitMaterial;
        Invoke("RestoreMaterial", 0.05f);
    }

    private void RestoreMaterial()
    {
        brickRenderer.sharedMaterial = orgMaterial;
    }
}
