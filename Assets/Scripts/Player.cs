using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    //create singleton
    #region Singleton
    
    static private Player _S;
    static public Player S
    {
        get { return _S; }
        private set
        {
            if (_S != null)
            {
                Debug.LogWarning("Second attempt to set Player singleton _S.");
            }

            _S = value;
        }
    }
    
    #endregion

    #region Varibles
    
    [Header("Shield")]
    [SerializeField] private GameObject shield;
    public bool underShield = false;
    
    [Header("Lives")]
    [SerializeField] private int lives = 5;
    [SerializeField] private Text livesText;
    
    [Header("Jump")]
    [SerializeField] private float jumpCoef = 15;
    
    [Header("Attack")]
    [SerializeField] private int attackPower = 1;
    
    private Vector3 pixelPos; //touch position in screen coordinates
    private Vector3 touchPos;//touch position in world coordinates
    private Vector3 previousPos = new Vector3(1,1,1); // previous touch position in world coordinates
    
    private float touchTime; //how long touching is going
    private bool swipeTouch; //Check if the current touch is swipe or not

    private Camera mainCamera;

    private Rigidbody2D rb;
    private Collider2D hit;
    
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    void Start()
    {
        S = this;
        
        //Check if you has lives from previous location
        if (DataHolder.Lives > 0) 
        { 
            lives = DataHolder.Lives;
        }
        
        touchTime = 0f;
        livesText.text = lives.ToString();
        DisactivateShield();
    }
    
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            ConvertToWorldCoordinates(touch);

            touchTime += Time.deltaTime;


            if (touch.phase == TouchPhase.Began)
            {
               hit = Physics2D.OverlapPoint(touchPos);
            }


            else if (touch.phase == TouchPhase.Moved)
            {
                Move();
            }
            

            else if (touch.phase == TouchPhase.Stationary)
            {
               HoldingAction();
            }

            else if (touch.phase == TouchPhase.Ended)
            {
                swipeTouch = false;
                previousPos = new Vector3(1,1,1);
                DisactivateShield();

                TabAction();

                touchTime = 0;
            }
        }
    }

    private void ConvertToWorldCoordinates(Touch touch)
    {
        pixelPos = touch.position;
        pixelPos.z = 10;
        touchPos = mainCamera.ScreenToWorldPoint(pixelPos);
    }

    private void Move()
    {
        swipeTouch = true;
        DisactivateShield();
        if (previousPos == new Vector3(1,1,1))
        {
            previousPos = touchPos;
        }

        var delta = touchPos.x - previousPos.x;

        if (delta > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (delta < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
                
        transform.position += new Vector3(delta, 0, 0);
        previousPos = touchPos;
    }

    private void HoldingAction()
    {
        if (hit)
        {
            if (hit.tag == "Player")
            {
                if (touchTime > 0.2f && swipeTouch == false)
                {
                    ActivateShield();
                }
            }
        }
    }

    private void ActivateShield()
    {
        shield.SetActive(true);
        underShield = true;
    }

    private void DisactivateShield()
    {
        shield.SetActive(false);
        underShield = false;
    }

    private void TabAction()
    {
        if (touchTime < 0.2f)
        {
            if (hit == Hammer.S.collisionObj && Hammer.S.attackPossible == true)
            {
                Attack();
            }
            else
            {
                var hittedObjects = Physics2D.RaycastAll(transform.position, Vector2.down, 0.3f);
                for (int i = 0; i < hittedObjects.Length; i++)
                {
                    if (hittedObjects[i].collider.tag != "Player")
                    {
                        Jump();
                        break;
                    }
                }
            }
        }
    }

    private void Attack()
    {
        Hammer.S.AttackStart(attackPower);
    }

    private void Jump()
        {
            var pixelCoef = pixelPos.y / Camera.main.pixelHeight;
            rb.AddForce(transform.up * pixelCoef * jumpCoef, ForceMode2D.Impulse);
        }

    public void ChangeHealth(int hp)
    {
        lives += hp;
        livesText.text = lives.ToString();
        DataHolder.Lives = lives;

        if (lives <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        UIManager.S.Loosing();
        Destroy(this.gameObject);
    }
}

