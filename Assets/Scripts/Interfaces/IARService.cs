using UnityEngine;
public interface IARService
{
    public float GetDistance(ARObject obj, Camera cam);
    public void AddObjectAdditionListener();
    public void AddCameraListener(ARService.AREvent aREvent);
}
