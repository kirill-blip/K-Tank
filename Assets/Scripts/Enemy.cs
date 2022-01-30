using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
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
    private void Start()
    {
        currentRotation = rotations[3];
        currentDir = dir[3];

        index = Random.Range(0, rotations.Length);
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
        index = Random.Range(0, rotations.Length);
        currentDir = dir[index];
        currentRotation = rotations[index];
        transform.eulerAngles = currentRotation;
    }

    public void Damage(int damage)
    {
        //if (tag != "Enemy")
        Destroy(gameObject);
    }
}
