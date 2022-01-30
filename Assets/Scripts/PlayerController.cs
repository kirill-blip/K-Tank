using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public Transform movePoint;
    public float speed = 5;
    public float radius;
    public float dis = .8f;
    public LayerMask mask;

    public LayerMask whatStopsMovement;
    private float horizontalInput;
    private float verticalInput;
    private bool thereIsAWall = false;
    private float currentShootingTime;
    private float maxShootingTime = .5f;

    private Vector3 direction = Vector3.up;
    private bool isMoving = false;
    private Vector3 origPos;
    private Vector3 targetPos;
    private float timeToMove = .01f;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.025f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), radius, whatStopsMovement))
                {
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") + dis, 0f, 0f);
                    }
                    if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal") - dis, 0f, 0f);
                    }
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), radius, whatStopsMovement))
                {
                    if (Input.GetAxisRaw("Vertical") < 0f)
                        movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical") + dis, 0f);
                    else if (Input.GetAxisRaw("Vertical") > 0f)
                        movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical") - dis, 0f);

                }
            }
        }

        currentShootingTime += Time.deltaTime;
        //horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
        thereIsAWall = Physics2D.OverlapCircle(transform.position, radius, mask);
        //if (!thereIsAWall && !isMoving)
        //{
        //    transform.Translate(Vector2.up * speed * Time.deltaTime * verticalInput);
        //}



        //if (Input.GetButtonDown("RotateToLeft"))
        //{
        //    //direction = transform.eulerAngles;

        //    transform.eulerAngles += new Vector3(0, 0, 90);
        //}
        //else if (Input.GetButtonDown("RotateToRight"))
        //{

        //    transform.eulerAngles += new Vector3(0, 0, -90);
        //}
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
    public void Damage(int damage)
    {
        //if (tag != "Player")
        Destroy(gameObject);
    }
    private IEnumerator MoveByGrid()
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }
}