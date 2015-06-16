using UnityEngine;
using System.Collections;

public class ExRotationTouch : TouchAbstract {

	public float speedRotation = 10;
	public Vector2 clampX = Vector2.zero;
	public Vector2 clampY = Vector2.zero;
	public bool invert = false;
	private Vector2 swipXYFactor;
	private GameObject toto;

	void start()
	{
	}
	void OnTouchMoved()
	{
		swipXYFactor = RotateTouchObject(transform.eulerAngles, speedRotation, clampX, clampY, invert);
		transform.Rotate(new Vector3(0.0f, swipXYFactor[1], swipXYFactor[0]), Space.World);//WORLD
	}
}
