using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "InteractableObjectsSettingsInstaller", menuName = "Installers/InteractableObjectsSettingsInstaller")]
public class InteractableObjectsSettingsInstaller : ScriptableObjectInstaller<InteractableObjectsSettingsInstaller>
{
    [SerializeField] InteractableObjectsSettings currentSettings;
    public override void InstallBindings()
    {
        Container.Bind<InteractableObjectsSettings>().FromInstance(currentSettings);
    }
}