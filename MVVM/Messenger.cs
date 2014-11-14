using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MVVM
{
    public static class Messenger
    {
        private static readonly object m_Lock = new object();

        private static ConcurrentDictionary<object, List<WeakReference<ViewModelBase>>>
            m_Tokens = new ConcurrentDictionary<object, List<WeakReference<ViewModelBase>>>();

        private static List<WeakReference<ViewModelBase>> m_Models
            = new List<WeakReference<ViewModelBase>>();

        public static void Register(ViewModelBase viewMode)
        {
            if (viewMode == null) return;
            var model = new WeakReference<ViewModelBase>(viewMode);
            lock (m_Lock)
            {
                m_Models.Add(model);
                List<WeakReference<ViewModelBase>> list = null;
                if (!m_Tokens.TryGetValue(viewMode.Token, out list))
                {
                    list = new List<WeakReference<ViewModelBase>>();
                    m_Tokens.TryAdd(viewMode.Token, list);
                }
                list.Add(model);
            }
        }

        public static void Send(object token, IMessage message)
        {
            if (token == null || message == null) return;
            List<WeakReference<ViewModelBase>> list = null;
            if (m_Tokens.TryGetValue(token, out list))
            {
                List<WeakReference<ViewModelBase>> nullList
                    = new List<WeakReference<ViewModelBase>>();
                foreach (var item in list)
                {
                    ViewModelBase model = null;
                    if (item.TryGetTarget(out model))
                        model.Receive(message);
                    else
                        nullList.Add(item);
                }

                lock (m_Lock)
                {
                    nullList.ForEach(i => list.Remove(i));
                }
            }
        }

        public static void SendAll(IMessage message)
        {
            List<WeakReference<ViewModelBase>> nullList
                    = new List<WeakReference<ViewModelBase>>();
            foreach (var item in m_Models)
            {
                ViewModelBase model = null;
                if (item.TryGetTarget(out model))
                    model.Receive(message);
                else
                    nullList.Add(item);
            }

            lock (m_Lock)
            {
                nullList.ForEach(i => m_Models.Remove(i));
            }
        }
    }
}