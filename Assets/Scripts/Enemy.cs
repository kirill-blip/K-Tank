using UnityEngine;
using System.Collections.Generic;
using System;
public class Enemy : MonoBehaviour, IDamageable
{
    public float speed = 5;
    public LayerMask obstacleMask;
    public LayerMask enemyMask;
    private Vector3[] rotations = { new Vector3(0, 0, 0), new Vector3(0, 0, 90), new Vector3(0, 0, -90), new Vector3(0, 0, 180) };
    private Vector2[] dir = { Vector2.up, Vector2.left, Vector2.right, Vector2.down };
    private Vector2 currentDir;
    private Vector3 currentRotation;

    public GameObject bulletPrefab;
    public Transform bulletTransform;

    private ParticleSystem particleSystem;
    public event EventHandler<GameObject> enemyDestroyed;
    private void Start()
    {
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
    float maxTime = 3f;
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
        if (currentTime >= maxTime)
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

    public void Damage(int damage, Vector3 rotationOfBullet)
    {
        particleSystem.transform.parent = null;
        particleSystem.Play();
        particleSystem.GetComponent<ParticaleScript>().DestroyParticaleSystem();
        //if (tag != "Enemy")
        enemyDestroyed?.Invoke(this, gameObject);
        Destroy(gameObject);
    }
}
