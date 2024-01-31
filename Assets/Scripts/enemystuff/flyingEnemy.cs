using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingEnemy : MonoBehaviour
{
    public float speed;
    private Transform player;
    public float lineofSite;
    public float shootingRange;
    public GameObject bullet;
    public GameObject bulletParent;
    public float fireRate = 1f;
    private float nextFireTime;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, this.transform.position);
        if (distanceFromPlayer < lineofSite && distanceFromPlayer > shootingRange)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, speed * Time.deltaTime);
        }
        else if (distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
        {
            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, lineofSite);
        Gizmos.DrawWireSphere(this.transform.position, shootingRange);
    }



}

