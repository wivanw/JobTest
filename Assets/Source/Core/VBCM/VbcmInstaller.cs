using VBCM.Interfaces;
using Zenject;

namespace VBCM
{
    public class VbcmInstaller : Installer<VbcmInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBinder>().To<Binder>().AsSingle();
            Container.Bind<IController>().To<Controller>().AsSingle();
            Container.Bind<IValidator>().To<Validator>().AsSingle();
        }
    }
}