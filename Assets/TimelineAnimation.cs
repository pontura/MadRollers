using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;

public class TimelineAnimation : MonoBehaviour {

	public List<TimelineData> timeLineData;
	int id = 0;
	Vector3 initialPosition;
	Vector3 initialRotation;
    bool isOn;

	void OnEnable () {
		id = 0;
		initialPosition = transform.position;
		initialRotation = transform.eulerAngles;
        isOn = true;
        Init ();
        
    }
	void Init()
	{
        if (!isOn)
            return;
        if (!gameObject.activeSelf)
            return;
		if (timeLineData == null)
			return;
		if (timeLineData.Count==0)
			return;
		if (timeLineData [id].rotate)
			RotateInTimeLine ();
		else if (timeLineData [id].move)
			MoveInTimeLine ();
	}
	Ease GetEaseType(TimelineData.easetypes type)
	{
		switch (type) {
		case TimelineData.easetypes.IN_OUT:
                return Ease.InCubic;
		case TimelineData.easetypes.OUT_IN:
                return Ease.OutCubic;
            default:
                return Ease.Linear;
		}
	}
	void MoveInTimeLine()
	{
		if (timeLineData [id].duration == 0)
			return;
        if (transform != null)
        {
            transform.DOMove(
                new Vector3(initialPosition.x + timeLineData[id].data.x, initialPosition.y + timeLineData[id].data.y, initialPosition.z + timeLineData[id].data.z),
                timeLineData[id].duration).OnComplete(TweenCompleted).SetEase(GetEaseType(timeLineData[id].easeType));
        }
	}
	void RotateInTimeLine()
	{
		if (timeLineData [id].duration == 0)
			return;
        if (transform != null)
        {
            transform.DORotate(new Vector3(initialRotation.x + timeLineData[id].data.x, initialRotation.y + timeLineData[id].data.y, initialRotation.z + timeLineData[id].data.z),
            timeLineData[id].duration).OnComplete(TweenCompleted).SetEase(GetEaseType(timeLineData[id].easeType));
        }

    }
	void TweenCompleted()
	{
		id++;
		if (id >= timeLineData.Count)
			id = 0;
		Init ();
	}
    private void OnDisable()
    {
        isOn = false;
        Destroy(this);
    }
    public void OnComponentDisposed()
	{
        isOn = false;
        Destroy (this);
	}

}