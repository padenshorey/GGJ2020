using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeForce : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private float thrust = 10.0f;

    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb2D.AddForce(new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * thrust, ForceMode2D.Impulse);
            Debug.Log("APPLY FORCE");
        }
    }
}
