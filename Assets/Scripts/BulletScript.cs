using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public int damage;

    public ParticleSystem particleSystem;
    private void Start()
    {
        //particleSystem.Play();
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bonus" || collision.gameObject.CompareTag("Water")) return;
        particleSystem.Play();
        particleSystem.transform.parent = null;
        particleSystem.GetComponent<ParticaleScript>().DestroyParticaleSystem();

        var damageables = collision.GetComponentsInChildren<IDamageable>();
        foreach (var damageable in damageables)
        {
            damageable.Damage(damage, transform.localEulerAngles);
        }

        //StartCoroutine(WaitForDestroy());
        Destroy(gameObject);
    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(.05f);
    }
}
