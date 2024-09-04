using UnityEngine;

namespace MadRollers.LevelCreator
{
    public class PaletteItem : MonoBehaviour
    {
#if UNITY_EDITOR
        public enum Category
        {
            Floors,
            Obstacles,
            Enemies,
            Grabbable,
            Bosses
        }
        public Category category = Category.Floors;
        public string itemName = "";
        public Object inspectedScript;

#endif
    }
}
