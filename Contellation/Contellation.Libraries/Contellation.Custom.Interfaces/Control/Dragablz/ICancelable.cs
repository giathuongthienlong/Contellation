namespace Contellation.Custom.Interfaces.Control.Dragablz
{
    internal interface ICancelable : IDisposable
    {
        bool IsDisposed { get; }
    }
}
