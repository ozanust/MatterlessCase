using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : IGameController
{
	private IARService arService;
	public GameController(IARService arService)
	{
		this.arService = arService;
	}

	public void TryPlaceObject(Vector2 touchPosition)
	{
		arService.TryPlaceObject(touchPosition);
	}
}
