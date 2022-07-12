using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int totalHealth;

    public GameObject deathEffect;

    public void DamageEnemy(int damageAmount)
    {
        totalHealth -= damageAmount;

        if(totalHealth <= 0)
        {
            if(deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, transform.rotation);
            }

            Destroy(gameObject);

            AudioManager.instance.PlaySFX(4);
        }

    }
}
