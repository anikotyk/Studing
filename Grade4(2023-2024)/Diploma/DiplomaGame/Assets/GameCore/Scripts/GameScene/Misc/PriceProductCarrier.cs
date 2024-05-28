using System;
using GameBasicsSDK.Modules.IdleArcade.LevelItems.StackingCarriers.Products;

[Serializable]
public class PriceProductCarrier : PriceProduct
{
    public ProductsCarrier carrier;
    public override int count => carrier.count;
}