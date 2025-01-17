﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Better.Attributes.Runtime.Select;
using Better.Extensions.Runtime;
using UnityEngine;

namespace Better.Attributes.EditorAddons.Drawers.Select.SetupStrategies
{
    public abstract class SetupStrategy
    {
        private protected readonly Type _fieldType;
        private protected readonly SelectAttributeBase _selectAttributeBase;

        protected SetupStrategy(Type fieldType, SelectAttributeBase selectAttributeBase)
        {
            _fieldType = fieldType;
            _selectAttributeBase = selectAttributeBase;
        }
        
        public abstract List<object> Setup(Type baseType);
        public abstract GUIContent ResolveName(object value, DisplayName displayName);
        public abstract GUIContent[] ResolveGroupedName(object value, DisplayGrouping grouping);
        public abstract string GetButtonName(object currentValue);
        public abstract bool ResolveState(object currentValue, object iteratedValue);
        public abstract bool Validate(object item);
        public abstract bool CheckSupported();
        public abstract GUIContent GenerateHeader();

        public virtual Type GetFieldOrElementType()
        {
            var t = _selectAttributeBase.GetFieldType();
            if (t != null)
            {
                return t;
            }

            return _fieldType;
        }
    }
}