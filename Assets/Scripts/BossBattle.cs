using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{

    private CameraController theCam;
    public Transform camPosition;
    public float camSpeed;

    public int trashold1, trashold2;

    public float activeTime, fadeTime, inactiveTime;
    private float activeCounter, fadeCounter, inactiveCounter;

    public Transform[] spawnPoints;
    private Transform targetPoint;
    public float moveSpeed;

    public Animator anim;

    public Transform theBoss;

    public float timeBetweenShot1, timeBetweenShot2;

    private float shotCounter;
    public GameObject bullet;
    public Transform shotPoint;

    public GameObject winObjects;

    private bool batleEnd;


    void Start()
    {
        theCam = FindObjectOfType<CameraController>();
        theCam.enabled = false;

        activeCounter = activeTime;

        shotCounter = timeBetweenShot1;

        AudioManager.instance.PlayBossMusic();
    }


    void Update()
    {
   

        theCam.transform.position = Vector3.MoveTowards(theCam.transform.position, camPosition.position, camSpeed * Time.deltaTime);

        if (!batleEnd)
        {
            ObservePlayer();

            if (BossHealthController.instance.currentHealth > trashold1)
            {


                if (activeCounter > 0)
                {
                    activeCounter -= Time.deltaTime;
                    if (activeCounter <= 0)
                    {
                        fadeCounter = fadeTime;
                        anim.SetTrigger("vanish");
                    }

                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        shotCounter = timeBetweenShot1;
                        Instantiate(bullet, shotPoint.position, Quaternion.identity);
                    }

                }
                else if (fadeCounter > 0)
                {
                    fadeCounter -= Time.deltaTime;
                    if (fadeCounter <= 0)
                    {
                        theBoss.gameObject.SetActive(false);
                        inactiveCounter = inactiveTime;
                    }
                }
                else if (inactiveCounter > 0)
                {
                    inactiveCounter -= Time.deltaTime;
                    if (inactiveCounter <= 0)
                    {
                        theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                        theBoss.gameObject.SetActive(true);

                        activeCounter = activeTime;

                        shotCounter = timeBetweenShot1;

                    }
                }

            }
            else
            {
                if (targetPoint == null)
                {
                    targetPoint = theBoss;
                    fadeCounter = fadeTime;
                    anim.SetTrigger("vanish");
                }
                else
                {
                    if (Vector3.Distance(theBoss.position, targetPoint.position) > 0.02f)
                    {
                        anim.SetTrigger("motion");
                        theBoss.position = Vector3.MoveTowards(theBoss.position, targetPoint.position, moveSpeed * Time.deltaTime);


                        if (Vector3.Distance(theBoss.position, targetPoint.position) <= 0.02f)
                        {
                            fadeCounter = fadeTime;
                            anim.SetTrigger("vanish");
                        }

                        shotCounter -= Time.deltaTime;
                        if (shotCounter <= 0)
                        {
                            if (PlayerHealthController.instance.currentHealth > trashold2)
                            {
                                shotCounter = timeBetweenShot1;
                            }
                            else
                            {
                                shotCounter = timeBetweenShot2;
                            }

                            Instantiate(bullet, shotPoint.position, Quaternion.identity);
                        }

                    }
                    else if (fadeCounter > 0)
                    {
                        fadeCounter -= Time.deltaTime;
                        if (fadeCounter <= 0)
                        {
                            theBoss.gameObject.SetActive(false);
                            inactiveCounter = inactiveTime;
                        }
                    }
                    else if (inactiveCounter > 0)
                    {
                        inactiveCounter -= Time.deltaTime;
                        if (inactiveCounter <= 0)
                        {
                            theBoss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

                            targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                            int whileBreaker = 0;
                            while (targetPoint.position == theBoss.position && whileBreaker < 100)
                            {
                                targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                                whileBreaker++;
                            }

                            theBoss.gameObject.SetActive(true);

                            if (PlayerHealthController.instance.currentHealth > trashold2)
                            {
                                shotCounter = timeBetweenShot1;
                            }
                            else
                            {
                                shotCounter = timeBetweenShot2;
                            }

                        }
                    }

                }
            }
        }
        else
        {
            fadeCounter -= Time.deltaTime;
            if(fadeCounter < 0)
            {
                if(winObjects != null)
                {
                    winObjects.SetActive(true); //активировать
                    winObjects.transform.SetParent(null); //все обьекты winObjects лишаются своего родителя 
                }

                theCam.enabled = true;

                gameObject.SetActive(false);

                AudioManager.instance.PlayLevelMusic();
            }

        }



    }

    public void EndBattle()
    {
        batleEnd = true;
        fadeCounter = fadeTime;
        anim.SetTrigger("vanish");
        theBoss.GetComponent<Collider2D>().enabled = false;

        BossBullet[] bullets = FindObjectsOfType<BossBullet>();
        if(bullets.Length > 0)
        {
            foreach (BossBullet bullet in bullets)
            {
                Destroy(bullet.gameObject);
            }
        }
    }


    public void ObservePlayer()
    {
        float observPos;
        observPos = theBoss.transform.position.x - PlayerHealthController.instance.transform.position.x;
        if (observPos < 0)
        {
            theBoss.transform.localScale = new Vector2(-1f, 1f);
        }
        else 
        {
            theBoss.transform.localScale = Vector2.one;
        }
    }

}
