using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HomeScript : MonoBehaviour, IDamageable
{
    public event EventHandler<GameObject> homeDestroyed;
    public Sprite ironSprite;
    public Sprite brickSprite;
    public List<GameObject> walls;
    // Start is called before the first frame update
    void Start()
    {
        var wallsSpcrits = transform.parent.GetComponentsInChildren<WallScript>();
        foreach (var wallScript in wallsSpcrits)
        {
            walls.Add(wallScript.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    { }

    public void Damage(int damage, Vector3 rotationOfBullet)
    {
        homeDestroyed?.Invoke(this, this.gameObject);
    }

    public void ChangeWall()
    {
        StartCoroutine(WaitForChange());
    }

    IEnumerator WaitForChange()
    {
        foreach (var wall in walls)
        {
            ChangeSprite(wall, ironSprite);
            wall.GetComponent<WallScript>().isWallIron = true;
        }

        yield return new WaitForSeconds(15f);

        foreach (var wall in walls)
        {
            ChangeSprite(wall, brickSprite);
            wall.GetComponent<WallScript>().isWallIron = false;
        }
    }

    void ChangeSprite(GameObject wall, Sprite sprite)
    {
        wall.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
