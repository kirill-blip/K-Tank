using System.Collections.Generic;
using UnityEngine;
public class EnemyWithBonus : Enemy
{
    public List<GameObject> bonus;
    public override void Damage(int damage, Vector3 rotationOfBullet)
    {
        base.Damage(damage, rotationOfBullet);
        if (health <= 0)
            RandomBonus();
    }

    private void RandomBonus()
    {
        if (GameObject.FindWithTag("Bonus") == null)
        {
            int index = Random.Range(0, bonus.Count);
            GameObject bonusGO = Instantiate(bonus[index], transform.position, bonus[index].transform.rotation);
            bonusGO.transform.position = new Vector2(Random.Range(-17, 12), Random.Range(-23, -2));
        }
    }
}