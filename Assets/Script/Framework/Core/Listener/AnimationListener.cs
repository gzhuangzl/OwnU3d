using UnityEngine;
using System.Collections.Generic;
using System;

namespace Framework
{
    public class AnimationListener : MonoBehaviour
    {
        protected Dictionary<MatchModel,Dictionary<string, HashSet<EventListenerInfo>>> listeners;
        protected virtual void Start()
        {
            listeners = new Dictionary<MatchModel, Dictionary<string, HashSet<EventListenerInfo>>>();
        }
        protected virtual void OnAnimationEvent(AnimationEvent animationEvent)
        {
            string eventName = animationEvent.stringParameter;
        }

        public virtual void AddListener(string eventName, AnimationEventListenerFunction listener, MatchModel eventNameMatchModel = MatchModel.Whole)
        {
            GetListenerList(eventName,eventNameMatchModel,true).Add(new EventListenerInfo(eventName, listener, eventNameMatchModel));
        }

        public virtual void RemoveListener(string eventName, AnimationEventListenerFunction listener, MatchModel eventNameMatchModel = MatchModel.Whole)
        {
            HashSet<EventListenerInfo> set = GetListenerList(eventName, eventNameMatchModel);
            if (set != null)
            {
                set.Remove(new EventListenerInfo(eventName, listener, eventNameMatchModel));
            }
        }

        public virtual void RemoveAllListener(string eventName)
        {
            foreach (MatchModel model in Enum.GetValues(typeof(MatchModel)))
            {
                HashSet<EventListenerInfo> set = GetListenerList(eventName, model);
                if (set != null)
                {
                    set.Clear();
                    listeners[model].Remove(eventName);
                }
            }
        }

        protected virtual HashSet<EventListenerInfo> GetListenerList(string eventName,MatchModel matchModel,bool isAutoCreateOnNonexistent = false)
        {
            Dictionary<string, HashSet<EventListenerInfo>> eventListenerList = null;
            if (!listeners.ContainsKey(matchModel))
            {
                if (isAutoCreateOnNonexistent)
                {
                    eventListenerList = new Dictionary<string, HashSet<EventListenerInfo>>();
                    listeners.Add(matchModel, eventListenerList);
                }
            }
            else
            {
                eventListenerList = listeners[matchModel];
            }
            if (eventListenerList != null)
            {
                if (eventListenerList.ContainsKey(eventName))
                {
                    return eventListenerList[eventName];
                }
                else if (isAutoCreateOnNonexistent)
                {
                    HashSet<EventListenerInfo> listenerList = new HashSet<EventListenerInfo>();
                    eventListenerList.Add(eventName, listenerList);
                    return listenerList;
                }
            }
            return null;
        }

        public virtual void Clear()
        {
            if (listeners != null)
            {
                foreach (KeyValuePair<MatchModel, Dictionary<string, HashSet<EventListenerInfo>>> pair in listeners)
                {
                    foreach (KeyValuePair<string, HashSet<EventListenerInfo>> pair2 in pair.Value)
                    {
                        pair2.Value.Clear();
                    }
                    pair.Value.Clear();
                }
                listeners.Clear();
                listeners = null;
            }
        }

        protected virtual void OnDestroy()
        {
            Clear();
            listeners = null;
        }

        protected class EventListenerInfo : IEquatable<EventListenerInfo>
        {
            public readonly string eventName;
            public readonly AnimationEventListenerFunction listenerFunction;
            public MatchModel matchModel;
            private readonly int hashCode;
            public EventListenerInfo(string eventName, AnimationEventListenerFunction listenerFunction, MatchModel matchModel)
            {
                this.eventName = eventName;
                this.listenerFunction = listenerFunction;
                this.matchModel = matchModel;
                this.hashCode = EventListenerInfo.GetHashCode(eventName, listenerFunction);
            }

            public override int GetHashCode()
            {
                return hashCode;
            }

            public bool Equals(EventListenerInfo other)
            {
                return this.eventName == other.eventName && this.listenerFunction == other.listenerFunction;
            }
            public static int GetHashCode(string eventName, AnimationEventListenerFunction listenerFunction)
            {
                return (eventName + listenerFunction.ToString()).GetHashCode();
            }
        }
    }
}
