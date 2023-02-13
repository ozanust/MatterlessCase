using UnityEngine;
public interface IARService
{
	public float GetDistance(ARObject obj);
	public void AddObjectAdditionListener(ARService.AREvent aREvent);
	public void AddCameraListener(ARService.AREvent aREvent);
	public void SetARObject(GameObject arObject);
}
