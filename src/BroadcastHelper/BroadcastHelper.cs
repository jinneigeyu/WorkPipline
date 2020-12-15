using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BroadcastGlobal
{
    public class EventController
    {

        private readonly static Dictionary<Type, BaseEvent> events = new Dictionary<Type, BaseEvent>();
        // Captures the sync context for the UI thread when constructed on the UI thread 
        // in a platform agnositc way so it can be used for UI thread dispatching
        private readonly static SynchronizationContext syncContext = SynchronizationContext.Current;
        public static TEventType GetEvent<TEventType>() where TEventType : BaseEvent, new()
        {
            lock (events)
            {
                BaseEvent existEvent = null;
                if (!events.TryGetValue(typeof(TEventType), out existEvent))
                {
                    // 如果找不到key的item 就创建一个添加到字典中
                    TEventType newEvent = new TEventType();
                    newEvent.SynchronizationContext = syncContext;
                    events[typeof(TEventType)] = newEvent;
                    return newEvent;
                }
                else
                {
                    return (TEventType)existEvent;
                }
            }
        }
    }

    public abstract class BaseEvent
    {
        public SynchronizationContext SynchronizationContext { get; set; }
        protected BaseEvent() { }
    }

    public class SubEvent<TPayload> : BaseEvent
    {
        private Action<TPayload> _action;
        public virtual void Publish(TPayload payload)
        {
            _action?.Invoke(payload);
        }

        /// <summary>
        /// replace currunt delegate
        /// </summary>
        /// <param name="action"></param>
        public virtual void Subscribe(Action<TPayload> action)
        {
            _action = action;
        }

        /// <summary>
        /// Accumulate delegate
        /// </summary>
        /// <param name="action"></param>
        public virtual void SubscribeAccumu(Action<TPayload> action)
        {
            _action += action;

        }

        public virtual void UnSubscribe(Action<TPayload> action)
        {
            _action -= action;
        }
    }

    public class ActEvent : SubEvent<string>
    {

    }
}

namespace BroadcastGlobal.Lower
{
    /// <summary>
    ///  低级，不同的T 需要不同的Broadcast
    ///  可以将T 用一个基类约束 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class BroadcastLower<T>
    {
        static Dictionary<string, Action<T>> keyValues = new Dictionary<string, Action<T>>();
        public static void Resgister(string key, Action<T> e)
        {
            lock (keyValues)
            {
                keyValues.Add(key, e);
            }
        }

        public static void Publish(string key, T payload)
        {
            var t = keyValues.Where((sk) => { return key == sk.Key; });
            t?.First().Value?.Invoke(payload);
        }

        public static void UnRegister(string key)
        {
            lock (keyValues)
            {
                keyValues.Remove(key);
            }
        }

        public static void Release()
        {
            keyValues.Clear();
        }
    }

}

namespace BroadcastGlobal.Higher
{
    public class BaseBroadContent
    {

    }

    public class ExampleContent : BaseBroadContent
    {
        public string StringContent { get; set; }
    }

    public static class BroadcastHigher<T> where T : BaseBroadContent, new()
    {
        static Dictionary<string, Action<T>> keyValues = new Dictionary<string, Action<T>>();
        public static void Resgister(string key, Action<T> e)
        {
            lock (keyValues)
            {
                keyValues.Add(key, e);
            }
        }

        public static void Publish(string key, T payload)
        {
            var t = keyValues.TryGetValue(key, out Action<T> act);
            if (!t) return;
            act?.Invoke(payload);
        }

        public static void UnRegister(string key)
        {
            lock (keyValues)
            {
                keyValues.Remove(key);
            }
        }

        public static void Release()
        {
            keyValues.Clear();
        }
    }
}