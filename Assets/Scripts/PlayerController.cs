using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public float speed = 5;
    public float radius;
    public LayerMask mask;
    private Vector2 direction;
    private float horizontalInput;
    private float verticalInput;
    private bool thereIsAWall = false;
    private float currentShootingTime;
    private float maxShootingTime = .5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentShootingTime += Time.deltaTime;
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        thereIsAWall = Physics2D.OverlapCircle(transform.position, radius, mask);
        if (!thereIsAWall)
            transform.Translate(Vector2.up * speed * Time.deltaTime * verticalInput);



        if (Input.GetButtonDown("RotateToLeft"))
        {
            direction = transform.eulerAngles;

            transform.eulerAngles += new Vector3(0, 0, 90);
        }
        else if (Input.GetButtonDown("RotateToRight"))
        {

            transform.eulerAngles += new Vector3(0, 0, -90);
        }
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
}