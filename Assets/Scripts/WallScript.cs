using UnityEngine;

public class WallScript : MonoBehaviour, IDamageable
{
    public bool isWallIron;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Damage(int damage, Vector3 rotationOfBullet, bool ironCanDestroy)
    {
        if (isWallIron && ironCanDestroy == false) return;
        //Vector3 rotation = collision2D.gameObject.transform.localEulerAngles;
        if (rotationOfBullet == new Vector3(0, 0, 180) || rotationOfBullet == new Vector3(0, 0, 0))
        {
            transform.localScale -= new Vector3(0, 0.25f, 0);

            if (rotationOfBullet == new Vector3(0, 0, 180))
                transform.position -= new Vector3(0, 0.125f, 0);
            if (rotationOfBullet == new Vector3(0, 0, 0))
            {
                spriteRenderer.material.mainTextureOffset += new Vector2(0, 0.125f);
                transform.position += new Vector3(0, 0.125f, 0);
            }
        }

        if (rotationOfBullet == new Vector3(0, 0, 270) || rotationOfBullet == new Vector3(0, 0, 90))
        {
            transform.localScale -= new Vector3(0.25f, 0, 0);

            if (rotationOfBullet == new Vector3(0, 0, 270))
                transform.position += new Vector3(0.125f, 0, 0);
            if (rotationOfBullet == new Vector3(0, 0, 90))
                transform.position -= new Vector3(0.125f, 0, 0);
        }
    }
}
