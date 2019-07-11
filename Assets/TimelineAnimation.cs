using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TimelineAnimation : MonoBehaviour {

	public List<TimelineData> timeLineData;
	int id = 0;
	Vector3 initialPosition;
	Vector3 initialRotation;

	void OnEnable () {
		id = 0;
		initialPosition = transform.position;
		initialRotation = transform.eulerAngles;
		Init ();
	}
	void Init()
	{
		if (timeLineData == null)
			return;
		if (timeLineData.Count==0)
			return;
		if (timeLineData [id].rotate)
			RotateInTimeLine ();
		else if (timeLineData [id].move)
			MoveInTimeLine ();
	}
	iTween.EaseType GetEaseType(TimelineData.easetypes type)
	{
		switch (type) {
		case TimelineData.easetypes.IN_OUT:
			return iTween.EaseType.easeInCubic;
		case TimelineData.easetypes.OUT_IN:
			return iTween.EaseType.easeOutCubic;
		default:
			return iTween.EaseType.linear;
		}
	}
	void MoveInTimeLine()
	{
		if (timeLineData [id].duration == 0)
			return;

		iTween.MoveTo(gameObject, iTween.Hash(
			"x", initialPosition.x + timeLineData[id].data.x,
			"y", initialPosition.y + timeLineData[id].data.y,
			"z", initialPosition.z + timeLineData[id].data.z,
			"islocal", false,
			"time", timeLineData[id].duration,
			"easetype", GetEaseType(timeLineData[id].easeType),
			"oncomplete", "TweenCompleted",
			"onCompleteTarget", this.gameObject
		));
	}
	void RotateInTimeLine()
	{
		if (timeLineData [id].duration == 0)
			return;
		iTween.RotateTo(gameObject, iTween.Hash(
			"x",  initialRotation.x + timeLineData[id].data.x,
			"y",  initialRotation.y + timeLineData[id].data.y,
			"z",  initialRotation.z + timeLineData[id].data.z,
			"islocal", false,
			"time", timeLineData[id].duration,
			"easetype", GetEaseType(timeLineData[id].easeType),
			"oncomplete", "TweenCompleted",
			"onCompleteTarget", this.gameObject
			// "axis", "x"
		));
	}
	void TweenCompleted()
	{
		id++;
		if (id >= timeLineData.Count)
			id = 0;
		Init ();
	}
	public void OnComponentDisposed()
	{
		iTween.Stop (this.gameObject);
		Destroy (this);
	}
//	void OnDisable()
//	{
//		//iTween.Stop (this.gameObject);
//	}
}