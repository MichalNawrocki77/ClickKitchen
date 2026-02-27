using UnityEngine;
using Zenject;

public class CameraInstaller : MonoInstaller
{
    [SerializeField] Camera MainCam;

    public override void InstallBindings()
    {
        if(MainCam == null)
        {
            MainCam = GetComponent<Camera>();
            if(MainCam == null)
                MainCam = Camera.main;
        }
        
        if(MainCam != null)
            Container.Bind<Camera>().FromInstance(MainCam);
    }
}