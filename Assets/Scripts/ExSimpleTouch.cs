/*/
Author : Colin Laubry
	Simple script for test differents methode from
	Abstract derivated class TouchAbstract.
/*/
		
using UnityEngine;
using System.Collections;

public class ExSimpleTouch : TouchAbstract {
	
	void OnTouchBegan()
	{
		Debug.Log("Began touched " + touchedObject.name + " ID's " + touchId);
	}
	void OnTouchEnded()
	{
		Debug.Log("Ended touched " + touchedObject.name + " ID's " + touchId);
	}
	void OnTouchCanceled()
	{
		Debug.Log("Canceled touched " + touchedObject.name + " ID's " + touchId);
	}
	void OnTouchMoved()
	{
		Debug.Log("Moved touched " + touchedObject.name + " ID's " + touchId);
	}
	void OnTouchStationary()
	{
		Debug.Log("Stationary touched " + touchedObject.name + " ID's " + touchId);
	}
}