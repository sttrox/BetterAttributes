﻿using UnityEditor;

namespace BetterAttributes.EditorAddons.Drawers.Gizmo
{
    public class BoundsWrapper : BoundsBaseWrapper
    {
        public override void Apply(SceneView sceneView)
        {
            if (!ShowInSceneView) return;
            if (!ValidateSerializedObject()) return;
            DrawLabel($"{_serializedProperty.name}:\nCenter: {_bounds.center}\nSize: {_bounds.size}", _bounds.center,
                _defaultRotation, sceneView);
            _bounds.center = Handles.PositionHandle(_bounds.center, _defaultRotation);
            DrawAndSetSize(_bounds.center);
            ValidateSize();
            Handles.DrawWireCube(_bounds.center, _bounds.size);

            SetValueAndApply(_bounds);
        }
    }
}