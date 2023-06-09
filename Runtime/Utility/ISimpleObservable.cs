using System;

namespace LnxArch
{
    public interface ISimpleObservable
    {
        event Action Callbacks;
    }
}
