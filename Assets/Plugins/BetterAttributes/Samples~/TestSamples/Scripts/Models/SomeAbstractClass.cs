﻿using System;
using UnityEngine;

namespace BetterAttributes.Samples.Models
{
    [Serializable]
    public abstract class SomeAbstractClass
    {
        [SerializeField] private protected int baseIntField;
    }
}