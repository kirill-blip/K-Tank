using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
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
public class BonusManager : MonoBehaviour
{
    public List<GameObject> bonus;
    public List<Transform> bonusTransforms;

    private PlayerController playerController;
    private GameManager gameManager;
    private HomeScript baseGO;


    public void Awake()
    {
        baseGO = GameObject.FindObjectOfType<HomeScript>();
        gameManager = GameObject.FindObjectOfType<GameManager>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    public void InstantiateBonus()
    {
        var bonusTemp = RandomBonus();
        GameObject bonus = Instantiate(bonusTemp, bonusTransforms[Random.Range(0, bonusTransforms.Count)].transform.position, bonusTemp.transform.rotation);
        bonus.GetComponent<Bonus>().onBonus += OnBonus;
    }
    private GameObject RandomBonus()
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
    public void OnBonus(object sender, BonusType type)
    {
        string whoIsIt = (sender as GameObject).tag;
        if (whoIsIt == "Player")
        {
            switch (type)
            {
                case BonusType.shootingTime:
                    playerController.ShootingBonus();
                    break;
                case BonusType.stopTimeForEnemy:
                    StartCoroutine(StopEnemy());
                    break;
                case BonusType.bomb:
                    DestroyEnemies();
                    break;
                case BonusType.boat:
                    playerController.ActivateBoat();
                    break;
                case BonusType.shield:
                    StartCoroutine(playerController.SetActiveShield());
                    break;
                case BonusType.destroyBush:
                    playerController.CanDestroyBush();
                    break;
                case BonusType.ironBonus:
                    baseGO.GetComponent<HomeScript>().ChangeWall();
                    break;
            }
        }
        else if (whoIsIt == "Enemy")
        {
            switch (type)
            {
                case BonusType.bomb:
                    playerController.Damage(1, Vector3.zero, false);
                    break;
            }
        }
    }
    IEnumerator StopEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        gameManager.isStopped = true;
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().StopTank();
        }
        yield return new WaitForSeconds(15f);

        gameManager.isStopped = false;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().StartTank();
        }
    }

    void DestroyEnemies()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().DestroyTank();
            Debug.Log("Enemy destoy");
        }
    }
}
