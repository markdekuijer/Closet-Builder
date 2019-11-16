using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ScrewSack : MonoBehaviour
{
    [SerializeField] private Transform followTransform;

    void Update()
    {
        transform.parent.position = new Vector3(followTransform.position.x, followTransform.position.y - 0.4f, followTransform.position.z);
        transform.parent.rotation = Quaternion.Euler(new Vector3(Quaternion.identity.eulerAngles.x, followTransform.eulerAngles.y ,Quaternion.identity.eulerAngles.z));
    }
}
