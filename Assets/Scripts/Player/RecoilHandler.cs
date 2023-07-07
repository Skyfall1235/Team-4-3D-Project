using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilHandler : MonoBehaviour
{
    Vector3 targetRotation;
    Vector3 currentRotation;

    public void HandleRecoil(float returnSpeed, float snapiness)
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snapiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    public void RecoilFire(Vector3 recoilAmounts)
    {
        targetRotation += new Vector3(recoilAmounts.x, Random.Range(-recoilAmounts.y, recoilAmounts.y), Random.Range(-recoilAmounts.z, recoilAmounts.z));
    }
}
