using System;
using System.Linq;
using ModestTree;
using UnityEngine;

public static class TypeExtensionMethods
{
    public static bool Inherits(this Type type, Type inheritedType)
    {
        Type currentlyCheckedType = type.BaseType;
        while(currentlyCheckedType != typeof(object) && currentlyCheckedType != null)
        {
            if(currentlyCheckedType == inheritedType) return true;

            Type[] interfaces = currentlyCheckedType.Interfaces();
            if(interfaces != null && interfaces.Contains(inheritedType)) return true;
        
            currentlyCheckedType = currentlyCheckedType.BaseType;
        }
        return false;
    }
}
