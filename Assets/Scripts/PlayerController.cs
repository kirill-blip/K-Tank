using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    #region
    public int health = 3;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public Transform movePoint;
    public float speed = 5;
    public float radius;
    public float distance = 0.2f;

    public LayerMask whatStopsMovement;
    public LayerMask onlyObstacleMask;

    private float currentShootingTime;
    public float maxShootingTime = .5f;
    public bool timeOfShootingChanged = false;

    public event EventHandler<GameObject> playerDestroyed;
    public bool canMove = true;
    public bool turboShooting = false;
    public GameObject bullet;


    [SerializeField]
    private float dis = .8f;
    #endregion
    public Transform pos;
    public GameObject boat;
    public bool canMoveOnWater;
    // Start is called before the first frame update
    void Start()
    {
        boat.SetActive(false);
        movePoint.parent = null;
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        // Movement
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) == distance)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        dis = -Mathf.Abs(dis);
                        transform.eulerAngles = new Vector3(0f, 0f, 90f);
                    }
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        transform.eulerAngles = new Vector3(0f, 0f, -90f);
                        dis = Mathf.Abs(dis);
                    }
                    if (CanMove(new Vector3(dis, 0f, 0f)))
                        movePoint.position += new Vector3(dis, 0f, 0f);
                }
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        dis = -Mathf.Abs(dis);
                        transform.eulerAngles = new Vector3(0f, 0f, 180f);
                    }
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        dis = Mathf.Abs(dis);
                        transform.eulerAngles = Vector3.zero;
                    }
                    if (CanMove(new Vector3(0, dis, 0f)))
                        movePoint.position += new Vector3(0f, dis, 0f);
                }
            }
        }

        // Shooting
        Shoot();
    }

    bool CanMove(Vector3 point)
    {
        if (canMoveOnWater)
            return !Physics2D.OverlapCircle(movePoint.position + point, radius, onlyObstacleMask);
        else
            return !Physics2D.OverlapCircle(movePoint.position + point, radius, whatStopsMovement);
    }
    void Shoot()
    {
        if (bullet == null && Input.GetButton("Jump"))
        {
            bullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
        }
        else if(turboShooting)
        {
            currentShootingTime += Time.deltaTime;
            if (Input.GetKey(KeyCode.E) && currentShootingTime >= maxShootingTime)
            {
                GameObject tempBullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
                currentShootingTime = 0;
            }
        }
    }
    public ParticleSystem particleSystem;
    public void Damage(int damage, Vector3 rotationOfBullet)
    {
        health--;
        canMove = false;
        particleSystem.Play();
        GetComponent<Collider2D>().enabled = false;
        playerDestroyed?.Invoke(this, gameObject);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public int GetHealth()
    {
        return health;
    }

}