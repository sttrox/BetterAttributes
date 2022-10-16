﻿using BetterAttributes.EditorAddons.Drawers.Base;
using BetterAttributes.EditorAddons.Drawers.Utilities;
using BetterAttributes.EditorAddons.Drawers.WrapperCollections;
using BetterAttributes.EditorAddons.Helpers;
using BetterAttributes.Runtime.Attributes.Gizmo;
using UnityEditor;
using UnityEngine;

namespace BetterAttributes.EditorAddons.Drawers.Gizmo
{
    [CustomPropertyDrawer(typeof(GizmoAttribute))]
    [CustomPropertyDrawer(typeof(GizmoLocalAttribute))]
    public class GizmoDrawer : MultiFieldDrawer<GizmoWrapper>
    {
        private GizmoWrappers Collection
        {
            get
            {
                _wrappers ??= GenerateCollection();
                return _wrappers as GizmoWrappers;
            }
        }

        private HideTransformButtonUtility _hideTransformDrawer;

        public GizmoDrawer() : base()
        {
            SceneView.duringSceneGui += OnSceneGUIDelegate;
        }

        private void OnSceneGUIDelegate(SceneView sceneView)
        {
            if (sceneView.drawGizmos)
            {
                GizmoUtility.Instance.ValidateCachedProperties(_wrappers);
                Collection?.Apply(sceneView);
            }
        }

        private protected override void Deconstruct()
        {
            SceneView.duringSceneGui -= OnSceneGUIDelegate;
            _wrappers?.Deconstruct();
        }

        private protected override void PostDraw(Rect position, SerializedProperty property, GUIContent label)
        {
            if (EditorGUI.EndChangeCheck())
            {
                Collection.SetProperty(property, fieldInfo.FieldType);
            }

            if (GUI.Button(PrepareButtonRect(position), Collection.ShowInSceneView(property) ? "Hide" : "Show"))
            {
                Collection.SwitchShowMode(property);
                SceneView.RepaintAll();
            }
        }

        private protected override WrapperCollection<GizmoWrapper> GenerateCollection()
        {
            return new GizmoWrappers();
        }

        private protected override bool PreDraw(ref Rect position, SerializedProperty property, GUIContent label)
        {
            var fieldType = fieldInfo.FieldType;
            var attributeType = attribute.GetType();

            if (_hideTransformDrawer == null && property.IsTargetComponent(out _))
            {
                _hideTransformDrawer = new HideTransformButtonUtility(property, GizmoUtility.Instance);
            }

            if (!GizmoUtility.Instance.IsSupported(fieldType))
            {
                EditorGUI.BeginChangeCheck();
                DrawField(position, property, label);
                DrawersHelper.NotSupportedAttribute(label.text, fieldInfo.FieldType, attributeType, false);
                return false;
            }

            if (!ValidateCachedProperties(property, GizmoUtility.Instance))
            {
                Collection.SetProperty(property, fieldType);
            }

            EditorGUI.BeginChangeCheck();

            if (_hideTransformDrawer != null)
            {
                _hideTransformDrawer.DrawHideTransformButton();
            }

            return true;
        }


        private protected override Rect PreparePropertyRect(Rect original)
        {
            var copy = original;
            copy.width *= 0.89f;
            return copy;
        }

        private Rect PrepareButtonRect(Rect original)
        {
            var copy = original;
            copy.x += copy.width * 0.9f;
            copy.width *= 0.1f;
            copy.height = EditorGUIUtility.singleLineHeight;
            return copy;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }
    }
}