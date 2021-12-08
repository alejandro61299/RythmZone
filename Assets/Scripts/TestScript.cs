using UnityEngine;
using Core.EventManager;
using Core.SceneLoader;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            SceneLoader.LoadScene(SceneLoader.SceneID.MainMenu);
    }
}

public class TestEvent : EventData
{

}
