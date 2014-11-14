using Jil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HiveQuery.Common
{
    public static class Helper
    {
        public static T Clone<T>(this T data)
        {
            return JSON.Deserialize<T>(JSON.Serialize(data));
        }

        public static void WaitChanged<T>(ObservableCollection<T> collection, Action callBack) where T : INotifyPropertyChanged
        {
            NotifyCollectionChangedEventHandler handler = null;
            PropertyChangedEventHandler handler2 = null;
            var action = new Action<object, EventArgs>((o, e) =>
            {
                collection.CollectionChanged -= handler;
                foreach (var item in collection)
                {
                    item.PropertyChanged -= handler2;
                }

                callBack();
            });
            handler = new NotifyCollectionChangedEventHandler(action);
            handler2 = new PropertyChangedEventHandler(action);
            collection.CollectionChanged += handler;
            foreach (var item in collection)
            {
                item.PropertyChanged += handler2;
            }
        }

        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}

namespace System
{
    public partial class Tuple
    {
        public static Tuple<T, T1> Create<T, T1>(T t, T1 t1)
        {
            return new Tuple<T, T1>(t, t1);
        }
    }
}