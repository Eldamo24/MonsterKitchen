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

    public bool IsEmpty { get => isEmpty; set => isEmpty = value; }

    void Start()
    {
        isEmpty = true;
        chef = FindObjectOfType<ChefController>();
    }

    private void Update()
    {
        if(flag == 1)
        {
            if(triggering && Input.GetKeyDown(KeyCode.E))
            {
                int index = Random.Range(0, menu.Length - 1);
                chef.Orders.Add(menu[index]);
                flag = 0;
                client.Text.text = "";
                StopCoroutine(client.WaitingToOrder());
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
            client.StartCoroutine(client.WaitingToOrder());
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
