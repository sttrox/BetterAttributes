﻿using BetterAttributes.EditorAddons.Drawers.Base;
using BetterAttributes.EditorAddons.Drawers.Utilities;
using BetterAttributes.EditorAddons.Drawers.WrapperCollections;
using BetterAttributes.EditorAddons.Helpers;
using BetterAttributes.Runtime.Attributes.Preview;
using UnityEditor;
using UnityEngine;

namespace BetterAttributes.EditorAddons.Drawers.Preview
{
    [CustomPropertyDrawer(typeof(PreviewAttribute))]
    public class PreviewDrawer : MultiFieldDrawer<BasePreviewWrapper>
    {
        private protected bool _objectChanged;
        private float _previewSize;
        public PreviewWrappers Collection => _wrappers as PreviewWrappers;

        private protected override void Deconstruct()
        {
            _wrappers.Deconstruct();
        }

        private protected override bool PreDraw(ref Rect position, SerializedProperty property, GUIContent label)
        {
            var fieldType = fieldInfo.FieldType;
            var attributeType = attribute.GetType();
            if (!PreviewUtility.Instance.IsSupported(fieldType))
            {
                DrawersHelper.NotSupportedAttribute(label.text, fieldType, attributeType);
                return false;
            }

            if (!ValidateCachedProperties(property, PreviewUtility.Instance))
            {
            }

            EditorGUI.BeginChangeCheck();
            _previewSize = ((PreviewAttribute)attribute).PreviewSize;
            var copy = position;
            label.image = DrawersHelper.GetIcon(IconType.View);
            copy.size = GUIStyle.none.CalcSize(label);
            Collection.OnGUI(copy, property, _previewSize, _objectChanged);

            return true;
        }

        private protected override Rect PreparePropertyRect(Rect original)
        {
            return original;
        }

        private protected override void PostDraw(Rect position, SerializedProperty property, GUIContent label)
        {
            _objectChanged = EditorGUI.EndChangeCheck();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }

        private protected override WrapperCollection<BasePreviewWrapper> GenerateCollection()
        {
            return new PreviewWrappers();
        }
    }
}