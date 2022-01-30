using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour, IDamageable
{
    public GameObject[] briks;
    public void Damage(int damage)
    {
        foreach (var b in briks)
        {
            Destroy(b);
        }
        Destroy(gameObject);
    }
}
