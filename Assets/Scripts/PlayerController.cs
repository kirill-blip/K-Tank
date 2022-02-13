using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public GameObject boatGO;
    public GameObject shieldGO;

    public int health = 3;

    public GameObject bulletPrefab;
    public Transform bulletPosition;

    public Transform movePoint;
    public float speed = 5;
    public float radius;
    public float stopDistance = 0.2f;
    public LayerMask whatStopsMovement;
    public LayerMask onlyObstacleMask;

    [SerializeField]
    private float distance = .8f;

    public float maxShootingTime = .5f;
    public bool timeOfShootingChanged = false;
    private float currentShootingTime;

    public bool canMove = true;
    public bool turboShooting = false;
    public bool hasShield = false;
    public bool canDestroyBush;
    public bool canMoveOnWater;

    private GameObject bullet;
    public event EventHandler<GameObject> playerDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        particleSystem.gameObject.SetActive(true);
        boatGO.SetActive(false);
        movePoint.parent = null;
        particleSystem = GetComponentInChildren<ParticleSystem>();
        if (PlayerPrefs.GetInt("HaveBoat") == 1)
        {
            canMoveOnWater = true;
            boatGO.SetActive(true);
        }
        if (PlayerPrefs.GetInt("TurboShooting") == 1)
        {
            turboShooting = true;
        }
        if (PlayerPrefs.GetInt("CanDestroyBush") == 1)
        {
            canDestroyBush = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Movement
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) == stopDistance)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        distance = -Mathf.Abs(distance);
                        transform.eulerAngles = new Vector3(0f, 0f, 90f);
                    }
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        transform.eulerAngles = new Vector3(0f, 0f, -90f);
                        distance = Mathf.Abs(distance);
                    }
                    if (CanMove(new Vector3(distance, 0f, 0f)))
                        movePoint.position += new Vector3(distance, 0f, 0f);
                }
                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (Input.GetAxisRaw("Vertical") < 0)
                    {
                        distance = -Mathf.Abs(distance);
                        transform.eulerAngles = new Vector3(0f, 0f, 180f);
                    }
                    if (Input.GetAxisRaw("Vertical") > 0)
                    {
                        distance = Mathf.Abs(distance);
                        transform.eulerAngles = Vector3.zero;
                    }
                    if (CanMove(new Vector3(0, distance, 0f)))
                        movePoint.position += new Vector3(0f, distance, 0f);
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
        currentShootingTime += Time.deltaTime;
        if (bullet == null && Input.GetButton("Jump") && currentShootingTime >= .25f)
        {
            currentShootingTime = 0;
            bullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
            bullet.GetComponent<BulletScript>().canDestroyBush = canDestroyBush;
        }
        else if (turboShooting)
        {
            if (Input.GetKey(KeyCode.E) && currentShootingTime >= maxShootingTime)
            {
                GameObject tempBullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
                tempBullet.GetComponent<BulletScript>().canDestroyBush = canDestroyBush;
                currentShootingTime = 0;
            }
        }
    }
    public ParticleSystem particleSystem;
    public void Damage(int damage, Vector3 rotationOfBullet)
    {
        if (hasShield) return;
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