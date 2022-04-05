using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public EntityFetcher Fetcher { get; private set; }

	private void Awake() {
		Fetcher = new EntityFetcher(this);
	}

}
