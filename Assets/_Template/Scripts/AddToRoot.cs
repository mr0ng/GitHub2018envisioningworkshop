using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToRoot : MonoBehaviour
{
	[SerializeField] bool addToInputShare = false;

	void Awake ()
	{
		string     objName = addToInputShare ? "InputSharingRoot" : "WorldRoot";
		GameObject root    = GameObject.Find(objName);
		if (root == null)
			Debug.LogError("Could not find a "+objName+" object!");
		else
			transform.parent = root.transform;
	}
}
