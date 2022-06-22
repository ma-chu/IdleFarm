using Cinemachine;
using EzySlice;
using UnityEngine;
using DG.Tweening;

public class Sickle : MonoBehaviour
{
    [SerializeField] private GameObject reapPrefab;
    [SerializeField] private Material sliceMaterial;

    private void Awake()
    {
        DOTween.Init();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Grass")) return;

        var sneaf = other.GetComponent<Sneaf>();
        var id = sneaf.id;                    // координаты снопа/сена
        var pos = sneaf.pos;                
        
        var hulls = 
            Slice(other.gameObject, other.transform.localPosition, other.transform.position - transform.position, sliceMaterial);
        
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() =>    
        {
            Destroy(other.gameObject);
            hulls[1].transform.DOLocalMoveY(hulls[1].transform.position.y + Random.Range(0.4f, 0.8f), 0.2f).SetEase(Ease.InQuart);     
        });
        sequence.AppendInterval(0.2f);
        sequence.AppendCallback(() =>  Destroy(hulls[0]));    // корешок
        sequence.Append(hulls[1].transform.DOLocalMoveY(0f, 0.2f).SetEase(Ease.InQuart));
        sequence.AppendCallback(() =>
        {
            var reap = Instantiate(reapPrefab, hulls[1].transform.position, Quaternion.Euler(-90,0,0)).GetComponent<Reap>();
            reap.id = id;
            reap.pos = pos;
            Destroy(hulls[1]);                                // вершок
        });
    }

    private GameObject[] Slice(GameObject objectToSlice, Vector3 planeWorldPosition, Vector3 planeWorldDirection, Material mat) 
    {
        return objectToSlice.SliceInstantiate(planeWorldPosition, planeWorldDirection, new TextureRegion(), mat);
    }
    
    
}
