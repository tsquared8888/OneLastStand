using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        {
            FindObjectOfType<GameManager>().WallBreak();
            gameObject.SetActive(false);
        }
    }
}
