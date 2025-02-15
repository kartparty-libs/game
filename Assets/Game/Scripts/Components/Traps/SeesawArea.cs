using Framework;
using UnityEngine;
using Utility;

/// <summary>
/// 跷跷板
/// </summary>
public class SeesawArea : MonoBehaviour
{
    private float DownSpeedAngle = 10f;
    private float RecoverSpeedAngle = 10f;
    public float MaxAngle = 70f;
    public Transform Seesaw;
    private int m_nTriggerNum = 0;
    private float DownAngle = 0;
    private void Start()
    {
        if (Seesaw == null) Seesaw = this.transform.parent;
    }
    void Update()
    {
        if (m_nTriggerNum == 0 && Seesaw.localEulerAngles.z != 0)
        {
            float z = UtilityMethod.GetAngleByMinTurn(Seesaw.localEulerAngles.z, 0);
            float angle = z > 0 ? -RecoverSpeedAngle : RecoverSpeedAngle;
            Seesaw.localEulerAngles = new Vector3(Seesaw.localEulerAngles.x, Seesaw.localEulerAngles.y, z + angle * Time.deltaTime);
            if (Mathf.Abs(z) < 0.1) Seesaw.localEulerAngles = Vector3.zero;
        }
        if (DownAngle != 0)
        {
            Seesaw.localEulerAngles = new Vector3(Seesaw.localEulerAngles.x, Seesaw.localEulerAngles.y, Seesaw.localEulerAngles.z + DownAngle * DownSpeedAngle * Time.deltaTime);
        }
    }

    void LateUpdate()
    {
        DownAngle = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        m_nTriggerNum++;
    }
    void OnTriggerStay(Collider other)
    {
        ICharacter Character = other.transform.GetComponent<ICharacter>();
        if (Character != null)
        {
            Vector3 direction = Character.GetPosition() - Seesaw.position;
            var cross = Vector3.Cross(Seesaw.forward, direction);
            float angle = cross.y < 0 ? 1 : -1;
            DownAngle += angle;
        }
    }
    void OnTriggerExit(Collider other)
    {
        m_nTriggerNum--;
    }
}
