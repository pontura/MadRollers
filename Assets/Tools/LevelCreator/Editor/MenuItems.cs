using System.Collections;
using UnityEngine;
using UnityEditor;

namespace MadRollers.LevelCreator
{
    public class MenuItems : MonoBehaviour
    {
        [MenuItem("Tools/Level Creator/Show Palette _l")]
        private static void ShowPalette()
        {
            PaletteWindow.ShowPalette();
        }
    }
}
