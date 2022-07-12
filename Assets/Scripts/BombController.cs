using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float timeToExplode;
    public GameObject explosion;
    public GameObject impactEffect;

    public float blastRange;
    public LayerMask whatisDestructible;

    public int damageAmount;
    public LayerMask whatIsDamageable;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Bomb
        timeToExplode -= Time.deltaTime;
        if(timeToExplode < 0)
        {
            if(explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }

            Destroy(gameObject);

            //Destroy Bomb
            Collider2D[] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, blastRange, whatisDestructible);

            if(objectsToRemove.Length > 0)
            {
                foreach (Collider2D obj in objectsToRemove)
                {
                    
                    Destroy(obj.gameObject);
                    Instantiate(impactEffect, obj.transform.position, Quaternion.identity);

                }    
            }

            Collider2D[] objectsToDamage = Physics2D.OverlapCircleAll(transform.position, blastRange, whatIsDamageable);

            foreach(Collider2D col in objectsToDamage)
            {
                EnemyHealthController enemyHealth = col.GetComponent<EnemyHealthController>();
                if(enemyHealth != null)
                {
                    
                    enemyHealth.DamageEnemy(damageAmount);
                }
            }
            AudioManager.instance.PlaySFXAdjusted(4);
        }

       

    }
}
