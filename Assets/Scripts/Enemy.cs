using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Enemy : MonoBehaviour
{
    public float lookRadius = 10f;
    Transform target;
    NavMeshAgent agent;

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
            }
        }

    }
    // Update is called once per frame


    void FaceTarget()
    {
        anim.SetBool("running", false);
        anim.SetInteger("condition", 0);
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void GetInput()
    {
        // if gestenattacke, Gegenattacke


    }

    void Attacking()
    {
        anim.SetBool("attacking", true);
        anim.SetInteger("condition", 2);


    }
}