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

    [Header("Orders")]
    [SerializeField] private List<SOReceta> orders;
    [SerializeField] private List<string> selectedIngredients;

    [SerializeField] private bool startCook = false;
    [SerializeField] private bool gameStarted = false;
    [SerializeField] TMP_Text timer;
    [SerializeField] TMP_Text ingredients;
    private float satisfaction = 100;

    public List<SOReceta> Orders { get => orders; set => orders = value; }
    public List<string> SelectedIngredients { get => selectedIngredients; set => selectedIngredients = value; }

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
        if(selectedIngredients.Count != 4)
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
        CanClickIngredients();
        orders.RemoveAt(0);
        gameStarted = false;
        selectedIngredients = null;
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

}
