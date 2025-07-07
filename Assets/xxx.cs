using System.Collections.Generic;
using GamesTan.UI;
using UnityEngine;
using UnityEngine.UI;

public class xxx : MonoBehaviour, ISuperScrollRectDataProvider
{
    [Header("Basic")] public SuperScrollRect ScrollRect;
    public int Count = 500;
    private List<DemoCellData> Datas = new List<DemoCellData>();
    private void Awake()
    {
        for (int i = 0; i < Count; i++)
        {
            Datas.Add(new DemoCellData()
            {
                Idx = i,
                Name = "Cell " + i,
                Count = UnityEngine.Random.Range(1, 10)
            });
        }

        ScrollRect.DoAwake(this);
        DoAwake();
    }

    protected virtual void DoAwake()
    {
    }

    public int GetCellCount()
    {
        return Datas.Count;
    }

    public void SetCell(GameObject cell, int index)
    {
        print("setcell ");
        var item = cell.GetComponent<DemoCell>();
        item.BindData(Datas[index]);
    }

    public class DemoCell : MonoBehaviour, IScrollCell
{
    public Button BtnItem;
    //public Text TextCount;
   // public Text TextName;

    private DemoCellData _data;

    public void BindData(DemoCellData data)
    {
        _data = data;
        BtnItem.onClick.RemoveListener(OnClick_BtnItem);
        BtnItem.onClick.AddListener(OnClick_BtnItem);
       // TextCount.text = data.Count.ToString();
       // TextName.text = data.Name.ToString();
        name = "Cell " + data.Idx;
    }

    void OnClick_BtnItem()
    {
        UnityEngine.Debug.Log(" Click " + _data);
    }
}

public class DemoCellData
    {
        public int Idx;
        public string Name;
        public int Count;

        public override string ToString()
        {
            return $"Idx:{Idx} Name:{Name} Count:{Count}";
        }
    }


   
    }