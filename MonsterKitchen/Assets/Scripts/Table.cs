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
            coroutine = client.StartCoroutine(client.WaitingToOrder());
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
