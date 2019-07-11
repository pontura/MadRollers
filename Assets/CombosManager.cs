using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombosManager : MonoBehaviour {

	public int total;
	public int value;
	float lastAreaIDChecked;

	public void GetNewItemToCombo(float areaID, int total)
	{		

		this.total = total;
		if(lastAreaIDChecked != areaID)
			value = 0;

		lastAreaIDChecked = areaID;

		if(total<7)
			return;

		value++;
		if(value>=total)
		{
			Data.Instance.events.OnGenericUIText("Perfect Combo! (x" + total + ")");
			Data.Instance.events.OnScoreOn (total * 100, Vector3.zero, -1, ScoresManager.types.COMBO);
		}	
			
	}
}
