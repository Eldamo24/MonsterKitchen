using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateFila : MonoBehaviour
{
    [SerializeField] private Table[] tables;
    [SerializeField] private GameObject[] client;
    [SerializeField] private Transform instancePosition;

    private void Start()
    {
        GetTables();
    }

    public void GetTables()
    {
        tables = FindObjectsOfType<Table>();
        StartCoroutine("InstantiatePeople");
    }

    IEnumerator InstantiatePeople()
    {
        for(int i = 0; i < tables.Length; i++)
        {
            if (tables[i].IsEmpty)
            {
                int seconds = Random.Range(15, 31);
                int index = Random.Range(0, client.Length);
                Instantiate(client[index], instancePosition.position, instancePosition.rotation);
                yield return new WaitForSeconds(seconds);
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }




}
