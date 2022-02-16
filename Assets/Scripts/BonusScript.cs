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
        {
            onBonus?.Invoke(this, type);
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            transform.position = new Vector2(UnityEngine.Random.Range(-17, 12), UnityEngine.Random.Range(-23, -2));
        }
    }

    IEnumerator DeleteBonus()
    {
        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }

}
