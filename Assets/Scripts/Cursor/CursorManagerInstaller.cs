using System;
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
            Container.Bind<CustomCursor[]>().FromInstance(cursors).AsSingle();
            
            foreach(CustomCursor cursor in cursors)
            {
                Type cursorType = cursor.GetType();
                Container.Bind(cursorType).FromInstance(cursor);
            }
        }
    }
}