using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ClientController : MonoBehaviour
{
    [SerializeField] private Transform waitForTablePosition;
    [SerializeField] private bool entering;
    [SerializeField] private bool waitForATable;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text temporizador;
    private bool waitForOrder;
    private Transform chairPosition;
    private bool canInteract = true;
    private bool collisioning = false;

    private int timeToWaitTable;
    private int timeToWaitToOrder;
    private int waitToOrder = 0;
    private float speed = 3f;
    public bool WaitForOrder { get => waitForOrder; set => waitForOrder = value; }
    public TMP_Text Text { get => text; set => text = value; }
    public int WaitToOrder { get => waitToOrder; set => waitToOrder = value; }

    private void OnEnable()
    {
        waitForATable = false;
        entering = true;
        waitForTablePosition = GameObject.Find("WaitForATable").GetComponent<Transform>();
        text = GameObject.Find("Time").GetComponent<TMP_Text>();
        temporizador = GetComponentInChildren<Canvas>().GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (entering)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, waitForTablePosition.position, step);
        }
        if (!waitForATable && !canInteract)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, chairPosition.position, step);
        }
    }

    private void Update()
    {
        if (canInteract && timeToWaitTable > 0 && collisioning)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                canInteract = false;
                timeToWaitTable = 0;
                StopCoroutine("WaitingTable");
                temporizador.text = "";
                Table[] tables = FindObjectsOfType<Table>();
                foreach (Table table in tables)
                {
                    if (table.IsEmpty)
                    {
                        table.IsEmpty = false;
                        waitForATable = false;
                        chairPosition = table.gameObject.GetComponentInChildren<Transform>();
                        break;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "WaitForATable")
        {
            entering = false;
            if (canInteract)
            {
                waitForATable = true;
                StartCoroutine("WaitingTable");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canInteract)
        {
            text.text ="E to interact";
        } 
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collisioning = true;
            //if (timeToWaitTable > 0 && Input.GetKeyDown(KeyCode.E) && canInteract)
            //{
                
            //    canInteract = false;
            //    timeToWaitTable = 0;
            //    StopCoroutine("WaitingTable");
            //    temporizador.text = "";
            //    Table[] tables = FindObjectsOfType<Table>();
            //    foreach (Table table in tables)
            //    {
            //        if (table.IsEmpty)
            //        {
            //            table.IsEmpty = false;
            //            waitForATable = false;
            //            chairPosition = table.gameObject.GetComponentInChildren<Transform>();
            //            break;
            //        }
            //    }
            //}
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            text.text = "";
            collisioning = false;
        }
    }

    IEnumerator WaitingTable()
    {
        timeToWaitTable = Random.Range(15, 31);
        while(waitForATable && timeToWaitTable > 0)
        {
            temporizador.text = timeToWaitTable.ToString();
            yield return new WaitForSeconds(1f);
            timeToWaitTable--;
        }
        if(timeToWaitTable <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public IEnumerator WaitingToOrder()
    {
        timeToWaitToOrder = Random.Range(5,11);
        while(timeToWaitToOrder > 0)
        {
            yield return new WaitForSeconds(1f);
            timeToWaitToOrder--;
        }
        waitToOrder = Random.Range(15, 31);
        while (waitToOrder > 0)
        {
            temporizador.text = waitToOrder.ToString();
            yield return new WaitForSeconds(1f);
            waitToOrder--;
        }
        if(waitToOrder <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator WaitToEat()
    {
        int waitFood = Random.Range(60, 81);
        while (waitFood > 0)
        {
            text.text = waitFood.ToString();
            yield return new WaitForSeconds(1f);
            waitFood--;
        }
        if (waitFood <= 0)
        {
            Destroy(gameObject);
        }
    }
}
