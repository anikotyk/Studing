using System;
using GameBasicsSDK.Modules.IdleArcade.DataConfigs;

[Serializable]
public class PriceProduct
{
    public ProductDataConfig config;
    public int price;
    private int _count = 0;
    public virtual int count => _count;

    public void SetCount(int value)
    {
        _count = value;
    }
    
    public void IncreaseCount(int value)
    {
        _count += value;
    }

    public bool IsBought()
    {
        return count >= price;
    }
}