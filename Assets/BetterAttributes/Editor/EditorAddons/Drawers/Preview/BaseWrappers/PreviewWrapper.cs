﻿using System.Threading;
using System.Threading.Tasks;
using Better.EditorTools.Helpers;
using UnityEditor;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Preview
{
    public abstract class PreviewWrapper : BasePreviewWrapper
    {
        private bool _isMouseDown;

        private CancellationTokenSource _cancellation;
        private Vector2 _currentMousePosition;
        private Vector2 _currentScreenMousePosition;
        private protected readonly PreviewSceneRenderer _previewScene;

        private protected abstract void UpdateTexture();

        protected PreviewWrapper()
        {
            _previewScene = new PreviewSceneRenderer();
        }

        ~PreviewWrapper()
        {
            Deconstruct();
        }

        public override void Deconstruct()
        {
            EditorPopup.CloseInstance();
            _previewScene?.Deconstruct();
            _cancellation?.Cancel(false);
            _cancellation = null;
            _isMouseDown = false;
        }

        private protected abstract Texture GenerateTexture(Object drawnObject, float size);

        public override void OnGUI(Rect position, SerializedProperty serializedProperty, float size)
        {
            if (!ValidateObject(serializedProperty.objectReferenceValue))
            {
                return;
            }

            _currentScreenMousePosition = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            _currentMousePosition = Event.current.mousePosition;
            CheckInteraction(position, serializedProperty, size);
        }

        private async void UpdateTextureLoop(EditorPopup editorPopup, CancellationToken cancellationToken)
        {
            await Task.Yield();
            while (_isMouseDown && !cancellationToken.IsCancellationRequested)
            {
                UpdateTexture();
                editorPopup.UpdatePosition(_currentScreenMousePosition);
                await Task.Yield();
                if (cancellationToken.IsCancellationRequested) break;
            }
        }

        private void CheckInteraction(Rect position, SerializedProperty serializedProperty, float size)
        {
            var contains = position.Contains(_currentMousePosition);
            if (contains && DrawersHelper.IsLeftButtonDown())
            {
                MouseDownCase(serializedProperty, size);
            }
            else
            {
                var isLeftDrag = DrawersHelper.IsMouseButton(EventType.MouseDrag, DrawersHelper.MouseButtonLeft);
                var isLeftUp = DrawersHelper.IsLeftButtonUp();
                if (isLeftUp || isLeftDrag && !contains && _isMouseDown)
                {
                    Deconstruct();
                }
            }
        }

        private void MouseDownCase(SerializedProperty serializedProperty, float size)
        {
            if (!_isMouseDown)
            {
                _previewScene.Construct();
                var texture = GenerateTexture(serializedProperty.objectReferenceValue, size);
                if (texture == null) return;
                var popup = EditorPopup.Initialize(texture,
                    new Rect(_currentScreenMousePosition, Vector2.one * size), true);
                _isMouseDown = true;

                _cancellation = new CancellationTokenSource();
                UpdateTextureLoop(popup, _cancellation.Token);
            }
        }
    }
}