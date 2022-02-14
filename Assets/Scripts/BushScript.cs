using UnityEngine;

public class BushScript : MonoBehaviour, IDamageable
{
    public void Damage(int damage, Vector3 rotationOfBullet)
    {
        Destroy(gameObject);
    }
}
