using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class ScoreSignal : SceneObject
{
    [SerializeField] private TextMesh field;

    public override void OnRestart(Vector3 pos)
    {
        base.OnRestart(pos);
        transform.DOMoveY(pos.y + 2, 0.5f).OnComplete(Pool);
    }
    public void SetScore(int playerID, int qty)
    {
		if (playerID > 3 || playerID <0)
			return;
		field.color = Data.Instance.GetComponent<MultiplayerData>().colors[playerID];
        field.text = "+" + qty.ToString();
    }
}
