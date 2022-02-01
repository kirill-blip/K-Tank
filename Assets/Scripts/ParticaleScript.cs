using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticaleScript : MonoBehaviour
{
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
