using UnityEngine;
using Zenject;

namespace InteractiveKitchen.Cursors
{
    [CreateAssetMenu(fileName = "CustomCursorsSettingsInstaller", menuName = "Installers/CustomCursorsSettingsInstaller")]
    public class CustomCursorsSettingsInstaller : ScriptableObjectInstaller<CustomCursorsSettingsInstaller>
    {
        [SerializeField] CustomCursorsSettings currentSettings;
        public override void InstallBindings()
        {
            Container.Bind<CustomCursorsSettings>().FromInstance(currentSettings);
        }
    }
}