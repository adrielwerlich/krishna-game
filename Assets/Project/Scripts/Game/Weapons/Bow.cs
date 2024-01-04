using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour {

	public GameObject playerModel;
	public GameObject arrowPrefab;

	public void Attack () {
		GameObject arrowObject = Instantiate (arrowPrefab);
		arrowObject.transform.position = playerModel.transform.position + (playerModel.transform.forward * 5);
        arrowObject.transform.position = new Vector3(
			arrowObject.transform.position.x,
			1.1f,
			arrowObject.transform.position.z
		);
        arrowObject.transform.forward = playerModel.transform.forward;
	}
}
