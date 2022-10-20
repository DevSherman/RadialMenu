using UnityEngine;
[CreateAssetMenu(fileName = "RadialMenuCategory", menuName = "RadialMenu/New Category")]
public class RadialMenuCategory : RadialMenuObject
{
    public RadialMenuCategory previous;
    public RadialMenuObject[] menuObjects;
}
