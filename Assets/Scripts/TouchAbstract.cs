﻿/*/
Author : Colin Laubry
	This abstract class wrap classique methode of Input.Touch ...
	form a script who derived from this class TouchAbstract call
	methode OnTouchMoved, OnTouchEnded, OnTouchStationary, OnTouchBegan
	put this derived script on any object of the scene like Camera.

	Public Param : useMask
	set to true for use mask, use mask param for precise layer

	Public Param : mask
	set the mask of touchable object.

	Public Param : touch2D
	set true for use with 2D physics
/*/
		
		
using System;
using UnityEngine;
using System.Collections;

public class TouchAbstract : MonoBehaviour {

	[Header("Mask Parameters")]
	public bool useMask = false;//touch everything if false
	public LayerMask mask; //touchable mask
	[Header("Others")]
    public bool touch2D = false; //public param for use with 2D physics
	public static int touchId; //id of current touch, accessible by other script
	private Ray ray; //ray for screentoscene
	private RaycastHit rayInfo; //get info from the raycast
	public static GameObject touchedObject = Camera.main.transform.gameObject; //get touched current object, accessible by other script, default is Camera.main
	public static float rayLength = Mathf.Infinity; //get ray length limit for rayCast, accessible by other script
	private bool cast; //bool for cast
	private RaycastHit2D hit; //ray for hit 2D
	private Vector3 pos2D; //pos2D from screen to world
	
	void Update()
	{
		if (Input.touches.Length > 0) 
		{
			foreach(Touch touch in Input.touches)
			{
				touchId = touch.fingerId;
				if(touch2D)
				{
					pos2D = Camera.main.ScreenToWorldPoint(touch.position); //get world pos
					hit = Physics2D.Raycast(pos2D, Vector2.zero, rayLength); //get the ray and hit
					if(hit.collider != null)
						touchedObject = hit.collider.transform.gameObject;
				}
				else //for 3D set up
				{
					ray = Camera.main.ScreenPointToRay(touch.position);
					if(useMask)
					{
						cast = Physics.Raycast(ray, out rayInfo, rayLength, mask);
						if(cast) // double check if ray collide object 2D or weard things
							touchedObject = rayInfo.transform.gameObject;
					}
				}
				
				if(hit.collider != null || useMask == false || cast && touchedObject != null)
				{
					switch(touch.phase)
					{
					case TouchPhase.Began :
						this.SendMessage("OnTouchBegan", SendMessageOptions.DontRequireReceiver);
						break;
					case TouchPhase.Canceled :
						this.SendMessage("OnTouchCanceled", SendMessageOptions.DontRequireReceiver);
						break;
					case TouchPhase.Ended :
						this.SendMessage("OnTouchEnded", SendMessageOptions.DontRequireReceiver);
						break;
					case TouchPhase.Moved :
						this.SendMessage("OnTouchMoved", SendMessageOptions.DontRequireReceiver);
						break;
					case TouchPhase.Stationary :
						this.SendMessage("OnTouchStationary", SendMessageOptions.DontRequireReceiver);
						break;
					}
				}
			}
		}
	}

	/*/ Pinch Factor Method /*/
	//get the factor between two pinch touches, usefull for pinch zoom ...
	//use this methode in OnTouchMoved and OnTouchStationary
	//return float value
	Vector2 currPosFirst = Vector2.zero;//curr positions
	Vector2 currPosSecond = Vector2.zero;
	Vector2 lastPosFirst = Vector2.zero;//last positions
	Vector2 lastPosSecond = Vector2.zero;
	float currDistance;//curr distance
	float lastDistance;//last distance
	float zoomFactor;//factor between curr and last dist
	public virtual float PinchFactor()
	{

		if(touchId==0 && Input.touchCount > 1)//check plus de un touch pour eviter IndexError avec le GetTouch
		{
			currPosFirst = Input.GetTouch(touchId).position;//stock la pos du premier touch
			lastPosFirst = currPosFirst - Input.GetTouch(touchId).deltaPosition;//stock la derniere pos grace a la diff 
		}																  //entre la pos current et la delta pos

		if(touchId==1 && Input.touchCount > 1)//check plus de un touch pour eviter IndexError avec le GetTouch
		{
			currPosSecond = Input.GetTouch(touchId).position;
			lastPosSecond = currPosSecond - Input.GetTouch(touchId).deltaPosition;
		}
		if(touchId >= 1)//check plus de 1 touch
		{
			currDistance = Vector2.Distance(currPosFirst, currPosSecond);
			lastDistance = Vector2.Distance(lastPosFirst, lastPosSecond);
		}
		else
		{
			currDistance = 0.0f;
			lastDistance = 0.0f;
		}

		zoomFactor = lastDistance - currDistance;
		return zoomFactor;
	}
	/*/ Pinch Factor End /*/


	/*/ Rotate byte touch Method /*/
	//return tuple with swipX and swipY factor use it
	//with eulerAngle new Vector3
	//remindRotation : Vector3 eulerAngle of object touched for not to reset at zero
	//Param : 
	//speed : float speed fo the rotation 0 - 100
	//clampX : Vector2 clamp rotation X  
	//clampY : Vector2 clamp rotation Y
	//invert : bool invert rotation
	private float swipX;
	private float swipY;
	private Vector3 remindRotation;
	private int invertInt;
	public virtual Vector2 RotateTouchObject(Vector3 remindRotation, float speed, Vector2 clampX, Vector2 clampY, bool invert)
	{
		swipX = remindRotation.x;
		swipY = remindRotation.y;
		if(invert)
		{
			invertInt = -1;
		}
		else
		{
			invertInt = 1;
		}
		swipX = Input.GetTouch(touchId).deltaPosition.y * speed * invertInt * Time.deltaTime;
		swipY = Input.GetTouch(touchId).deltaPosition.x * speed * -invertInt * Time.deltaTime;
		if(clampX != Vector2.zero)
		{
			swipX = Mathf.Clamp(swipX, clampX[0], clampX[1]);
		}
		if(clampY != Vector2.zero)
		{
			swipY = Mathf.Clamp(swipY, clampY[0], clampY[1]);
		}
		return new Vector2(swipX, swipY);
	}

}























