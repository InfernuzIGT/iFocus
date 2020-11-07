using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Settings Asset", menuName = "ScriptableObjects/Settings", order = 2)]
public class SettingsSO : ScriptableObject
{
    [Header("Camera Settings")]
    [SerializeField] public float _waitAfterTransition = 0f;
    [SerializeField] public float _transitionSpeed = 1f;
    [SerializeField] public float _defaultZoomValue = 40;
    [SerializeField] public float _zoomInValue = 16;
    [SerializeField] public float _rotationSpeedFactor = 0.15f;
    [SerializeField] public float _rotationDeadPoint = 0.05f;
    [SerializeField] public float _waitBeforeTransition = 0f;
    [SerializeField] public float _waitAfterZoomIn = 0f;
}
