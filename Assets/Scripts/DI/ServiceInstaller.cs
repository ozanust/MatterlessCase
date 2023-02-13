using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.XR.ARFoundation;

public class ServiceInstaller : MonoInstaller
{
    [SerializeField]
    private ARSession aRSession;
    public override void InstallBindings()
    {
        Container.Bind(typeof(ARRaycastManager), typeof(Camera)).FromComponentInNewPrefab(aRSession).AsSingle();
        Container.Bind(typeof(IARService), typeof(ITickable)).To<ARService>().AsSingle();
    }
}
