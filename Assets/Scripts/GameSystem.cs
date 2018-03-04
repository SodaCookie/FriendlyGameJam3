using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour {

	[SerializeField]
	private GameObject playerPrefab;
	public GameObject player;
	private GameObject startLocation;

	// Use this for initialization
	void Start () {
		startLocation = GameObject.Find ("[Start Location]");
		Debug.Assert (startLocation != null);

		player = Instantiate(playerPrefab);
		player.GetComponent<PlayerControls> ().system = this;
		player.transform.position = startLocation.transform.position;
	}

	public void Reset () {
		Destroy (player);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
