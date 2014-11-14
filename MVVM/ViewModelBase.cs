using System;

namespace MVVM
{
    public class ViewModelBase : Observer, IDisposable
    {
        public object Token { get; protected set; }

        public ViewModelBase(object token)
        {
            Token = token;
            Messenger.Register(this);
        }

        public ViewModelBase()
        {
            Token = this.GetType();
            Messenger.Register(this);
        }

        public virtual void Receive(IMessage message)
        {
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Cleanup()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Cleanup();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion IDisposable Support
    }
}