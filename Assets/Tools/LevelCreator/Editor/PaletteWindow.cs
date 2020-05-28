using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace MadRollers.LevelCreator
{
    public class PaletteWindow : EditorWindow
    {
        private List<PaletteItem.Category> _categories;
        private List<string> _categoriesLabels;
        private PaletteItem.Category _categorySelected;

        public static PaletteWindow instance;

        public static void ShowPalette()
        {
            instance = (PaletteWindow)EditorWindow.GetWindow(typeof(PaletteWindow));
            instance.titleContent = new GUIContent("Mad Rollers Palette");
        }
        private void OnEnable()
        {
            if(_categories == null)
            {
                InitCateGories();
            }
        }
        void InitCateGories()
        {
            _categories = EditorUtils.GetListFromEnum<PaletteItem.Category>();
            _categoriesLabels = new List<string>();
            foreach(PaletteItem.Category c in _categories)
            {
                _categoriesLabels.Add(c.ToString());
            }
        }
        void DrawTabs()
        {
            int index = (int)_categorySelected;
            index = GUILayout.Toolbar(index, _categoriesLabels.ToArray());
            _categorySelected = _categories[index];
        }
        private void OnDisable()
        {
            
        }
        private void OnDestroy()
        {
            
        }
        private void OnGUI()
        {
            EditorGUILayout.LabelField("Mad Rollers Levels Creator");
            DrawTabs();
        }
        private void Update()
        {
            
        }
    }
}
