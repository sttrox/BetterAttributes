﻿using System;
using System.Diagnostics;
using UnityEngine;

namespace Better.Attributes.Runtime.DrawInspector
{
    /// <summary>
    /// Replaces object field with nested inspector
    /// </summary>
    [Conditional(ConstantDefines.Editor)]
    [AttributeUsage(AttributeTargets.Field)]
    public class DrawInspectorAttribute : PropertyAttribute
    {
    }
}