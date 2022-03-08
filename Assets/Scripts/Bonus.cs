using System;
using UnityEngine;
using System.Collections;

public class Bonus : MonoBehaviour
{
    public BonusType type;
    public event EventHandler<BonusType> onBonus;

    public bool eventHandler;

    private void Awake()
    {
#if UNITY_EDITOR
        if (eventHandler)
            onBonus += GameObject.FindObjectOfType<BonusManager>().OnBonus;
#endif
        StartCoroutine(DeleteBonus());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            DestoyBonus(collision);
        if (collision.gameObject.CompareTag("Enemy") && type == BonusType.bomb)
            DestoyBonus(collision);
    }

    void DestoyBonus(Collider2D collision)
    {
        onBonus?.Invoke(collision.gameObject, type);
        Destroy(gameObject);
    }
    private IEnumerator DeleteBonus()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }

}
