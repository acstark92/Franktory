using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour
{
	public float turnSpeed = 15f;
	
	
	void Update ()
	{
		transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
	}
}