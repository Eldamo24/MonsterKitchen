using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ClientController : MonoBehaviour
{
    [SerializeField] private Transform waitForTablePosition;
    [SerializeField] private bool entering;
    [SerializeField] private bool waitForATable;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text temporizador;
    private Table table; 
    private bool waitForOrder;
    private Transform chairPosition;
    private bool canInteract = true;
    private bool collisioning = false;


    private int timeToWaitTable;
    private int timeToWaitToOrder;
    private int waitToOrder = 0;
    private float speed = 7f;
    public bool WaitForOrder { get => waitForOrder; set => waitForOrder = value; }
    public TMP_Text Text { get => text; set => text = value; }
    public int WaitToOrder { get => waitToOrder; set => waitToOrder = value; }
    public TMP_Text Temporizador { get => temporizador; set => temporizador = value; }


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
                StopAllCoroutines();
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
        else if (other.gameObject.CompareTag("Table"))
        {
            table = other.gameObject.GetComponent<Table>();
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
        timeToWaitTable = Random.Range(30,60);
        while(waitForATable && timeToWaitTable > 0)
        {
            temporizador.text = timeToWaitTable.ToString();
            yield return new WaitForSeconds(1f);
            timeToWaitTable--;
        }
        if(timeToWaitTable <= 0)
        {
            FindObjectOfType<InstantiateFila>().GetTables();
            FindObjectOfType<InstantiateFila>().Chances--;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }



    public IEnumerator WaitingToOrder()
    {
        timeToWaitToOrder = Random.Range(15, 30);
        while (timeToWaitToOrder > 0)
        {
            yield return new WaitForSeconds(1f);
            timeToWaitToOrder--;
        }
        waitToOrder = Random.Range(30,45);
        while (waitToOrder > 0)
        {
            temporizador.text = waitToOrder.ToString();
            yield return new WaitForSeconds(1f);
            waitToOrder--;
        }
        if(waitToOrder <= 0)
        {
            table.IsEmpty = true;
            FindObjectOfType<InstantiateFila>().GetTables();
            FindObjectOfType<InstantiateFila>().Chances--;
            Destroy(gameObject);
        }
    }

    public IEnumerator WaitToEat()
    {
        int waitFood = Random.Range(220, 251);
        while (waitFood > 0)
        {
            temporizador.text = waitFood.ToString();
            yield return new WaitForSeconds(1f);
            waitFood--;
        }
        if (waitFood <= 0)
        {
            table.IsEmpty = true;
            FindObjectOfType<InstantiateFila>().GetTables();
            FindObjectOfType<InstantiateFila>().Chances--;
            Destroy(gameObject);
        }
    }

}
