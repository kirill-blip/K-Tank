using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public int damage;
    public bool canDestroyBush = false;

    public ParticleSystem particleSystem;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bonus" || collision.gameObject.CompareTag("Water")) return;
        if (collision.gameObject.tag == "Bush" && !canDestroyBush) return;
        particleSystem.Play();
        particleSystem.transform.parent = null;
        particleSystem.GetComponent<ParticaleScript>().DestroyParticaleSystem();

        var damageables = collision.GetComponentsInChildren<IDamageable>();
        foreach (var damageable in damageables)
        {
            damageable.Damage(damage, transform.localEulerAngles);
        }

        Destroy(gameObject);
    }
}
