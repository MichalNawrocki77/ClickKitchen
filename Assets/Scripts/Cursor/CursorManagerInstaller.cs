using System;
using System.Collections.Generic;
using InteractiveKitchen.Cursors;
using UnityEngine;
using Zenject;

public class CursorManagerInstaller : MonoInstaller
{
    [SerializeField] CursorManager cursorManager;
    public override void InstallBindings()
    {
        CustomCursor[] cursors = cursorManager != null ? 
            cursorManager.GetComponentsInChildren<CustomCursor>(true) :
            GetComponentsInChildren<CustomCursor>(true);
        
        if(cursorManager != null)
            Container.Bind<CursorManager>().FromInstance(cursorManager).AsSingle();
        
        if(cursors != null && cursors.Length > 0)
        {
            Dictionary<Type,CustomCursor> AllCursors = new Dictionary<Type, CustomCursor>();
            foreach(CustomCursor cursor in cursors)
            {
                Type cursorType = cursor.GetType();
                AllCursors.Add(cursorType, cursor);

                Container.Bind(cursorType).FromInstance(cursor);
            }
            Container.Bind<Dictionary<Type, CustomCursor>>().FromInstance(AllCursors).AsSingle();
        }
    }
}