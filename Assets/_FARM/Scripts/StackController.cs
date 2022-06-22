using UnityEngine;
using DG.Tweening;

public class StackController : MonoBehaviour
{
    private void Awake()
    {
        DOTween.Init();
    }

    public void FootstepAnimation(float speed = 1f)
    {
        Sequence sequence = DOTween.Sequence();
        // следующий метод в последовательности после AppendCallback (как и все инструкции внутри AppendCallback) выполнится сразу, а не по окончании анимации
        // а если использовать sequence.Append(), то следующий метод выполнится по окончании анимации предыдущего
        sequence.AppendCallback(() =>    
        {
            transform.DOScaleX(1.7f, 0.1f/speed).SetEase(Ease.InQuart);
            transform.DOScaleY(1.2f, 0.1f/speed).SetEase(Ease.InQuart);
            transform.DOLocalMoveY(0.1f, 0.1f/speed).From(-0.3f).SetEase(Ease.InQuart);
        });
        sequence.AppendInterval(0.1f / speed);    // пауза перед следующим методом в последовательности
        sequence.AppendCallback(() =>    
        {
            transform.DOScaleX(1.5f, 0.1f/speed).SetEase(Ease.InQuart);
            transform.DOScaleY(1.0f, 0.1f/speed).SetEase(Ease.InQuart);
            transform.DOLocalMoveY(-0.2f, 0.1f/speed).SetEase(Ease.InQuart);
        });
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Reap")) return;
        Destroy(other.gameObject);
    }
}
