﻿using System.Collections.Generic;
using UnityEngine;
public class EnemyWithBonus : Enemy
{
    public List<GameObject> bonus;
    public override void Damage(int damage, Vector3 rotationOfBullet)
    {
        base.Damage(damage, rotationOfBullet);
        RandomBonus();
    }
    public override void DestroyTank()
    {
        base.DestroyTank();
        RandomBonus();
    }

    private void RandomBonus()
    {
        int index = Random.Range(0, bonus.Count);
        Instantiate(bonus[index], transform.position, bonus[index].transform.rotation);
    }
}