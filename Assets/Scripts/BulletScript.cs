using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public int damage;
    public string tagString;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if(collision.tag == tagString) Destroy(collision.gameObject);


        var damageables = collision.GetComponentsInChildren<IDamageable>();
        foreach (var damageable in damageables)
        {
            damageable.Damage(damage);
        }
        Destroy(gameObject);
    }
}
