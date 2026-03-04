using InteractiveKitchen.Utilities;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeStorage", menuName = "ClickKitchen/SO/Recipe Storage")]
public class RecipeStorage : ScriptableObject
{
    [SerializeField] Dictionary<string, int> chuj;
}
