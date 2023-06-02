using UnityEngine;

namespace LnxArch
{
    public abstract class LnxComponentPairDerivation<TTarget, KSource, TValue, KValue> : LnxComponentDerivation<TTarget, TValue>
    where TTarget : LnxComponent<TValue>
    where KSource : LnxComponent<KValue>
    {
#region Properties
        private KSource _source;
#endregion

#region Abstract
        protected abstract KValue SyncOnWrite(TValue newValue, TTarget observedTarget);
        protected abstract TValue ReadReplacement(KSource source);
#endregion

#region Constructors
        [LnxInit]
        protected void PrepareDeriverSource(
            KSource source
        )
        {
            _source = source;
        }
#endregion

#region Private
        protected override void SyncOnWrite(TValue newValue)
        {
            KValue writtenValue = SyncOnWrite(observedTarget: _target, newValue: newValue);
            if (_debug) Debug.Log($"[{this.GetType().Name}-OnWrite] To: {_source.GetType().Name}:{writtenValue}");
            _source.Write(writtenValue);
        }

        protected override TValue ReadReplacement()
        {
            if (_debug) Debug.Log($"[{this.GetType().Name}-OnRead] From: {_source.GetType().Name}:{_source.Value}");
            return ReadReplacement(source: _source);
        }
#endregion
    }
}
