using UnityEngine;
using Pathfinding;
using System.Collections.Generic;
public class Enemy : MonoBehaviour
{
    private AIDestinationSetter setter;
    private AIPath path;
    private Seeker seeker;
    private Transform currentTargetTransform;
    public List<Transform> target;
    private void Start()
    {
        path = GetComponent<AIPath>();
        setter = GetComponent<AIDestinationSetter>();
        seeker = GetComponent<Seeker>();
        currentTargetTransform = target[Random.Range(0, target.Count)];
        setter.target = currentTargetTransform;
    }
    private void Update()
    {
        Debug.Log(path.velocity);
        if (path.velocity.magnitude == 0)
            path.target = target[Random.Range(0, target.Count)];
        Debug.Log(path.velocity.magnitude);
        //if (path.velocity.x > 0.5f)
        //    transform.eulerAngles = new Vector3(0, 0, -90);
        //if (path.velocity.x < -0.5f)
        //    transform.eulerAngles = new Vector3(0, 0, 90);

        //if (path.velocity.y > 0.5f)
        //    transform.eulerAngles = new Vector3(0, 0, 0);
        //if (path.velocity.y < -0.5f)
        //    transform.eulerAngles = new Vector3(0, 0, 180);
    }
}
