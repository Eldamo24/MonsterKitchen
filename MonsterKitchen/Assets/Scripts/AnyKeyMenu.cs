using UnityEngine;

public class AnyKeyMenu : MonoBehaviour
{

    [SerializeField] private GameObject menuGame;


    private void Update()
    {
        if (Input.anyKey)
        {
            menuGame.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
