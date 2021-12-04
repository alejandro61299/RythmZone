using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Event = Core.Event;


public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        EventManager.AddListener<TestEvent>(OnTest);
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<TestEvent>(OnTest);
    }

    private void OnDestroy()
    {
        
    }


    // Update is called once per frame
    void Start()
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
