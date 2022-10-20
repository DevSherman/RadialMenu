using System;
using TMPro;
using UnityEngine;

public class RadialMenuController : MonoBehaviour
{
    public RadialItem radialItemPrefab;
    private float degreesPerItem;
    public float gapDegrees = 1.0f;

    public RadialMenuCategory mainMenu; //items or categories
    private RadialMenuCategory currentMenu;

    public Color backgroundColor = new(1f, 1f, 1f, 0.5f);
    public Color selectionColor = new(1f, 1f, 1f, 0.75f);

    private RadialItem[] currentUI_Items; //instance UI
    public TextMeshProUGUI infoText;

    private void Start()
    {
        currentMenu = mainMenu;
        UpdateRadialMenu();
    }

    private void UpdateRadialMenu()
    {
        foreach (Transform child in transform) { GameObject.Destroy(child.gameObject); }

        int size = currentMenu.menuObjects.Length;
        degreesPerItem = 360f / size;
        float distanceToIcon = Vector3.Distance(radialItemPrefab.icon.transform.position, radialItemPrefab.background.transform.position);
        currentUI_Items = new RadialItem[size];

        for (int i = 0; i < size; i++)
        {
            //background
            //currentUI_Items[i].background.sprite == backgroundSprite; //setted in the prefab
            currentUI_Items[i] = Instantiate(radialItemPrefab, transform);
            currentUI_Items[i].background.fillAmount = (1f / size) - (gapDegrees / 360f);
            currentUI_Items[i].background.transform.localRotation = Quaternion.Euler(0, 0, (degreesPerItem * 0.5f) + (gapDegrees * 0.5f) + (i * degreesPerItem));
            //icon
            currentUI_Items[i].icon.sprite = currentMenu.menuObjects[i].icon;
            Vector3 dir = Quaternion.AngleAxis(i * degreesPerItem, Vector3.forward) * Vector3.up;
            Vector3 mov = dir * distanceToIcon;
            currentUI_Items[i].icon.transform.localPosition = currentUI_Items[i].background.transform.localPosition + mov;
        }
    }

    public void UpdateUI()
    {
        int currentElement = GetCurrentElement();
        UpdateElement(currentElement);

        if (Input.GetMouseButtonDown(0)) SelectElement(currentElement);
    }
    private int GetCurrentElement()
    {
        Vector3 screenCenter = new(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector3 cursorVector = Input.mousePosition - screenCenter;

        float angle = Vector3.SignedAngle(Vector3.up, cursorVector, Vector3.forward) + degreesPerItem * 0.5f;
        float normalizedAngle = (angle + 360f) % 360f;

        return (int)(normalizedAngle / degreesPerItem);
    }
    private void UpdateElement(int index)
    {
        if (currentUI_Items == null) return;

        for (int i = 0; i < currentUI_Items.Length; i++)
            if (i != index) currentUI_Items[i].background.color = backgroundColor;

        currentUI_Items[index].background.color = selectionColor;
        infoText.text = currentMenu.menuObjects[index].objectName;
    }
    private void SelectElement(int index)
    {
        Type typeClass = currentMenu.menuObjects[index].GetType();

        if (typeClass.FullName == "RadialMenuCategory") //category selection
        {
            currentMenu = (RadialMenuCategory)currentMenu.menuObjects[index];
            UpdateRadialMenu();
        }
        else if (typeClass.FullName == "RadialMenuItem") //item selection
        {
            RadialMenuItem item = (RadialMenuItem)currentMenu.menuObjects[index];
            //do something here like
            //Action action = item.prefab.GetComponent<Action>();
            //action.Use(); 
        }
        else if (typeClass.FullName == "RadialMenuBack") //back
        {
            currentMenu = currentMenu.previous;
            UpdateRadialMenu();
        }
    }
}
