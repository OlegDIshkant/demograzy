using System;


namespace Common
{
    public abstract class DisposableObject : IDisposable
    {
        public bool IsDisposed { get; private set; } = false;


        protected void ExceptionIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(this.ToString());
            }
        }


        public void Dispose()
        {
            if (!IsDisposed)
            {
                IsDisposed = true;
                OnDispose();
            }
        }


        protected virtual void OnDispose()
        {

        }
    }
}
