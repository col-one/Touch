/*/
Author : Colin Laubry
	Simple script for test pinch factor methode
	like zoom on Camera from
	Abstract derivated class TouchAbstract.
	
	Public Param : zoomSpeed
	vitesse du zoom entre 0 et 1
/*/

using UnityEngine;
using System.Collections;

public class ExZoomPinch : TouchAbstract {

	private float zoom;
	public float zoomSpeed;

	void OnTouchMoved()
	{
		CameraZoom();
	}
	void OnTouchStationary()
	{
		CameraZoom();
	}
	void CameraZoom()
	{
		zoom = PinchFactor();
		Camera.main.transform.Translate(Vector3.forward*zoom*zoomSpeed*Time.deltaTime);
	}
	void OnTouchEnded()
	{
	}
	void OnTouchCanceled()
	{
	}
}
