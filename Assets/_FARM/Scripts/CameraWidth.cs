using System;
using Cinemachine;
using UnityEngine;

public class CameraWidth : MonoBehaviour
{
    [SerializeField] private Vector2 DefaultResolution = new Vector2(720, 1280);
    [Range(0f, 1f)] [SerializeField] private float WidthOrHeight = 0;
    
    private CinemachineVirtualCamera _componentCamera;
    
    private float _targetAspect;
    private float _initialFov;
    private float _horizontalFov/* = 90f*/;

    private void Start()
    {
        _componentCamera = GetComponent<CinemachineVirtualCamera>();
        _targetAspect = DefaultResolution.x / DefaultResolution.y;     // заданное соотношение сторон
        
        _initialFov = _componentCamera.m_Lens.FieldOfView;             // начальный вертикальный угол камеры
        
        _horizontalFov = CalcVerticalFov(_initialFov, 1 / _targetAspect); // нужный горизонтальный угол камеры, чтоб обеспечить aspectRatio   
        
        float constantWidthFov = CalcVerticalFov(_horizontalFov, _componentCamera.m_Lens.Aspect); // нужный вертикальный угол камеры, чтоб обеспечить aspectRatio
        _componentCamera.m_Lens.FieldOfView = Mathf.Lerp(constantWidthFov, _initialFov, WidthOrHeight);    // задаем его с учетом слайдера ширины/высоты
    }

    // Сделать из вертикального угла камеры горизантальный по соотношению сторон (и наоборот)
    private float CalcVerticalFov(float hFovInDeg, float aspectRatio)
    {
        float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

        float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

        return vFovInRads * Mathf.Rad2Deg;
    }

    private void Update()
    {
        float constantWidthFov = CalcVerticalFov(_horizontalFov, _componentCamera.m_Lens.Aspect); // нужный вертикальный угол камеры, чтоб обеспечить aspectRatio
        _componentCamera.m_Lens.FieldOfView = Mathf.Lerp(constantWidthFov, _initialFov, WidthOrHeight);
    }
}