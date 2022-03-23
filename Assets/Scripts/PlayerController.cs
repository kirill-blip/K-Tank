using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private bool turboShooting = false;
    private bool hasShield = false;
    private bool canDestroyBush = false;
    private bool canMoveOnWater = false;
    private bool canDestroyIron = false;
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

    private PlayerControls controls;
    private Vector2 move;

    public event EventHandler<GameObject> playerDamaged;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += context => move = context.ReadValue<Vector2>();
        controls.Player.Move.canceled += context => move = Vector2.zero;
    }
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
    void Update()
    {
        // Movement
        MoveInputSystem();
        // Shooting
        currentShootingTime += Time.deltaTime;
        TurboShoot();
        DefaultShoot();
    }
    private void OnEnable()
    {
        controls.Player.Enable();
    }
    private void OnDisable()
    {
        controls.Player.Disable();
    }
    private void MoveInputManager()
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
    private void MoveInputSystem()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);
            playerAnimator.Play("PlayerMovement");

            if (Vector3.Distance(transform.position, movePoint.position) == stopDistance)
            {
                if (Mathf.Abs(move.x) == 1)
                {
                    CheckAndRotatePlayer(Vector2.left, Vector2.right, new Vector3(0f, 0f, 90f), new Vector3(0f, 0f, -90f));
                    if (CanMove(new Vector3(move.x, 0f, 0f)))
                        movePoint.position += new Vector3(move.x, 0f, 0f);
                }
                else if (Mathf.Abs(move.y) == 1)
                {
                    CheckAndRotatePlayer(Vector2.down, Vector2.up, new Vector3(0f, 0f, 180f), Vector3.zero);
                    if (CanMove(new Vector3(0, move.y, 0f)))
                        movePoint.position += new Vector3(0f, move.y, 0f);
                }
            }
        }
    }
    private void CheckAndRotatePlayer(Vector2 firstDirection, Vector2 secondDirection, Vector3 firstEulerAngels, Vector3 secondEulerAngels)
    {
        if (move == firstDirection)
            transform.eulerAngles = firstEulerAngels;
        if (move == secondDirection)
            transform.eulerAngles = secondEulerAngels;
    }
    private bool CanMove(Vector3 point)
    {
        if (canMoveOnWater)
            return !Physics2D.OverlapCircle(movePoint.position + point, radius, onlyObstacleMask);
        else
            return !Physics2D.OverlapCircle(movePoint.position + point, radius, whatStopsMovement);
    }
    private void DefaultShoot()
    {
        if (bullet == null && Input.GetKey(KeyCode.Space) && currentShootingTime >= maxShootingTime)
        {
            audioManager.PlaySound(SoundName.PlayerShooting);
            bullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
            bullet.GetComponent<BulletScript>().canDestroyBush = canDestroyBush;
            bullet.GetComponent<BulletScript>().canDestroyIron = canDestroyIron;
            currentShootingTime = 0;
        }
    }
    private void TurboShoot()
    {
        if (turboShooting && Input.GetKey(KeyCode.E) && currentShootingTime >= maxShootingTime)
        {
            audioManager.PlaySound(SoundName.PlayerShooting);
            GameObject tempBullet = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
            BulletScript tempBulletScript = tempBullet.GetComponent<BulletScript>();
            tempBulletScript.canDestroyBush = canDestroyBush;
            tempBulletScript.canDestroyIron = canDestroyIron;
            currentShootingTime = 0;
        }
    }
    public void Damage(int damage, Vector3 rotationOfBullet, bool ironCanDestroy)
    {
        if (hasShield) return;
        if (boatGO.activeInHierarchy)
        {
            ActivateBoat();
            return;
        }
        audioManager.PlaySound(SoundName.DestroyingPlayer);
        health--;
        canMove = false;
        playerParticleSystem.Play();
        GetComponent<Collider2D>().enabled = false;
        playerDamaged?.Invoke(this, gameObject);
    }
    public void BombDestroying()
    {
        audioManager.PlaySound(SoundName.DestroyingPlayer);
        health--;
        canMove = false;
        playerParticleSystem.Play();
        GetComponent<Collider2D>().enabled = false;
        playerDamaged?.Invoke(this, gameObject);
    }
    public int GetHealth()
    {
        return health;
    }
    public void ActivateBoat()
    {
        boatGO.SetActive(!boatGO.activeInHierarchy);
        canMoveOnWater = !canMoveOnWater;
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
    public void SetActiveDestoyBush()
    {
        canDestroyBush = true;
    }

    public bool CanDestroyBush()
    {
        return canDestroyBush;
    }
    public bool CanDestroyIron()
    {
        return canDestroyIron;
    }
    public bool CanMoveOnWater()
    {
        return canMoveOnWater;
    }
    public bool HaveTurboShooting()
    {
        return turboShooting;
    }
    public IEnumerator SetActiveShield(float shieldTime)
    {
        hasShield = true;
        shieldGO.gameObject.SetActive(true);
        yield return new WaitForSeconds(shieldTime);
        hasShield = false;
        shieldGO.gameObject.SetActive(false);
    }
}