﻿using System;
using BetterExtensions.Runtime.Extension;
using UnityEngine;

namespace BetterAttributes.Runtime.Attributes.Headers
{
    /// <summary>
    /// Replacement for Header("Settings")
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class SettingsHeaderAttribute : HeaderAttribute
    {
        public SettingsHeaderAttribute() : base("Settings")
        {
        }

        public SettingsHeaderAttribute(string additionalText, bool preHeader = true) : base(preHeader
                                                                                                ? $"{additionalText.PrettyCamelCase()} Settings"
                                                                                                : $"Settings {additionalText.PrettyCamelCase()}")
        {
        }
    }
}
