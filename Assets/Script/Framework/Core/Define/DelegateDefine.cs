using UnityEngine;

namespace Framework
{
    public delegate void TimerHandler(ITimer timer, bool isFinish);
    public delegate void EventListenerFunction(int eventType, object userData);
    public delegate void AnimationEventListenerFunction(AnimationEvent animationEvent);
}
