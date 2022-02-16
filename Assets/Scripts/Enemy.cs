using System;
using UnityEngine;
public class Enemy : MonoBehaviour, IDamageable
{
    public int health = 1;
    public float speed = 5;
    public float maxSpeed = 5;
    public LayerMask obstacleMask;
    public LayerMask enemyMask;
    private Vector3[] rotations = { new Vector3(0, 0, 0), new Vector3(0, 0, 90), new Vector3(0, 0, -90), new Vector3(0, 0, 180) };
    private Vector2[] dir = { Vector2.up, Vector2.left, Vector2.right, Vector2.down };
    private Vector2 currentDir;
    private Vector3 currentRotation;

    public GameObject bulletPrefab;
    public Transform bulletTransform;

    public bool canShoot;

    private new ParticleSystem particleSystem;
    public event EventHandler<GameObject> enemyDestroyed;
    public Rigidbody2D enemyRigidbody;

    private void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        particleSystem = GetComponentInChildren<ParticleSystem>();
        currentRotation = rotations[3];
        currentDir = dir[3];

        index = UnityEngine.Random.Range(0, rotations.Length);
        currentDir = dir[index];
        currentRotation = rotations[index];
        transform.eulerAngles = currentRotation;
    }

    int index;
    float currentTime = 0;
    public float maxTime = 2f;

    private void Update()
    {
        if (!Physics2D.Raycast(transform.position, currentDir, 1, obstacleMask))
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else
        {
            RandomRotateEnemy();
        }

        if (Physics2D.Raycast(bulletTransform.transform.position, currentDir, 2, enemyMask))
        {
            RandomRotateEnemy();
        }
        currentTime += Time.deltaTime;
        if (currentTime >= maxTime && canShoot == true)
        {
            Shoot();
            currentTime = 0;
        }
    }

    void Shoot()
    {
        GameObject go = Instantiate(bulletPrefab, bulletTransform.position, bulletTransform.rotation);
    }

    void RandomRotateEnemy()
    {
        index = UnityEngine.Random.Range(0, rotations.Length);
        currentDir = dir[index];
        currentRotation = rotations[index];
        transform.eulerAngles = currentRotation;
    }

    public virtual void Damage(int damage, Vector3 rotationOfBullet)
    {
        health--;
        if (health <= 0)
        {
            particleSystem.transform.parent = null;
            particleSystem.Play();
            particleSystem.GetComponent<ParticaleScript>().DestroyParticaleSystem();
            enemyDestroyed?.Invoke(this, gameObject);
            Destroy(gameObject);
        }
    }

    public virtual void DestroyTank()
    {
        Destroy(gameObject);
    }

    public virtual void StopTank()
    {
        speed = 0;
        canShoot = false;
    }

    public virtual void StartTank()
    {
        speed = maxSpeed;
        canShoot = true;
    }
}
