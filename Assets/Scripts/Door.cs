using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Door : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("Scene number in build that door is leading to")]
    [SerializeField] private int sceneBehindTheDoor;
    [SerializeField] private bool isActive;

    private void Start()
    {
        if (isActive)
        {
            Activate();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive)
        {
            if (other.tag == "Player")
            {
                SceneManager.LoadScene(sceneBehindTheDoor);
            }
        }
    }

    public void Activate()
    {
        isActive = true;
        
        //Change the transparency
        var sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }
}
