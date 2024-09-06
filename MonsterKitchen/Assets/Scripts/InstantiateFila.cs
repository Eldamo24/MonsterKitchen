using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateFila : MonoBehaviour
{
    [SerializeField] private Table[] tables;
    [SerializeField] private GameObject client;
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
            int seconds = Random.Range(5, 20);
            Instantiate(client, instancePosition.position, instancePosition.rotation);
            yield return new WaitForSeconds(seconds);
        }
    }




}
