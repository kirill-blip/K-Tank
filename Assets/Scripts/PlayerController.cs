using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    #region
    [Header("Clips")]
    public AudioClip shootingClip;
    public AudioClip destoyingClip;

    [Space(.25f)]
    [Header("Shooting variables")]
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public float maxShootingTime = .5f;
    public bool timeOfShootingChanged = false;

    [Space(.25f)]
    [Header("Moving variables")]
    public Transform movePoint;
    public float speed = 5;
    public float radius;
    public float stopDistance = 0.2f;
    [SerializeField]
    private float distance = .8f;

    [Space(.25f)]
    [Header("Layers variables")]
    public LayerMask whatStopsMovement;
    public LayerMask onlyObstacleMask;
    #endregion
    #region
    [Space(.25f)]
    [Header("Bonuses variables")]
    public GameObject boatGO;
    public GameObject shieldGO;

    public bool canMove = true;
    public bool turboShooting = false;
    public bool hasShield = false;
    public bool canDestroyBush = false;
    public bool canMoveOnWater = false;
    public bool canDestroyIron = false;
    #endregion
    #region
    private float currentShootingTime;
    private int health = 3;

    private GameObject bullet;

    public ParticleSystem playerParticleSystem;
    private Animator playerAnimator;
    private AudioManager audioManager;
    private DataManager dataManager;
    #endregion

    public event EventHandler<GameObject> playerDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        playerAnimator = GetComponent<Animator>();
        boatGO.SetActive(false);
        movePoint.parent = null;

        playerParticleSystem.gameObject.SetActive(true);

        dataManager = GameObject.FindObjectOfType<DataManager>();
        dataManager.LoadPlayerData(this);
    }
    // Update is called once per frame
    void Update()
    {
        // Movement
        Move();
        // Shooting
        Shoot();
    }
    void Move()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);
            playerAnimator.Play("PlayerMovement");

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
    }
    private bool CanMove(Vector3 point)
    {
        if (canMoveOnWater)
            return !Physics2D.OverlapCircle(movePoint.position + point, radius, onlyObstacleMask);
        else
            return !Physics2D.OverlapCircle(movePoint.position + point, radius, whatStopsMovement);
    }
    private void Shoot()
    {
        currentShootingTime += Time.deltaTime;
        if (bullet == null && Input.GetButton("Jump") && currentShootingTime >= .25f)
        {
            audioManager.PlaySound(SoundName.PlayerShooting);
            currentShootingTime = 0;
            bullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
            bullet.GetComponent<BulletScript>().canDestroyBush = canDestroyBush;
            bullet.GetComponent<BulletScript>().canDestroyIron = canDestroyIron;
        }
        else if (turboShooting)
        {
            if (Input.GetKey(KeyCode.E) && currentShootingTime >= maxShootingTime)
            {
                audioManager.PlaySound(SoundName.PlayerShooting);
                GameObject tempBullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
                BulletScript tempBulletScript = tempBullet.GetComponent<BulletScript>();
                tempBulletScript.canDestroyBush = canDestroyBush;
                tempBulletScript.canDestroyIron = canDestroyIron;
                currentShootingTime = 0;
            }
        }
    }
    public void Damage(int damage, Vector3 rotationOfBullet, bool ironCanDestroy)
    {
        if (hasShield) return;
        audioManager.PlaySound(SoundName.DestroyingPlayer);
        health--;
        canMove = false;
        playerParticleSystem.Play();
        GetComponent<Collider2D>().enabled = false;
        playerDestroyed?.Invoke(this, gameObject);
    }
    public int GetHealth()
    {
        return health;
    }
    public IEnumerator SetActiveShield()
    {
        hasShield = true;
        shieldGO.gameObject.SetActive(true);
        yield return new WaitForSeconds(10f);
        hasShield = false;
        shieldGO.gameObject.SetActive(false);
    }

    public void ActivateBoat()
    {
        boatGO.SetActive(true);
        canMoveOnWater = true;
    }
    public void ShootingBonus()
    {
        if (turboShooting == true)
        {
            canDestroyIron = true;
        }
        else
        {
            turboShooting = true;
        }
    }
    public void CanDestroyBush()
    {
        canDestroyBush = true;
    }
}