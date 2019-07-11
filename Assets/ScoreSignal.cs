using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreSignal : SceneObject
{
    public TextMesh field;
    public TextMesh field_outline;

    public override void OnRestart(Vector3 pos)
    {
        base.OnRestart(pos);
        
        Hashtable tweenData = new Hashtable();
        tweenData.Add("y", pos.y+2);
        tweenData.Add("time", 0.5f);
        tweenData.Add("easeType", iTween.EaseType.easeOutQuad);
        tweenData.Add("onComplete", "Pool");

        iTween.MoveTo(gameObject, tweenData);
    }
    public void SetScore(int playerID, int qty)
    {
		if (playerID > 3 || playerID <0)
			return;
		field.color = Data.Instance.GetComponent<MultiplayerData>().colors[playerID];
        field.text = "+" + qty.ToString();
      //  field_outline.text = "+" + qty.ToString();
    }
}
