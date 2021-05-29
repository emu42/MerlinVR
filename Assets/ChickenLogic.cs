using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Money for nothin', chicks for free
public class ChickenLogic : MonoBehaviour
{
    private static Vector3 UP = new Vector3(0, 0.2f, 0);

    private GameObject target;

    Animator animator;

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
            Vector3 aimVector = target.transform.position - gameObject.transform.position;
            gameObject.transform.rotation = Quaternion.Euler(aimVector.x, aimVector.y, aimVector.z);
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.velocity = rb.velocity + aimVector.normalized * 0.1f + UP;
        } else {
            animator.SetBool("hasTarget", false);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DoHoming();
    }
}
