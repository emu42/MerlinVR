using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Money for nothin', chicks for free
public class ChickenLogic : MonoBehaviour
{
    private static Vector3 UP = new Vector3(0, 0.2f, 0);

    private GameObject target;

    Animator animator;

    // chicken power !!!
    public float thrust = 500;

    // Update is called once per frame
    void Update()
    {
    }


    // Start is called before the first frame update
    void Start()
    {
        // find closest enemy and lock onto
        GameObject closest = null; 
        float closestDistance = 0;
        Enemy[] enemyScripts = FindObjectsOfType<Enemy>();
        foreach (Enemy enemyScript in enemyScripts) {
            GameObject enemy = enemyScript.gameObject;
            float distance = Vector3.Distance(gameObject.transform.position, enemy.transform.position);
            if (closest == null) {
                closest = enemy;
                closestDistance = distance;
            } else {
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closest = enemy;
                }

            }
        }

        animator = GetComponent<Animator>();
        target = closest;

        DoHoming();
    }

    void DoHoming() {
        if (target != null) {
            Vector3 aimVector = target.transform.position - gameObject.transform.position + 0.5f * UP;
            Quaternion newRotation = Quaternion.Euler(aimVector.x, aimVector.y, aimVector.z);
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();

            rb.AddForce(aimVector.normalized * thrust);
            gameObject.transform.LookAt(target.transform);
        } else {
            animator.SetBool("hasTarget", false);
        }

    }

    void OnCollisionStay(Collision collisionInfo) {
        Enemy enemy = collisionInfo.gameObject.GetComponent<Enemy>();
        if (enemy != null) {
            // pecking away until the enemy is dead
            enemy.ReceiveDamage(1);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DoHoming();
    }
}
