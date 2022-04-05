using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sort of singleton class that gets instantiated by the game manager
/// </summary>
public class EntityFetcher {

	public static EntityFetcher Instance { get; private set; } 

	public GameObject Player { get; private set; }

	public GameManager Manager { get; private set; }

	public Camera MainCamera { get; private set; }

	public EntityFetcher(GameManager gameManager) {
		Player = GameObject.FindGameObjectWithTag("Player");
		MainCamera = Camera.main;
		Instance = this;
		Manager = gameManager;
	}

}
