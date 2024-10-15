using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

public class EnemySpawn : MonoBehaviour
{
    public int numEnemies = 1;
    public GameObject prefab;
    Random rand = new Random();
    [SerializeField] List<GameObject> SpawnPoints;
    [SerializeField] bool randomised;


    // Now can make any number of enemies in 1 frame
    void MakeAChild(int totalEnemies)
    {
        for (int i = 0; i < totalEnemies; i++)
        {
            Transform SpawnLocation;
            SpawnPointData SpawnObj;

            if (SpawnPoints.Count > 0 && randomised == true)
            {
                SpawnLocation = SpawnPoints[rand.Next(SpawnPoints.Count)].transform;
            } else if (SpawnPoints.Count > 0 && i <= SpawnPoints.Count-1)
            {
                SpawnLocation = SpawnPoints[i].transform;
            }
            else
            {
                SpawnLocation = transform;
            }
            SpawnObj = SpawnLocation.gameObject.GetComponent<SpawnPointData>();
            if (SpawnObj != null)
            {
                if (SpawnObj.respawn == true || SpawnObj.alive == true)
                {
                    GameObject newChild = Instantiate(SpawnObj.prefab, SpawnLocation.position, Quaternion.identity) as GameObject;
                    Target enemy = newChild.GetComponent<Target>();
                    //stores enemy's spawn point's data in the enemy
                    enemy.spawn = SpawnObj;
                    enemy.target = SpawnLocation.position + new Vector3(0.001f, 0, 0);
                    //Make the child's parent the spawn area
                    newChild.transform.parent = transform;
                }
            } else if (SpawnLocation == transform)
            {
                GameObject newChild = Instantiate(prefab, SpawnLocation.position, Quaternion.identity) as GameObject;
                newChild.transform.parent = transform;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //When Player exits trigger, kills all children
        if (other.transform.root.CompareTag("Player"))
        {
            for (int c = 0; c < transform.childCount; c++)
            {
                Destroy(transform.GetChild(c).gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.CompareTag("Player"))
        {
            MakeAChild(numEnemies);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            List<Component> ais = new List<Component>();
            ais.AddRange(GetComponentsInChildren<Target>());
            Vector3 diff = other.transform.position - transform.position;
            if ((diff.x < 15.5 && diff.x > -15.5) && (diff.z < 15.5 && diff.z > -15.5))
            {
                foreach (Target ai in ais)
                {
                    ai.target = other.transform.position;
                    ai.pTarget = true;
                }
            }
            else
            {
                foreach (Target ai in ais)
                {
                    ai.target = ai.spawn.transform.position;
                    ai.health = ai.maxhealth;
                    ai.pTarget = false;
                }
            }
        }
    }
}
