using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] powerups;
    GameManager gm;
    Vector3 targetPos;
    protected float speed;
    protected int health;
    protected int maxHealth;
    float posMax = 5;
    float stunnedTimer = 0.0f;
    float stunnedMax = 5.0f;
    bool stunned = false;

    public NormalEnemy ()
    {
        health = 1;
        maxHealth = 1;
        speed = 0.75f;
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        NewPos();
        targetPos = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            powerupDrop();
            NewPos();
            health = maxHealth;
            gm.AddScore();
            stunned = false;
            gameObject.SetActive(false);
        }
        else if (stunned)
        {
            stunnedTimer += Time.deltaTime;
            if (stunnedTimer > stunnedMax)
            {
                stunned = false;
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
    }

    void powerupDrop()
    {
        int index = Random.Range(0, 40);
        switch (index)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                Instantiate(powerups[index], transform.position, Quaternion.identity);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
        {
            health--;
        }
    }
    public void NewPos()
    {
        int side = Random.Range(0, 4);
        switch (side)
        { 
            case 0:
                transform.position = new Vector3(posMax, Random.Range(-posMax, posMax));
                break;
            case 1:
                transform.position = new Vector3(Random.Range(-posMax, posMax), -posMax);
                break;
            case 2:
                transform.position = new Vector3(-posMax, Random.Range(-posMax, posMax));
                break;
            case 3:
                transform.position = new Vector3(Random.Range(-posMax, posMax), posMax);
                break;
        }
        transform.up = player.transform.position - transform.position;
    }

    public void Stunned()
    {
        stunned = true;
        stunnedTimer = 0.0f;
    }

    public void SetHealth()
    {
        health = 0;
    }
}
