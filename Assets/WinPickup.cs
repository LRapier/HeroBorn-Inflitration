using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPickup : MonoBehaviour
{
    public GameBehavior gameManager;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player" || collision.gameObject.name == "DisguisedPlayer")
        {
            Destroy(this.transform.gameObject);
            Debug.Log("Intel collected!");
            GetComponent<AudioSource>().Play();
            gameManager.Items += 1;
        }
    }
}
