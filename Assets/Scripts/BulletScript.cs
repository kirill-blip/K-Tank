using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public int damage;
    public bool canDestroyBush = false;
    public bool canDestroyIron = false;
    public AudioClip destroyingClip;

    private Particle bulletParticle;
    private AudioManager audioManager;

    private void Awake()
    {
        bulletParticle = GetComponentInChildren<Particle>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bonus" || collision.gameObject.CompareTag("Water")) return;
        if (collision.gameObject.tag == "Bush" && !canDestroyBush) return;
        DestroyBush(collision);
        DestroyIron(collision);
        DestroySomething(collision);
    }
    private void DestroyIron(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacles")
        {
            var wallScript = collision.GetComponent<WallScript>();
            if (wallScript == null)
            {
                DestroyBullet();
                return;
            }
            else if (wallScript.isWallIron && canDestroyIron)
            {
                var damageablesBrick = collision.GetComponentsInChildren<IDamageable>();
                foreach (var damageable in damageablesBrick)
                {
                    damageable.Damage(damage, transform.localEulerAngles, canDestroyIron);
                }
                DestroyBullet();
                return;
            }
        }
    }
    private void DestroyBush(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bush" && !canDestroyBush)
            return;
        else if (canDestroyBush && collision.gameObject.tag == "Bush")
            collision.GetComponent<IDamageable>().Damage(damage, transform.localEulerAngles, canDestroyIron);
            return;
    }
    private void DestroySomething(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bush" && !canDestroyBush) return;
        if (collision.gameObject.tag == "Bush" && canDestroyBush) return;

        var damageables = collision.GetComponentsInChildren<IDamageable>();
        foreach (var damageable in damageables)
        {
            damageable.Damage(damage, transform.localEulerAngles, canDestroyIron);
        }
        DestroyBullet();
    }
    private void DestroyBullet()
    {
        bulletParticle.PlayParcile();
        audioManager.PlaySound(SoundName.DestroyingBullet);
        Destroy(gameObject);
    }
}
