using System.Collections;
using UnityEngine;

public class ParticaleScript : MonoBehaviour
{
    public new ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
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
