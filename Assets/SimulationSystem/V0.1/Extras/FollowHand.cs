using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHand : MonoBehaviour
{
    [SerializeField] private Transform anchorTransform;
    [SerializeField] private Transform camTransform;
    [SerializeField] private float verticalOffset;

    private void LateUpdate()
    {
        transform.DOMove(anchorTransform.position + Vector3.up * verticalOffset, 0.05f).SetEase(Ease.InOutCubic);
        transform.LookAt(camTransform);
    }
}
