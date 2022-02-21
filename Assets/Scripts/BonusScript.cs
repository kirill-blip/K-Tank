using System;
using UnityEngine;
using System.Collections;

public enum BonusType
{
    shootingTime,
    stopTimeForEnemy,
    bomb,
    boat,
    shield,
    destroyBush,
    ironBonus
}

public class BonusScript : MonoBehaviour
{
    public BonusType type;
    public event EventHandler<BonusType> onBonus;

    private void Awake()
    {
        onBonus += Camera.main.GetComponent<GameManager>().GameManager_onBonus;
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
    IEnumerator DeleteBonus()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }

}
