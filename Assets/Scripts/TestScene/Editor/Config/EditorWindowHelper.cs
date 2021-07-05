using UnityEditor;
using System;

namespace GameProject.TrickyTowers.TestScene.Editor.Config
{
    public static class EditorWindowHelper
    {
        public static bool EditFloatValue(string name, float currentValue, Action<float> updateFunction)
        {
            float value = EditorGUILayout.FloatField(name, currentValue);
            if (value != currentValue)
            {
                updateFunction(value);
                return true;
            }

            return false;
        }

        public static bool EditIntValue(string name, int currentValue, Action<int> updateFunction)
        {
            int value = EditorGUILayout.IntField(name, currentValue);
            if (value != currentValue)
            {
                updateFunction(value);
                return true;
            }

            return false;
        }
    }
}
