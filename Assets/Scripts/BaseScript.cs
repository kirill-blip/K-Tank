using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BaseScript : MonoBehaviour, IDamageable
{
    public event EventHandler<GameObject> baseDestroyed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage(int damage, Vector3 rotationOfBullet)
    {
        //if (tag != "Player")
        baseDestroyed?.Invoke(this, this.gameObject);
        //Destroy(gameObject);
    }
}
