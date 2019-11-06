using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHammerHelper : MonoBehaviour
{
    [SerializeField] private string targetTag = "Hammerable";
    private PlaceTask task;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
        }
    }
}
