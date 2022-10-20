using UnityEngine;

public class MainController : MonoBehaviour
{
    public RadialMenuController radialMenu;
    public KeyCode keyCode;

    public bool showRadialMenu = false;

    private void Start()
    {
        radialMenu.transform.gameObject.SetActive(showRadialMenu); //hide
    }

    private void Update()
    {
        if(showRadialMenu) radialMenu.UpdateUI();

        if (Input.GetKeyDown(keyCode))
        {
            showRadialMenu = !showRadialMenu;
            radialMenu.transform.gameObject.SetActive(showRadialMenu);
        }
    }
}
