﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.Attributes.Runtime;
using Better.EditorTools.CustomEditors;
using Better.Extensions.Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Better.Attributes.EditorAddons.CustomEditors
{
    [BetterEditor(typeof(Object), true, Order = 999)]
    public class BetterButtonsEditor : EditorExtension
    {
        private Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>> _methodButtonsAttributes =
            new Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>>();
        
        public BetterButtonsEditor(Object target, SerializedObject serializedObject) : base(target, serializedObject)
        {
        }

        public override void OnDisable()
        {
        }

        public override void OnEnable()
        {
            var type = _target.GetType();
            _methodButtonsAttributes = type.GetSortedMethodAttributes();
        }

        private void DrawButton(KeyValuePair<MethodInfo, EditorButtonAttribute> button, GUIStyle guiStyle)
        {
            var attribute = button.Value;
            var methodInfo = button.Key;

            if (GUILayout.Button(attribute.GetDisplayName(methodInfo.PrettyMemberName()), guiStyle))
                methodInfo.Invoke(_target, attribute.InvokeParams);
        }

        private void DrawButtons(Dictionary<int, IEnumerable<KeyValuePair<MethodInfo, EditorButtonAttribute>>> buttons)
        {
            var guiStyle = new GUIStyle(GUI.skin.button)
            {
                stretchWidth = true,
                richText = true,
                wordWrap = true
            };

            foreach (var button in buttons)
            {
                if (button.Key == -1)
                {
                    var grouped = button.Value.GroupBy(key => key.Key, pair => pair.Value,
                        (info, attributes) =>
                            new KeyValuePair<MethodInfo,
                                IEnumerable<EditorButtonAttribute>>(info, attributes));
                    EditorGUILayout.BeginVertical();

                    foreach (var group in grouped)
                    {
                        EditorGUILayout.BeginHorizontal();

                        foreach (var attribute in group.Value)
                            DrawButton(new KeyValuePair<MethodInfo, EditorButtonAttribute>(group.Key, attribute),
                                guiStyle);
                        EditorGUILayout.EndHorizontal();
                    }

                    EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    foreach (var pair in button.Value) DrawButton(pair, guiStyle);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        public override void OnInspectorGUI()
        {
            _serializedObject.Update();
            DrawButtons(_methodButtonsAttributes);
            _serializedObject.ApplyModifiedProperties();
        }

        public override void OnChanged()
        {
            
        }
    }
}