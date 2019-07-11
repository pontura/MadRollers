using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSceneObjectManager : MonoBehaviour {

	public void AddComponentsToJson(AreaSceneObjectData newSOdata, GameObject go)
	{
		FullRotation fullRotation = go.GetComponent<FullRotation> ();
		TimelineAnimation timelineAnimation = go.GetComponent<TimelineAnimation> ();
		BossSettings bossSettings = go.GetComponent<BossSettings> ();
		MoveForward moveForward = go.GetComponent<MoveForward> ();
		Bumper bumper = go.GetComponent<Bumper> ();
		SceneObjectData soData = go.GetComponent<SceneObjectData> ();

		if (soData != null) {
			newSOdata.soData = new List<SceneObjectDataGeneric> ();
			SceneObjectDataGeneric data = new SceneObjectDataGeneric ();
			data.size = soData.size;
			data.random_pos_x = soData.random_pos_x;
			data.minPayers = soData.minPayers;
			newSOdata.soData.Add (data);
		} 
		if (bumper != null) {
			newSOdata.soData = new List<SceneObjectDataGeneric> ();
			SceneObjectDataGeneric data = new SceneObjectDataGeneric ();
			data.bumperForce = bumper.force;
			newSOdata.soData.Add (data);
		} 
		if (fullRotation != null) {
			newSOdata.fullRotationData = new List<FullRotationData> ();
			FullRotationData data = new FullRotationData ();
			data.rotateX = fullRotation.rotateX;
			data.rotateY = fullRotation.rotateY;
			data.rotateZ = fullRotation.rotateZ;
			data.speed = fullRotation.speed;
			data.random = fullRotation.randomRotation;
			newSOdata.fullRotationData.Add (data);
		} 
		if (timelineAnimation != null) {
			newSOdata.timelineAnimation = new List<TimelineAnimationData> ();
			TimelineAnimationData data = new TimelineAnimationData ();
			data.timeLineData = timelineAnimation.timeLineData;
			newSOdata.timelineAnimation.Add (data);
		}
		if (bossSettings != null) {
			newSOdata.bossSettings = new List<BossSettingsData> ();
			BossSettingsData data = new BossSettingsData ();
			data.bossModule = bossSettings.bossModule;
			data.asset = bossSettings.asset;
			data.time_to_init_enemies = bossSettings.time_to_init_enemies;
			data.time_to_kill = bossSettings.time_to_kill;
			data.distance_from_avatars = bossSettings.distance_from_avatars;
			newSOdata.bossSettings.Add (data);
		}
		if (moveForward != null) {
			newSOdata.moveForward = new List<MoveForwardData> ();
			MoveForwardData data = new MoveForwardData ();
			data.speed = moveForward.speed;
			data.randomSpeedDiff = moveForward.randomSpeedDiff;
			data.moveBackIn = moveForward.moveBackIn;
			newSOdata.moveForward.Add (data);
		}
	}
	public void AddComponentsToSceneObject(AreaSceneObjectData jsonData, GameObject so) 
	{
		if (jsonData.soData.Count > 0) {
			SceneObjectDataGeneric data = jsonData.soData [0];
			SceneObjectData sceneObjectData = so.GetComponent<SceneObjectData> ();
			Bumper bumper = so.GetComponent<Bumper> ();

			if (data.bumperForce > 0) {				
				if(bumper == null)
					bumper = so.gameObject.AddComponent<Bumper> ();
				bumper.force = data.bumperForce;
			}
			if (data.size != Vector3.zero) {
				
				if(sceneObjectData == null)
					sceneObjectData = so.gameObject.AddComponent<SceneObjectData> ();
				sceneObjectData.size = data.size;

			}
			if (data.random_pos_x != 0) {
				if(sceneObjectData == null)
					sceneObjectData = so.gameObject.AddComponent<SceneObjectData> ();

				sceneObjectData.random_pos_x = data.random_pos_x;
			}
			if (data.minPayers != 0) {
				if(sceneObjectData == null)
					sceneObjectData = so.gameObject.AddComponent<SceneObjectData> ();

				sceneObjectData.minPayers = data.minPayers;
			}
		}



		FullRotation fullRotation = so.GetComponent<FullRotation> ();
		if (jsonData.fullRotationData.Count > 0) {
			FullRotationData data = jsonData.fullRotationData [0];

			if(fullRotation == null)
				fullRotation = so.gameObject.AddComponent<FullRotation> ();
			fullRotation.rotateX = data.rotateX;
			fullRotation.rotateY = data.rotateY;
			fullRotation.rotateZ = data.rotateZ;
			fullRotation.speed = data.speed;
			fullRotation.randomRotation = data.random;
		} else if (fullRotation != null) {
			fullRotation.OnComponentDisposed ();
		}

		TimelineAnimation timelineAnimation = so.GetComponent<TimelineAnimation> ();
		if (jsonData.timelineAnimation.Count > 0) {
			TimelineAnimationData data = jsonData.timelineAnimation [0];
			if (timelineAnimation == null)
				timelineAnimation = so.gameObject.AddComponent<TimelineAnimation> ();
			timelineAnimation.timeLineData = data.timeLineData;
		} else if (timelineAnimation != null) {
			timelineAnimation.OnComponentDisposed ();
		}

		if (jsonData.bossSettings.Count > 0) {

			BossSettingsData data = jsonData.bossSettings [0];
			BossSettings newcomponent = so.GetComponent<BossSettings> ();

			if(newcomponent == null)
				newcomponent = so.gameObject.AddComponent<BossSettings> ();
			
			newcomponent.bossModule = data.bossModule;
			newcomponent.time_to_init_enemies = data.time_to_init_enemies;
			newcomponent.asset = data.asset;
			newcomponent.time_to_kill = data.time_to_kill;
			newcomponent.distance_from_avatars = data.distance_from_avatars;
		}
		if (jsonData.moveForward.Count > 0) {
			MoveForwardData data = jsonData.moveForward [0];
			MoveForward newcomponent = so.GetComponent<MoveForward> ();
			if(newcomponent == null)
				newcomponent = so.gameObject.AddComponent<MoveForward> ();
			newcomponent.speed = data.speed;
			newcomponent.randomSpeedDiff = data.randomSpeedDiff;
			newcomponent.moveBackIn = data.moveBackIn;
		}
	}
	public void ResetEveryaditionalComponent(SceneObject so)
	{
		TimelineAnimation timelineAnimation = so.GetComponent<TimelineAnimation> ();
		SceneObjectData sceneObjectData = so.GetComponent<SceneObjectData> ();
		FullRotation fullRotation = so.GetComponent<FullRotation> ();

		if (timelineAnimation != null) {
			timelineAnimation.OnComponentDisposed ();
		}
		if (sceneObjectData != null) {
			Destroy (sceneObjectData);
		}
		if (fullRotation != null) {
			fullRotation.OnComponentDisposed ();
		}
	}
}
