using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 10.0f;
    GameManager gm;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.up * speed * Time.deltaTime;
        transform.Translate(movement);
        if (!GetComponent<Renderer>().isVisible)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            gameObject.SetActive(false);

        }
        else if (collision.gameObject.tag == "Firerate")
        {
            gameObject.SetActive(false);
            player.FirerateUp();
        }
        else if (collision.gameObject.tag == "Nuke")
        {
            gameObject.SetActive(false);
            gm.Nuked();
        }
        else if (collision.gameObject.tag == "Stun")
        {
            gameObject.SetActive(false);
            gm.Stunned();
        }
        else if (collision.gameObject.tag == "Omni")
        {
            gameObject.SetActive(false);
            player.QuadShot();
        }
    }
}
