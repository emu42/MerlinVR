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
    [SerializeField] int minEnemyNumber = 1;
    [SerializeField] int enemyNumberIncreasePerLevel = 2;
    [SerializeField] int enemyNumber = 1;
    static int level = 1;
    [SerializeField] float waitSpawnTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        enemyNumber = minEnemyNumber + enemyNumberIncreasePerLevel * (level - 1);
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
                yield return new WaitForSeconds(waitSpawnTime);
                enemyCount += 1; 
            }

     
        
    }

    public void IncreaseLevel() {
        level += 1;
    }

    public void FirstLevel()
    {
        level = 1;
    }

}
