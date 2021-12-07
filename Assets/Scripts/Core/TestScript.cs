using Core.Coroutines;
using Core.EventManager;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Awake()
    {
        EventManager.Register<TestEvent>(OnTest);
    }

    private void OnDestroy()
    {
        EventManager.Unregister<TestEvent>(OnTest);
    }
    private void Start()
    {
        EventManager.Trigger(new TestEvent());
    }

    private void OnTest(TestEvent eventData)
    {
        
    }
}

public class TestEvent : EventData
{
    
}
