using System.Collections;
using System.Collections.Generic;
using Framework;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using UnityEngine;

/// <summary>
/// 传送带
/// </summary>
public class ConveyorArea : MonoBehaviour
{
    public float Speed = 1.4f;
    public Vector3 Direction = new Vector3(0, 0, 0);
    private void OnTriggerStay(Collider other)
    {
        ICharacterMovement CharacterMovement = other.transform.GetComponent<ICharacterMovement>();
        if (CharacterMovement != null)
        {
            CharacterMovement.GetCharacterMovementController().ContinuousOffsetVelocity(Direction.normalized * Speed);
        }
    }
}
