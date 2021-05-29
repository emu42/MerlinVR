using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Enemy : MonoBehaviour
{
    public float lookRadius = 10f;

    public int health = 50;

    Transform target;
    NavMeshAgent agent;

    [SerializeField] private float walkSpeed; 

    Animator anim; 


    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }


    void Update()
    {

        Movement();
        GetInput();
    }


    void Movement()
    {

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            anim.SetBool("running", true);
            anim.SetInteger("condition", 1);
            agent.SetDestination(target.position);

           
            if (distance <= agent.stoppingDistance)
            {

                FaceTarget();
                Attacking();
            }
        }

    }
    // Update is called once per frame


    void FaceTarget()
    {
        anim.SetBool("running", false);
        anim.SetInteger("condition", 0);

        transform.LookAt(target.transform);
       /* Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation; */
     

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void GetInput()
    {

        /* // if gestenattacke, Gegenattacke
         
        if (anim.GetBool("running") == false)
        {
            if (anim.GetBool("running") == true)
            {
                anim.SetBool("running", false);
                anim.SetInteger("condition", 0);
            }
            if (anim.GetBool("running") == false)
            {
                Attacking();
            }
        } 
        
         */

    }

    void Attacking()
    {
        anim.SetBool("attacking", true);
        anim.SetInteger("condition", 2);


    }

    void Die()
    {
        // TODO animate 
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        int damageCaused;

        damageCaused = (int)Math.Round(collision.impulse.magnitude);

        ReceiveDamage(damageCaused);    
    }

    public void ReceiveDamage(int amount)
    {
        Debug.Log("mob received damage: " + amount);
        health = Math.Max(0, health - amount);

        if (health == 0)
        {
            Die();
        }
    }
}