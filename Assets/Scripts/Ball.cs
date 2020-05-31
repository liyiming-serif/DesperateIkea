using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("terrain"))
        {
            Debug.Log("touched the floor");
            Collider2D collider = Physics2D.OverlapCircle((Vector2)transform.position, 6f, LayerMask.GetMask("player"));
            Debug.Log(collider);
            if (collider != null)
            {
                Rigidbody2D rb2d = collider.GetComponent<Rigidbody2D>();
                if (rb2d != null)
                {
                    rb2d.constraints = RigidbodyConstraints2D.None;

                    rb2d.AddForce(
                        new Vector2(
                            Random.Range(-1f, 1f),
                            Random.Range(0f, 1f)
                        ) * 65,
                        ForceMode2D.Impulse
                        );
                    rb2d.AddTorque(Random.Range(375, 220));
                }
            }

            GameManager.Instance().EndGame(transform.position.x);
            Destroy(gameObject);
        }
    }

    void Explode()
    {

    }
}
