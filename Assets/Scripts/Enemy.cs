using System;
using System.Collections;
using UnityEngine;
public class Enemy : MonoBehaviour, IDamageable
{
    #region
    [Header("Health")]
    public int health = 1;

    [Space(.25f)]
    [Header("Speed")]
    public float speed = 5;
    public float maxSpeed = 5;

    [Space(.25f)]
    [Header("Layers masks")]
    public LayerMask obstacleMask;
    public LayerMask enemyMask;

    [Space(.25f)]
    [Header("Shoting variables")]
    public GameObject bulletPrefab;
    public Transform bulletTransform;
    public float maxShootTime = 2f;

    [Space(.25f)]
    [Header("Audio")]
    public AudioClip destroyingClip;


    [Space(.25f)]
    [Header("Points")]
    public Transform raycastPoint;
    public Transform movePoint;
    #endregion
    // Private variables
    #region
    private float waitTimeForRotate = 0.25f;

    private Particle enemyParticle;
    private AudioManager audioManager;

    // Movement variables
    private Vector3[] rotations = { new Vector3(0, 0, 0), new Vector3(0, 0, 90), new Vector3(0, 0, -90), new Vector3(0, 0, 180) };
    private Vector2[] directions = { Vector2.up, Vector2.left, Vector2.right, Vector2.down };
    private Vector2 currentDirection;
    private Vector3 currentRotation;

    private float currentShootTime = 0;
    private bool rotateEnemyOnce = false;
    private bool canShoot;
    private Animator animator;
    #endregion
    public event EventHandler<GameObject> enemyDestroyed;


    protected virtual void Start()
    {
        enemyParticle = GetComponentInChildren<Particle>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
        if (movePoint != null)
            movePoint.parent = null;
    }

    private void Update()
    {
        Move();
        Shoot();
    }

    private void Move()
    {
        if (Physics2D.Raycast(raycastPoint.position, currentDirection, 3f, enemyMask))
        {
            StartCoroutine(RandomRotateEnemy(waitTimeForRotate));
        }

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) == 0)
        {
            if (!Physics2D.OverlapCircle(movePoint.position + (Vector3)currentDirection, 0.75f, obstacleMask))
            {
                movePoint.position += (Vector3)currentDirection;
            }
        }
        if (Physics2D.OverlapCircle(movePoint.position + (Vector3)currentDirection, 0.75f, obstacleMask))
        {
            StartCoroutine(RandomRotateEnemy(waitTimeForRotate));
        }
    }
    private void Shoot()
    {
        currentShootTime += Time.deltaTime;
        if (currentShootTime >= maxShootTime && canShoot == true)
        {
            Instantiate(bulletPrefab, bulletTransform.position, bulletTransform.rotation);
            currentShootTime = 0;
        }
    }
    private IEnumerator RandomRotateEnemy(float waitTime)
    {
        if (rotateEnemyOnce) yield break;
        rotateEnemyOnce = true;
        yield return new WaitForSeconds(waitTime);
        rotateEnemyOnce = false;
        RandomRotateEnemy();
    }
    private void RandomRotateEnemy()
    {
        var index = UnityEngine.Random.Range(0, rotations.Length);
        currentDirection = directions[index];
        currentRotation = rotations[index];
        transform.eulerAngles = currentRotation;

    }
    public virtual void Damage(int damage, Vector3 rotationOfBullet, bool ironCanDestroy)
    {
        health--;
        if (health <= 0)
        {
            enemyParticle.PlayParcile();
            enemyDestroyed?.Invoke(this, gameObject);
            audioManager.PlaySound(SoundName.DestroyingEnemy);
            Destroy(movePoint.gameObject);
            Destroy(gameObject);
        }
    }
    public void DestroyTank()
    {
        Destroy(movePoint.gameObject);
        Destroy(gameObject);
        enemyDestroyed?.Invoke(this, gameObject);
    }
    public void StopTank()
    {
        speed = 0;
        canShoot = false;
        GetComponent<Animator>().speed = 0;
    }
    public void StartTank()
    {
        speed = maxSpeed;
        canShoot = true;
        GetComponent<Animator>().speed = 1;
    }

    public void DefaultRotationAndPosition()
    {
        currentRotation = rotations[3];
        currentDirection = directions[3];
        transform.eulerAngles = currentRotation;
    }
}
