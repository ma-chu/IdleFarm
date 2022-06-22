using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Barn : MonoBehaviour
{
    private static Barn _instance;
    public static Barn Instance => _instance;
    private void Awake() => _instance ??= this;
    
    [SerializeField] private int stackCapacity = 40;
    [SerializeField] private int stackPrice = 15;
    [Space (height:5f)]
    [SerializeField] private Transform shopWindow;
    [SerializeField] private FlyParticleEffect coinPrefab;
    [SerializeField] private Image coinImage;    
    [SerializeField] private TextMeshProUGUI coinText;
    [Space (height:5f)]
    [SerializeField] private Transform stack;
    [SerializeField] private FlyParticleEffect grassPrefab;
    [SerializeField] private Image stackFillImage;  
    [SerializeField] private TextMeshProUGUI stackText;
    [Space (height:5f)]
    [SerializeField] private TextMeshProUGUI fullText;
    private int _coins;
    private int _stackCount;

    public bool CheckStackNotEmpty() => _stackCount>0;
    public bool CheckStackNotFull()
    {
        var result = _stackCount < stackCapacity;
        if (!result) fullText.gameObject.SetActive(true);
        return result;
    }

    private void SellStack(int count)
    {
        for (int i = 0; i < count; i++)
        {
            var sellParticle = Instantiate(coinPrefab, shopWindow);
            if (i == count-1)
            {
                sellParticle.Init(shopWindow, coinImage.transform, ()=>{
                    AddCoins();
                    EndCoinAnim();
                });
                break;
            }
            sellParticle.Init(shopWindow, coinImage.transform, AddCoins);
        }

        StartCoinAnim();
    }

    private Sequence _sequence;
    private void StartCoinAnim()
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(coinImage.transform.DOLocalRotate(Vector3.forward * 20f, 0.15f));
        _sequence.Append(coinImage.transform.DOLocalRotate(Vector3.back * 10f, 0.15f));
        _sequence.SetLoops(-1);
    }
    
    private void EndCoinAnim()
    {
        _sequence.Kill();
    }
    
    private void AddCoins()
    {
        _coins++;
        coinText.text = _coins.ToString();
    }
    
    
    public void UnloadStack()
    {
        var c = _stackCount;
        for (int i = 0; i < _stackCount; i++)
        {
            var unloadParticle = Instantiate(grassPrefab, stack.position, Quaternion.identity, stack);
            if (i == _stackCount-1)
            {
                unloadParticle.Init(shopWindow, coinImage.transform, ()=>
                {
                    UpdateStack(false);
                    SellStack(c*stackPrice);
                });
                break;
            }
            unloadParticle.Init(stack, transform, ()=>UpdateStack(false));
        }

        fullText.gameObject.SetActive(false);
    }

    public void UpdateStack(bool plus)
    {
        if (plus) _stackCount++; else _stackCount--;
        stackText.text = String.Format("{0}/{1}", _stackCount.ToString(), stackCapacity.ToString());
        stackFillImage.fillAmount = (float)_stackCount / stackCapacity;
    }
}


