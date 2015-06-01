using UnityEngine;
using System.Collections;

public class TrailRendererSortingLayerFix : MonoBehaviour {

	void Start () {
		gameObject.GetComponent<TrailRenderer>().sortingLayerName = "Default";
	}
}
