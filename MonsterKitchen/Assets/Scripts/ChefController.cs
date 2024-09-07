using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChefController : MonoBehaviour
{
    [Header("Switch Player")]
    [SerializeField] private PlayerInput chefInput;
    [SerializeField] private ChefController chefController;
    [SerializeField] private PlayerInput waiterInput;
    [SerializeField] private WaiterController waiterController;
    private Animator anim;

    [Header("Orders")]
    [SerializeField] private List<SOReceta> orders;
    [SerializeField] private List<string> selectedIngredients;

    [SerializeField] private bool startCook = false;
    [SerializeField] private bool gameStarted = false;
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text ingredients;
    private float satisfaction = 100;

    [SerializeField] List<KeyCode> keysToPress;
    [SerializeField] List<KeyCode> keysPressedByUser;

    [SerializeField] private Transform[] platesPosition;
    [SerializeField] private GameObject plate;

    private bool cutting = false;

    public List<SOReceta> Orders { get => orders; set => orders = value; }
    public List<string> SelectedIngredients { get => selectedIngredients; set => selectedIngredients = value; }

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(orders.Count > 0)
        {
            startCook = true;
        }
        else
        {
            startCook = false;
        }
        if (startCook)
        {
            StartCooking();
        }
        if (cutting)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    keysPressedByUser.Add(keyCode);
                }
            }
        }
    }


    public void SwitchPlayer(InputAction.CallbackContext context)
    {
        if (context.performed && !startCook)
        {
            if (chefController.isActiveAndEnabled)
            {
                chefController.enabled = false;
                chefInput.enabled = false;
                waiterController.enabled = true;
                waiterInput.enabled = true;
            }
            else
            {
                waiterController.enabled = false;
                waiterInput.enabled = false;
                chefController.enabled = true;
                chefInput.enabled = true;
            }
        }
    }

    void StartCooking()
    {
        if (!gameStarted)
        {
            anim.SetBool("Cooking", true);
            gameStarted = true;
            timer.text = "We are going to prepare: " + orders[0].nameReceta;
            ingredients.text = orders[0].ingredient1 + "\n" + orders[0].ingredient2 + "\n" + orders[0].ingredient3 + "\n" + orders[0].ingredient4;
            StartCoroutine("SelectIngredients");
        }
    }

    IEnumerator SelectIngredients()
    {
        yield return new WaitForSeconds(3f);
        timer.text = "Prepare to select your ingredients from the boxes with a click";
        yield return new WaitForSeconds(3f);
        timer.text = "Remember to collect them in order";
        yield return new WaitForSeconds(3f);
        float seconds = 5f;
        timer.text = seconds.ToString();
        CanClickIngredients();
        while(seconds > 0 && selectedIngredients.Count < 4)
        {
            yield return new WaitForSeconds(1f);
            seconds--;
            timer.text = seconds.ToString();
        }
        CanClickIngredients();
        if (selectedIngredients.Count != 4)
        {
            satisfaction -= 25f;
            timer.text = "Well... it could be better...";
        }
        else
        {
            int count = CheckIngredients();
            if (count == 4)
            {
                timer.text = "You did it well";
            }
            else
            {
                timer.text = "Well... it could be better...";
            }
        }
        yield return new WaitForSeconds(2f);
        timer.text = "";
        ingredients.text = "";
        StartCoroutine("CutIngredients");
    }

    IEnumerator CutIngredients()
    {
        timer.text = "Prepare to cut the ingredientes...";
        yield return new WaitForSeconds(3f);
        timer.text = "Repeat the keys in order to cut";
        yield return new WaitForSeconds(3f);
        timer.text = "Prepare to start... ";
        yield return new WaitForSeconds(3f);
        for(int j=0; j< selectedIngredients.Count; j++)
        {
            float time = 5f;
            string keysToCopy = "";
            timer.text = "Cut the " + selectedIngredients[j];
            yield return new WaitForSeconds(2f);
            int keys = UnityEngine.Random.Range(5, 11);
            for (int i = 0; i < keys; i++)
            {
                int key = UnityEngine.Random.Range(0, 4);
                switch (key)
                {
                    case 0:
                        keysToPress.Add(KeyCode.J);
                        break;
                    case 1:
                        keysToPress.Add(KeyCode.K);
                        break;
                    case 2:
                        keysToPress.Add(KeyCode.L);
                        break;
                    case 3:
                        keysToPress.Add(KeyCode.I);
                        break;
                }
                keysToCopy +=" " + keysToPress[i].ToString();
            }
            timer.text = keysToCopy;
            yield return new WaitForSeconds(5f);
            timer.text = "Cut!";
            yield return new WaitForSeconds(1f);
            cutting = true;
            while (time > 0)
            {
                timer.text = time.ToString();
                yield return new WaitForSeconds(1f);
                time--;
            }
            cutting = false;
            if (keysToPress.Count != keysPressedByUser.Count)
            {
                timer.text = "You have failed...";
                yield return new WaitForSeconds(2f);
                satisfaction -= 10;
            }
            else
            {
                for (int i = 0; i < keysPressedByUser.Count; i++)
                {
                    int count = 0;
                    if (keysPressedByUser[i].ToString() != keysToPress[i].ToString())
                    {
                        count++;
                        timer.text = "You missed " + count;
                        yield return new WaitForSeconds(2f);
                        satisfaction -= 10;
                    }
                }
            }
            timer.text = "";
            keysPressedByUser.Clear();
            keysToPress.Clear();
            yield return new WaitForSeconds(2f);
        }
        yield return new WaitForSeconds(3f);
        timer.text = "";
        gameStarted = false;
        selectedIngredients.Clear();
        orders.RemoveAt(0);
        foreach(Transform platePosition in platesPosition)
        {
            if (platePosition.gameObject.GetComponent<PlatePositions>().IsEmpty)
            {
                Instantiate(plate, platePosition.position, Quaternion.identity);
                platePosition.gameObject.GetComponent<PlatePositions>().IsEmpty = false;
                break;
            }
        }
        anim.SetBool("Cooking", false);
    }

    void CanClickIngredients()
    {
        IngredientsController[] ingrController = FindObjectsOfType<IngredientsController>();
        foreach (IngredientsController ingr in ingrController)
        {
            ingr.CanBeClicked = !ingr.CanBeClicked;
        }
    }

    private int CheckIngredients()
    {
        int count = 0;
        for (int i = 0; i < selectedIngredients.Count; i++)
        {
            switch (i)
            {
                case 0:
                    if (selectedIngredients[i] == orders[0].ingredient1)
                    {
                        count++;
                    }
                    break;
                case 1:
                    if (selectedIngredients[i] == orders[0].ingredient2)
                    {
                        count++;
                    }
                    break;
                case 2:
                    if (selectedIngredients[i] == orders[0].ingredient3)
                    {
                        count++;
                    }
                    break;
                case 3:
                    if (selectedIngredients[i] == orders[0].ingredient4)
                    {
                        count++;
                    }
                    break;
            }
        }
        return count;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
