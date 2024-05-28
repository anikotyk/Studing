using GameBasicsSignals;

namespace GameCore.Common.Misc
{
    public interface IWindowShowable
    {
        TheSignal onWindowClosed { get; }
    }
}