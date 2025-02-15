using UnityEngine;

public class EditorNode : MonoBehaviour
{
    private void Awake()
    {
        if(GameEntry.Context==null)
        {
            return;
        }
        GameObject.Destroy(this.gameObject);
    }
}