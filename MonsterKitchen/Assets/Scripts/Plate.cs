using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class Plate : MonoBehaviour
{
    private TMP_Text text;
    private int flag = 0;
    private Transform hand;
    private bool triggering;
    WaiterController controller;

    private void Start()
    {
        text = GameObject.Find("Time").GetComponent<TMP_Text>();
        hand = GameObject.Find("Hand").GetComponent<Transform>();
    }

    private void Update()
    {
        if(flag == 0 && triggering)
        {
            if (Input.GetKeyDown(KeyCode.E) && !controller.HasAPlate)
            {
                text.text = "";
                flag = 1;
                controller.HasAPlate = true;
            }
        }
        else if(flag == 1)
        {
            transform.position = hand.position; 
            transform.parent = hand.transform;
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && flag == 0)
        {
            text.text = "E to interact";
            triggering = true;
            controller = other.gameObject.GetComponent<WaiterController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(flag == 0 && other.CompareTag("Player"))
        {
            triggering = false;
            text.text = "";
            controller = null;
        }
    }
}
