using UnityEngine;
using UnityEngine.EventSystems;

public class GameConfig : MonoBehaviour
{
    public EventSystem EventSystem;
    public Camera MainCamera;
    public Camera UICamera;
    public Transform EneityContainer;
    private void Start()
    {
        if (EventSystem.current != this.EventSystem)
        {
            EventSystem.current.enabled = false;
        }
    }
    public void Test(string value)
    {
        Debug.LogError("js call " + value);
    }
}