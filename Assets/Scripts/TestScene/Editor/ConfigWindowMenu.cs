using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using GameProject.TrickyTowers.TestScene.Editor.Config;
using System.Linq;

namespace GameProject.TrickyTowers.TestScene.Editor
{
    public class ConfigWindowMenu
    {
        private EConfigMenuType _selectedOption = EConfigMenuType.None;
        private Color _defaultColor;
        private List<IConfigMenuDrawer> _menuItems;

        public ConfigWindowMenu()
        {
            _defaultColor = GUI.color;
            _menuItems = new List<IConfigMenuDrawer>
            {
                new InGamePhysicsConfig(),
                new GameplayConfig()
            };

        }

        public void Draw(TestSceneController testSceneController)
        {
            using (var scope = new EditorGUILayout.HorizontalScope("Box"))
            {
                DrawMenu();
                DrawBody(testSceneController);
            }
        }

        private void DrawMenuItem(EConfigMenuType option)
        {
            GUI.color = (_selectedOption == option) ? Color.red : _defaultColor;
            if (GUILayout.Button(option.ToString())) _selectedOption = option;
            GUI.color = _defaultColor;
        }

        private void DrawMenu()
        {
            var width = GUILayout.Width(100);
            using (var scope = new EditorGUILayout.VerticalScope("Box", width))
            {
                var align = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField("MENU", align, width);
                EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider, width);
                _menuItems.ForEach(i => DrawMenuItem(i.Option));
                GUILayout.FlexibleSpace();

                bool isEnabled = GUI.enabled;
                GUI.enabled = true;
                var buttonText = EditorApplication.isPlaying ? "Stop" : "Simulate";
                if (GUILayout.Button(buttonText, width))
                {
                    EditorApplication.isPlaying = !EditorApplication.isPlaying;
                }
                GUI.enabled = isEnabled;
            }
        }

        private void DrawBody(TestSceneController testSceneController)
        {
            var item = _menuItems.FirstOrDefault(i => i.Option == _selectedOption);
            item?.Draw(testSceneController);
        }
    }
}
