using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public int damage;
    public bool canDestroyBush = false;

    public new ParticleSystem particleSystem;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Bonus" || collision.gameObject.CompareTag("Water")) return;
        if (collision.gameObject.tag == "Bush" && !canDestroyBush)
        {
            return;
        }
        else if (canDestroyBush && collision.gameObject.tag == "Bush")
        {
            collision.GetComponent<IDamageable>().Damage(damage, transform.localEulerAngles);
            return;
        }

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
