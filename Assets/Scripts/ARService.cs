using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class ARService : IARService
{
	public delegate void AREvent();
	public AREvent OnAdd;
	public AREvent OnRemove;
	public AREvent OnMove;

	private ARSession arSession;
	public ARService()
	{
		
	}

	public float GetDistance(ARObject gameObject, Camera cam)
	{
		// make calc with ar session or any other
		return 0;
	}

	public void AddObjectAdditionListener()
	{

	}
	public void AddCameraListener(AREvent aREvent)
	{
		OnMove += aREvent;
	}

	private void OnAddEvent()
	{
		OnAdd();
	}
}