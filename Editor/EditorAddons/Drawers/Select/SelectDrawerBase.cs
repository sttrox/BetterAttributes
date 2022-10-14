﻿using System;
using System.Collections.Generic;
using BetterAttributes.EditorAddons.Drawers.Base;
using BetterAttributes.EditorAddons.Helpers;
using BetterAttributes.Runtime.Attributes.Select;
using UnityEditor;
using UnityEngine;

namespace BetterAttributes.EditorAddons.Drawers.Select
{
    public abstract class SelectDrawerBase<T> : FieldDrawer where T : SelectAttributeBase
    {
        private bool _needUpdate;
        private bool _isSetUp;
        private DisplayName _displayName;
        private DisplayGrouping _displayGrouping;

        private protected const string NotSupported = "Not supported";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }
        
        private protected override bool PreDraw(ref Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                if (!CheckSupported(property))
                {
                    DrawersHelper.NotSupportedAttribute(property.displayName, fieldInfo.FieldType, attribute.GetType());
                    return false;
                }

                var att = (T)attribute;
                var popupPosition = GetPopupPosition(position);
                if (!_isSetUp)
                {
                    Setup(property, att);
                    _displayName = att.DisplayName;
                    _displayGrouping = att.DisplayGrouping;
                    SetReady();
                }
                var referenceValue = GetCurrentValue(property);
                if (DrawButton(popupPosition, referenceValue))
                {
                    ShowDropDown(popupPosition, referenceValue);
                }

                if (_needUpdate)
                {
                    UpdateValue(property);
                    _needUpdate = false;
                    AfterValueUpdated(property);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return true;
        }

        private Rect GetPopupPosition(Rect currentPosition)
        {
            var popupPosition = new Rect(currentPosition);
            popupPosition.width -= EditorGUIUtility.labelWidth;
            popupPosition.x += EditorGUIUtility.labelWidth;
            popupPosition.height = EditorGUIUtility.singleLineHeight;
            return popupPosition;
        }

        private protected override void Deconstruct()
        {
            DropdownWindow.CloseInstance();
        }
        
        private bool DrawButton(Rect buttonPosition, object currentValue)
        {
            var content = DrawersHelper.GetIconGUIContent(IconType.GrayDropdown);

            content.text = GetButtonName(currentValue);
            return GUI.Button(buttonPosition, content, Styles.Button);
        }

        private void ShowDropDown(Rect popupPosition, object currentValue)
        {
            var copy = popupPosition;
            copy.y += EditorGUIUtility.singleLineHeight;
            var popup = DropdownWindow.ShowWindow(GUIUtility.GUIToScreenRect(copy), GenerateHeader());
            var items = GenerateItemsTree(currentValue);

            popup.SetItems(items);
        }

        private protected virtual DropdownCollection GenerateItemsTree(object currentValue)
        {
            var items = new DropdownCollection(new DropdownSubTree(new GUIContent("Root")));
            var collection = GetSelectCollection();
            if (_displayGrouping == DisplayGrouping.None)
            {
                foreach (var type in collection)
                {
                    var guiContent = new GUIContent(ResolveName(type, _displayName));
                    var item = new DropdownItem(guiContent, ResolveState(currentValue, type), OnSelectItem, type);
                    items.AddChild(item);
                }
            }
            else
            {
                foreach (var type in collection)
                {
                    var resolveGroupedName = ResolveGroupedName(type, _displayGrouping);
                    items.AddItem(resolveGroupedName, ResolveState(currentValue, type), OnSelectItem, type);
                }
            }

            return items;
        }

        private protected abstract GUIContent GenerateHeader();
        private protected abstract object GetCurrentValue(SerializedProperty property);
        private protected abstract bool CheckSupported(SerializedProperty property);
        private protected abstract string GetButtonName(object currentValue);
        private protected abstract void Setup(SerializedProperty property, T currentAttribute);
        private protected abstract List<object> GetSelectCollection();
        private protected abstract string ResolveName(object value, DisplayName displayName);
        private protected abstract string[] ResolveGroupedName(object value, DisplayGrouping grouping);
        private protected abstract bool ResolveState(object currentValue, object iteratedValue);
        private protected abstract void OnSelectItem(object obj);
        private protected abstract void UpdateValue(SerializedProperty property);
        private protected abstract void AfterValueUpdated(SerializedProperty property);

        private protected void SetNeedUpdate()
        {
            _needUpdate = true;
        }

        private void SetReady()
        {
            _isSetUp = true;
        }

        private protected override Rect PreparePropertyRect(Rect original)
        {
            return original;
        }

        private protected override void PostDraw(Rect position, SerializedProperty property, GUIContent label)
        {
        }
    }
}