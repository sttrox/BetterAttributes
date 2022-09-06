﻿using System;
using System.Diagnostics;
using UnityEngine;

namespace BetterAttributes.Runtime.ReadOnlyAttributes
{
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyFieldAttribute : PropertyAttribute
    {
        
    }
}