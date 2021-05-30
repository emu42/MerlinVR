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

    float timeNextTillAttack = 3.0f;

    public int damagePerAttack = 10;

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
            anim.SetBool("alive", true);
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
        
        // attack in fixed intervals
        timeNextTillAttack -= Time.deltaTime;
        if (timeNextTillAttack <= 0f) 
        {
            DealDamage();
            timeNextTillAttack = 3f;
        }
    }

    void DealDamage()
    {
        PlayerLogic[] playerScripts = FindObjectsOfType<PlayerLogic>();
        foreach (PlayerLogic playerScript in playerScripts)
        {
            playerScript.ReceiveDamage(damagePerAttack);
            
            if (anim.GetBool("alive") == true)
            {
                FindObjectOfType<AudioManager>().Play("EnemyAttack");
            }
    }

    void Die()
    {
        // TODO animate 
        anim.SetInteger("condition", 3);
       
        Destroy(gameObject, 5f);
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        int damageCaused;

        damageCaused = (int)Math.Round(collision.impulse.magnitude);
        Attacking();

        ReceiveDamage(damageCaused);
        anim.SetInteger("condition", 4);
    }

    public void ReceiveDamage(int amount)
    {
        Debug.Log("mob received damage: " + amount);
        health = Math.Max(0, health - amount);

        if (health == 0)
            {
                anim.SetBool("alive", false);
                FindObjectOfType<AudioManager>().Play("EnemyDie");
                Die();
            
        }
    }
}


   