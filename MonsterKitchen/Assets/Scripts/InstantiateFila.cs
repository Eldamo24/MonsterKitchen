using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InstantiateFila : MonoBehaviour
{
    [SerializeField] private Table[] tables;
    [SerializeField] private GameObject[] client;
    [SerializeField] private Transform instancePosition;
    [SerializeField] private TMP_Text chancesOnScreen;

    private int chances = 5;
    public int Chances { get => chances; set => chances = value; }


    private void Update()
    {
        chancesOnScreen.text = "Chances: " + chances.ToString();
        if (chances <= 0)
        {
            //Volver al menu
        }
    }

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
                int seconds = Random.Range(45, 61);
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
