using System;
using UnityEngine;

namespace InteractiveKitchen.Utilities
{    
    public class TypeFilterAttribute : PropertyAttribute
    {
        public Func<Type, bool> Filter { get; }

        public TypeFilterAttribute(Type FilterType)
        {
            Filter = type => (type.IsAbstract && type == FilterType)
                            ||
                            (type.Inherits(FilterType) &&
                            !type.IsAbstract &&
                            !type.IsInterface &&
                            !type.IsGenericType);
        }
    }
}
