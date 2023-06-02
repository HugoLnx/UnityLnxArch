using System;
using UnityEngine;

namespace LnxArch
{
    public abstract class LnxComponentDerivation<TTarget, TValue> : LnxBehaviour
    where TTarget : LnxComponent<TValue>
    {
#region Properties
        [SerializeField] private bool _writeOnUpdate = false;
        [SerializeField] private bool _writeOnFixedUpdate = false;
        [SerializeField] protected bool _debug;
        protected TTarget _target;
#endregion

#region Abstract
        protected virtual bool ReplaceReadingOnTarget => true;
        protected virtual bool SyncWhenWritingOnTarget => true;
        protected abstract void SyncOnWrite(TValue valueWritten);
        protected abstract TValue ReadReplacement();
#endregion

#region Constructors
        [LnxInit]
        protected void PrepareDeriver(
            [FromLocal(order: 0)]
            [FromEntity(order: 1)]
            TTarget target
        )
        {
            _target = target;
            SetupReadAndWriteHooks();
        }
#endregion

#region Lifecycle
        protected void Update()
        {
            if (!_writeOnUpdate) return;
            _target.Write(Read(), source: NewDerivationSource());
        }

        protected void FixedUpdate()
        {
            if (!_writeOnFixedUpdate) return;
            _target.Write(Read(), source: NewDerivationSource());
        }
#endregion

#region Private
        protected LnxComponentSource<TValue> NewDerivationSource()
        {
            return new() { Object = this };
        }
        private void SetupReadAndWriteHooks()
        {
            if (SyncWhenWritingOnTarget)
            {
                _target.OnWrite += (value, source) => {
                    bool isRepeated = System.Object.ReferenceEquals(source.Object, this);
                    if (_debug) Debug.Log($"[{this.GetType().Name}-OnWrite{(isRepeated ? "-Aborted" : "")}] value={value}");
                    if (isRepeated) return;
                    SyncOnWrite(value);
                };
            }
            if (ReplaceReadingOnTarget)
            {
                _target.OverwriteReadingTo(() => {
                    TValue value = ReadReplacement();
                    if (_debug) Debug.Log($"[{this.GetType().Name}-OnRead] {value}");
                    return value;
                });
            }
        }

        private TValue Read()
        {
            return ReplaceReadingOnTarget ? ReadReplacement() : _target.Value;
        }
#endregion
    }
}
