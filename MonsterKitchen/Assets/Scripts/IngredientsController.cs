using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientsController : MonoBehaviour
{
    private ChefController chefController;
    private bool canBeClicked;

    public bool CanBeClicked { get => canBeClicked; set => canBeClicked = value; }

    private void Start()
    {
        chefController = FindAnyObjectByType<ChefController>();
        canBeClicked = false;
    }

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canBeClicked)
        {
            chefController.SelectedIngredients.Add(gameObject.name);
        }
    }
}
