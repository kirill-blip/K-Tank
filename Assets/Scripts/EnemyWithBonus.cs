using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyWithBonus : Enemy
{
    public List<GameObject> bonus;
    private BonusManager bonusManager;
    protected override void Start()
    {
        base.Start();
        bonusManager = GameObject.Find("BonusManager").GetComponent<BonusManager>();
    }
    public override void Damage(int damage, Vector3 rotationOfBullet, bool ironCanDestroy)
    {
        base.Damage(damage, rotationOfBullet, ironCanDestroy);
        if (health <= 0)
            bonusManager.InstantiateBonus();
    }
}