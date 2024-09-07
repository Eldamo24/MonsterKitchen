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

    private void Start()
    {
        text = GameObject.Find("Time").GetComponent<TMP_Text>();
        hand = GameObject.Find("Hand").GetComponent<Transform>();
    }

    private void Update()
    {
        if(flag == 0 && triggering)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                text.text = "";
                flag = 1;
            }
        }
        else if(flag == 1)
        {
            transform.position = hand.position; 
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && flag == 0)
        {
            text.text = "E to interact";
            triggering = true;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(flag == 0 && other.CompareTag("Player"))
        {
            triggering = false;
            text.text = "";
        }
    }
}
