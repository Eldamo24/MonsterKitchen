using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Table : MonoBehaviour
{
    private bool isEmpty;
    [SerializeField] private SOReceta[] menu;
    private ClientController client;
    private int flag = 0;
    private bool triggering = false;
    private ChefController chef;
    private Coroutine coroutine;
    [SerializeField] private BoxCollider box;

    public bool IsEmpty { get => isEmpty; set => isEmpty = value; }

    void Start()
    {
        isEmpty = true;
        chef = FindObjectOfType<ChefController>();
    }

    private void Update()
    {
        if (isEmpty)
        {
            flag = 0;
        }
        else
        {
            box.enabled = true;
        }
        if(flag == 1)
        {
            if(triggering && Input.GetKeyDown(KeyCode.E))
            {
                if(coroutine != null)
                    client.StopCoroutine(coroutine);
                int index = Random.Range(0, menu.Length);
                chef.Orders.Add(menu[index]);
                flag = 0;
                client.Text.text = "";
                client.Temporizador.text = "";
                StartCoroutine(client.WaitToEat());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Client")) 
        {
            flag = 1;
            client = other.gameObject.GetComponent<ClientController>();
            client.WaitForOrder = true;
            if(coroutine == null)
                coroutine = client.StartCoroutine(client.WaitingToOrder());
        }
        if(other.gameObject.CompareTag("Player") && !isEmpty)
        {
            WaiterController waiter = other.gameObject.GetComponent<WaiterController>();
            waiter.HasAPlate = false;
            Destroy(GameObject.Find("Hand").GetComponentInChildren<Plate>().gameObject);
            Destroy(client.gameObject);
            isEmpty = true;
            flag = 0;
            FindObjectOfType<InstantiateFila>().GetTables();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (flag == 1)
        {
            if (other.gameObject.CompareTag("Player") && client.WaitForOrder && client.WaitToOrder > 0)
            {
                client.Text.text = "E to interact";
                triggering = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (flag == 1)
        {
            if (other.CompareTag("Player"))
            {
                client.Text.text = "";
                triggering = false;
            }
        }
    }

}
