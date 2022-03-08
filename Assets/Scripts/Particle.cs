using System.Collections;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public ParticleSystem particle;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void PlayParcile()
    {
        transform.parent = null;
        particle.Play();
        DestroyParticaleSystem();
    }

    public void DestroyParticaleSystem()
    {
        StartCoroutine(WaitForDestroy());
    }
    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(.5f);
        GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(.25f);
        Destroy(gameObject);
    }
}
