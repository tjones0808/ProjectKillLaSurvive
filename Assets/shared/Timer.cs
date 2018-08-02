using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    private class TimedEvent
    {
        public float TimeToExecute;
        public Callback Method;

    }

    public delegate void Callback();

    private List<TimedEvent> events;

    private void Awake()
    {
        events = new List<TimedEvent>();
    }

    public void Add(Callback method, float inSeconds)
    {
        events.Add(new TimedEvent
        {
            Method = method,
            TimeToExecute = Time.time + inSeconds
        });
    }

    void Update()
    {
        if (events.Count == 0)
            return;

        for (int i = 0; i < events.Count; i++)
        {
            var evt = events[i];

            if (evt.TimeToExecute <= Time.time)
            {
                evt.Method();
                events.Remove(evt);
            }
        }

    }

}
