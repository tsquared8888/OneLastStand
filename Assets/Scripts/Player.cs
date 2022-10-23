using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject[] bullets;
    [SerializeField] GameObject barrel;
    [SerializeField] GameObject rightBarrel;
    [SerializeField] GameObject leftBarrel;
    [SerializeField] GameObject backBarrel;
    [SerializeField] Text ammoText;
    [SerializeField] AudioClip shootSnd;
    GameManager gm;
    AudioSource audSrc;

    float horizontalInput;
    float rotateSpeed = 200.0f;
    float reloadTimer = 0f;
    float reloadLimit = 1.0f;
    int bulletCount = 20;
    int bulletIndex = 0;

    // powerups
    bool firerateUp = false;
    float firerateTimer = 0.0f;
    float firerateLimit = .1f;
    float firerateActiveTimer = 0.0f;
    float firerateActiveLimit = 10.0f;

    bool quadShot = false;
    float quadShotActiveTimer = 0.0f;
    float quadShotActiveLimit = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        audSrc = GetComponent<AudioSource>();
        gm = FindObjectOfType<GameManager>();
        ammoText.text = bulletCount + "/20"; 
        foreach(var bullet in bullets)
        {
            bullet.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        transform.Rotate(0, 0, -horizontalInput * rotateSpeed * Time.deltaTime);
        if (!firerateUp && Input.GetKeyDown(KeyCode.Space) && bulletCount > 0)
        {
            Shoot();
        }
        else if (bulletCount <= 0)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer > reloadLimit)
            {
                bulletCount = 20;
                reloadTimer = 0.0f;
                ammoText.text = bulletCount + "/20";
            }
        }

        if (firerateUp)
        {
            audSrc.volume = 0.5f;
            firerateTimer += Time.deltaTime;
            firerateActiveTimer += Time.deltaTime;
            if (firerateTimer > firerateLimit)
            {
                firerateTimer = 0;
                Shoot();
                bulletCount = 20;
            }

            if (firerateActiveTimer > firerateActiveLimit)
            {
                firerateUp = false;
                audSrc.volume = 1.0f;
            }
        }

        if (quadShot)
        {
            quadShotActiveTimer += Time.deltaTime;
            if (quadShotActiveTimer > quadShotActiveLimit)
            {
                quadShot = false;
                audSrc.volume = 1.0f;
            }
        }
    }

    void Shoot()
    {
        audSrc.PlayOneShot(shootSnd);
        var bullet = bullets[bulletIndex];
        bullet.transform.position = barrel.transform.position;
        bullet.transform.rotation = barrel.transform.rotation;
        bullet.SetActive(true);
        bulletCount--;
        BulletIndexing();
        ammoText.text = bulletCount + "/20";
        if (quadShot)
        {
            bullet = bullets[bulletIndex];
            bullet.transform.position = rightBarrel.transform.position;
            bullet.transform.rotation = rightBarrel.transform.rotation;
            bullet.SetActive(true);
            BulletIndexing();

            bullet = bullets[bulletIndex];
            bullet.transform.position = leftBarrel.transform.position;
            bullet.transform.rotation = leftBarrel.transform.rotation;
            bullet.SetActive(true);
            BulletIndexing();

            bullet = bullets[bulletIndex];
            bullet.transform.position = backBarrel.transform.position;
            bullet.transform.rotation = backBarrel.transform.rotation;
            bullet.SetActive(true);
            BulletIndexing();
            audSrc.volume = 0.5f;
        }
    }

    void BulletIndexing()
    {
        bulletIndex++;
        if (bulletIndex >= bullets.Length)
        {
            bulletIndex = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            gm.GameOver();
            gameObject.SetActive(false);
        }
    }

    public void FirerateUp()
    {
        bulletCount = 20;
        firerateUp = true;
        firerateTimer = 0.0f;
        firerateActiveTimer = 0.0f;
        ammoText.text = bulletCount + "/20";
    }

    public void QuadShot()
    {
        quadShot = true;
        quadShotActiveTimer = 0.0f;
    }
}
