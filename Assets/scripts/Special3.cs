using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special3 : SceneObject
{

    public GameObject asset;
    bool hasBrokenFloor;

    public override void OnRestart(Vector3 pos)
    {
        base.OnRestart(pos);
        asset.SetActive(false);
        StartCoroutine(ActionsToBeDone());
        hasBrokenFloor = false;
    }
    IEnumerator ActionsToBeDone()
    {
        yield return new WaitForSeconds(1.25f);
        Data.Instance.events.OnBossSpecial((int)transform.localPosition.x);
        yield return new WaitForSeconds(0.5f);
        float randomValue = Random.Range(0, 5);
        Vector3 pos = transform.localPosition;
        pos.y += randomValue;
        transform.localPosition = pos;
        asset.SetActive(true);
        yield return new WaitForSeconds(0.3f);

    }
    public override void OnPool()
    {
        StopAllCoroutines();
        asset.SetActive(false);
    }
    public override void onDie()
    {
        addExplotion(0.2f);
    }
    void addExplotion(float _y)
    {
        if (hasBrokenFloor) return;
        hasBrokenFloor = true;
        Game.Instance.level.OnAddExplotion(transform.position, 6, Color.red);
    }
}
