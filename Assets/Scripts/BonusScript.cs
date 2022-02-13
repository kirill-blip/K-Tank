using System;
using UnityEngine;

public enum BonusType
{
    shootingTime,
    stopTimeForEnemy,
    bomb,
    boat,
    shield,
    destroyBush
}

public class BonusScript : MonoBehaviour
{
    public BonusType type;
    public event EventHandler<BonusType> onBonus;

    private void Awake()
    {
        onBonus += Camera.main.GetComponent<GameManager>().GameManager_onBonus;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            onBonus?.Invoke(this, type);
            Destroy(gameObject);
        }
    }
}
