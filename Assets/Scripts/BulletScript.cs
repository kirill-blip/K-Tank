using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public int damage;
    public bool canDestroyBush = false;
    public AudioClip destroyingClip;
    public ParticleSystem bulletParticleSystem;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    internal bool canDestroyIron;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Bonus" || collision.gameObject.CompareTag("Water")) return;
        if (collision.gameObject.tag == "Bush" && !canDestroyBush)
        {
            return;
        }
        else if (canDestroyBush && collision.gameObject.tag == "Bush")
        {
            collision.GetComponent<IDamageable>().Damage(damage, transform.localEulerAngles, canDestroyIron);
            return;
        }

        if (collision.gameObject.tag == "Obstacles")
        {
            var script = collision.GetComponent<WallScript>();
            if (script == null)
            {
                PlayParticles();

                audioManager.PlaySound(SoundName.DestroyingBullet);
                Destroy(gameObject);
                return;
            }
            else if (script.isWallIron && canDestroyIron)
            {
                var damageablesBrick = collision.GetComponentsInChildren<IDamageable>();
                foreach (var damageable in damageablesBrick)
                {
                    damageable.Damage(damage, transform.localEulerAngles, canDestroyIron);
                }
                audioManager.PlaySound(SoundName.DestroyingBullet);
                Destroy(gameObject);
                return;
            }
        }

        PlayParticles();
        var damageables = collision.GetComponentsInChildren<IDamageable>();
        foreach (var damageable in damageables)
        {
            damageable.Damage(damage, transform.localEulerAngles, canDestroyIron);
        }

        audioManager.PlaySound(SoundName.DestroyingBullet);
        Destroy(gameObject);
    }
    void PlayParticles()
    {
        bulletParticleSystem.Play();
        bulletParticleSystem.GetComponent<ParticaleScript>().DestroyParticaleSystem();
        bulletParticleSystem.transform.parent = null;
    }
}
