using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BonusManager : MonoBehaviour
{
    public List<GameObject> bonus;
    public List<Transform> bonusTransforms;
    public void InstantiateBonus()
    {
        var bonus = RandomBonus();
        Instantiate(RandomBonus(), bonusTransforms[Random.Range(0, bonusTransforms.Count)].transform.position, bonus.transform.rotation);
    }
    GameObject RandomBonus()
    {
        if (GameObject.FindWithTag("Bonus") == null)
        {
            int index = Random.Range(0, bonus.Count);
            return bonus[index];
        }
        else
        {
            Destroy(GameObject.FindWithTag("Bonus").gameObject);
            int index = Random.Range(0, bonus.Count);
            return bonus[index];
        }
    }
}
