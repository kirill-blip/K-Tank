using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float radius;
    public LayerMask mask;
    private Vector2 direction;
    private float horizontalInput;
    private float verticalInput;
    private bool thereIsAWall = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //    direction = Vector2.left;
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //    direction = Vector2.right;
        //else if (Input.GetKeyDown(KeyCode.UpArrow))
        //    direction = Vector2.up;
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //    direction = Vector2.down;
    }
}