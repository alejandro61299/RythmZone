using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.EventManager;
using UnityEngine;
using Event = Core.EventManager.Event;



public class TestScript : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.AddListener<TestEvent>(OnTest);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<TestEvent>(OnTest);
    }
    private void Start()
    {
        EventManager.TriggerEvent(new TestEvent());
    }

    private void OnTest(TestEvent eventData)
    {
        
    }
}

public class TestEvent : Event
{
    
}
