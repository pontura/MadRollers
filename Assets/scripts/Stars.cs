using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Stars : MonoBehaviour {

	[SerializeField] 
    Image star1;
    [SerializeField]
    Image star2;
    [SerializeField]
    Image star3;

	public void Init(int stars)
    {
		Color grey = new Color (0,0,0);
        if (stars == 0)
        {
			star1.color = grey;
			star2.color = grey;
			star3.color = grey;
        }
        else if (stars == 1)
        {
			star2.color = grey;
			star3.color = grey;
        }
        else if (stars == 2)
        {
			star3.color = grey;
        }
	}
}
