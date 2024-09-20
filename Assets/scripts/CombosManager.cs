using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombosManager : MonoBehaviour {

	int total;
	int value;
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
            int comboID = 0;
            if (total < 10)
                comboID = 0;
            if (total < 20)
                comboID = 1;
            else
                comboID = 2;
            if (comboID > 0)
            {
                if (comboID == 1)
                    Data.Instance.events.OnGenericUIText("Pixel Combo!");
                else
                    Data.Instance.events.OnGenericUIText("Super Pixel Combo!");
                Data.Instance.events.OnScoreOn(total * (250 * comboID), Vector3.zero, -1, ScoresManager.types.COMBO);
                Data.Instance.events.OnSoundFX("combo", -1);
            }
		}	
			
	}
}
