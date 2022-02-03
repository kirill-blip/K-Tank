using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    #region

    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public Transform movePoint;
    public float speed = 5;
    public float radius;
    public float distance = 0.2f;
    public LayerMask mask;

    public LayerMask whatStopsMovement;

    private float currentShootingTime;
    private float maxShootingTime = .5f;

    public event EventHandler<GameObject> playerDestroyed;

    [SerializeField]
    private float dis = .8f;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }
    // Update is called once per frame
    void Update()
    {
        // Movement
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) == distance)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    dis = Mathf.Abs(dis);
                    transform.eulerAngles = new Vector3(0f, 0f, 90f);
                }
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    dis = -Mathf.Abs(dis);
                    transform.eulerAngles = new Vector3(0f, 0f, -90f);
                }
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal") + dis, 0f, 0f), radius, whatStopsMovement))
                {

                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") + dis, 0f, 0f);
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    dis = Mathf.Abs(dis);
                    transform.eulerAngles = new Vector3(0f, 0f, 180f);
                }
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    dis = -Mathf.Abs(dis);
                    transform.eulerAngles = Vector3.zero;
                }
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical") + dis, 0f), radius, whatStopsMovement))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical") + dis, 0f);
                }
            }
        }



        // Shooting
        currentShootingTime += Time.deltaTime;

        if (Input.GetButton("Jump") && currentShootingTime >= maxShootingTime)
        {
            Shoot();
            currentShootingTime = 0;
        }
    }
    void Shoot()
    {
        GameObject go = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
    }
    public void Damage(int damage, Vector3 rotationOfBullet)
    {
        playerDestroyed?.Invoke(this, gameObject);
        Destroy(gameObject);
    }
}