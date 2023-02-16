using System;
using UnityEngine;
public interface IARService
{
	public float GetDistance(ARObject obj);
	public void AddAREventListener(AREventType type, AREvent arEvent);
	public void TryPlaceObject(Vector2 touchPosition);
}
