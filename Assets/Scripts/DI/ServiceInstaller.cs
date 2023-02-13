using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ServiceInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IARService>().To<ARService>().AsSingle();
    }
}
