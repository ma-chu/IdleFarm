using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

    private Vector2 _origin;                         // start point касания
    private Vector2 _direction;                      // направление движения

    [SerializeField] private float smoothing = 100f; // k быстроты перемещения
    private Vector2 _smoothDirection;                // сглаженное направление движения

    private bool _touched;                           // было касание тачпада - заблокировать его для других пальцев
    private int _pointerID;                          // track касания 
    
    [SerializeField] private GameObject visualization; 
    [SerializeField] private RectTransform joystick; 

    void Awake() 
    {
        _direction = Vector2.zero;
        _touched = false;
    }

    public void OnPointerDown (PointerEventData data) 
    {
        if (_touched) return;
        
        _touched = true;
        _pointerID = data.pointerId;             // по сути: какой палец на тачпаде
        _origin = data.position;

        visualization.transform.position = _origin;
        visualization.SetActive(_touched);
        //Debug.Log("OnPointerDown");
    }

    public void OnDrag (PointerEventData data)
    {
        if (data.pointerId == _pointerID)
        {
            Vector2 currentPosition = data.position;
            Vector2 directionRaw =  _origin - currentPosition;
            _direction = directionRaw.normalized;
            
            joystick.anchoredPosition = -directionRaw.normalized * 50;
            //Debug.Log("OnDrag: " + joystick.anchoredPosition);
        }
    }

    public void OnPointerUp (PointerEventData data)  // ресет всего
    {
        if (data.pointerId == _pointerID)
        {
            _direction = Vector2.zero;
            _touched = false;
            
            joystick.localPosition = Vector3.zero;
            visualization.SetActive(_touched);
        }
        //Debug.Log("OnPointerUp");
    }

    public Vector2 GetDirection()                     // возвращаем направление перемещения
    {
        _smoothDirection = Vector2.MoveTowards(_smoothDirection, _direction, smoothing);
        return _smoothDirection;
    }
}

