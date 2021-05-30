using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemy : MonoBehaviour
{

    [SerializeField] GameObject spawnEnemy;
    [SerializeField] int xPos;
    [SerializeField] int zPos;
    [SerializeField] int yPos;
    [SerializeField] int enemyCount;
    [SerializeField] int enemyNumber = 10;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop()); 
    }


        IEnumerator EnemyDrop()
        {
            while (enemyCount < enemyNumber)
            {
            xPos = Random.Range(-99, 84);
            yPos = Random.Range(-2, 0);
            zPos = Random.Range(-49, 74);
                Instantiate(spawnEnemy, new Vector3(xPos, yPos, zPos), Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
                enemyCount += 1; 
            }

     
        
    }

 
}
