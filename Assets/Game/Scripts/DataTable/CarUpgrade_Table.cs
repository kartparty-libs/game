using UnityEngine;

public partial class CarUpgrade_Table
{
    public CarUpgrade_Data MinLevelData;
    public CarUpgrade_Data MaxLevelData;
    protected override void onPostAfter()
    {
        base.onPostAfter();
        int min = int.MaxValue;
        int max = int.MinValue;
        
        foreach (var item in this.DataList)
        {
            if (item.Id < min)
            {
                min = item.Id;
                MinLevelData = item;
            }
            if (item.Id > max)
            {
                max = item.Id;
                MaxLevelData = item;
            }
        }
    }
}