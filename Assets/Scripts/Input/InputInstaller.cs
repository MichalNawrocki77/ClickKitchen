using InteractiveKitchen;
using UnityEngine;
using Zenject;

public class InputInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GlobalGameInput>().ToSelf().AsSingle();
    }
}