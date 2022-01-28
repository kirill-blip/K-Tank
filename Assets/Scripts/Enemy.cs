using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
public class Enemy : MonoBehaviour
{
    public float speed = 5;
    public LayerMask mask;
    private Vector3[] rotations = { new Vector3(0, 0, 0), new Vector3(0, 0, 90), new Vector3(0, 0, -90), new Vector3(0, 0, 180) };
    private Vector2[] dir = { Vector2.up, Vector2.left, Vector2.right, Vector2.down };
    private Vector2 currentDir;
    private Vector3 currentRotation;

    public GameObject bulletPrefab;
    public Transform bulletTransform;
    private AIDestinationSetter setter;
    private AIPath path;
    private Seeker seeker;
    private Transform currentTargetTransform;
    public List<Transform> target;
    private void Start()
    {
        currentRotation = rotations[1];
        currentDir = dir[1];
        path = GetComponent<AIPath>();
        setter = GetComponent<AIDestinationSetter>();
        seeker = GetComponent<Seeker>();
        //currentTargetTransform = target[Random.Range(0, target.Count)];
        //setter.target = currentTargetTransform;
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
        if (!Physics2D.Raycast(transform.position, currentDir, 1, mask))
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else if (Physics2D.Raycast(transform.position, currentDir, 1, mask))
        {
            index = Random.Range(0, rotations.Length);
            currentDir = dir[index];
            currentRotation = rotations[index];
            transform.eulerAngles = currentRotation;
        }

        currentTime += Time.deltaTime;
        if (currentTime >= maxTime)
        {
            Shoot();
            currentTime = 0;
        }
        Debug.Log(path.velocity);
        //if (path.velocity.magnitude == 0)
        //    path.target = target[Random.Range(0, target.Count)];
        //Debug.Log(path.velocity.magnitude);
        //if (path.velocity.x > 0.5f)
        //    transform.eulerAngles = new Vector3(0, 0, -90);
        //if (path.velocity.x < -0.5f)
        //    transform.eulerAngles = new Vector3(0, 0, 90);

        //if (path.velocity.y > 0.5f)
        //    transform.eulerAngles = new Vector3(0, 0, 0);
        //if (path.velocity.y < -0.5f)
        //    transform.eulerAngles = new Vector3(0, 0, 180);
    }

    void Shoot()
    {
        GameObject go = Instantiate(bulletPrefab, bulletTransform.position, bulletTransform.rotation);
    }
}
