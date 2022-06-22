using System.Collections;
using UnityEngine;

public class Bed : MonoBehaviour
{
   [SerializeField] private int id;
   [SerializeField] private int width;
   [SerializeField] private int length;
   [SerializeField] private int regenTime = 10;
   [SerializeField] private GameObject sheafPrefab;
   
   private Vector3 _offset;

   private void OnEnable()
   {
      Reap.destroyed += DelayedFillCell;
   }
   private void OnDisable()
   {
      Reap.destroyed -= DelayedFillCell;
   }
   
   private void Start()
   {
      //_filled = new bool[width, length];
      
      _offset = new Vector3((float)-width/2+0.5f, 0, (float)-length/2+0.5f);
      
      for (int x = 0; x < width; x++)
         for (int y = 0; y < length; y++)
         {
            FillCell(new Vector2Int(x,y));
         }
   }

   private void FillCell(Vector2Int pos)
   {
      var sneaf = Instantiate(sheafPrefab, transform.position + new Vector3Int(pos.x, 0, pos.y) + _offset, Quaternion.identity).GetComponent<Sneaf>();
      sneaf.id = id;
      sneaf.pos = pos;
      //_filled[x, y] = true; 
   }

   private void DelayedFillCell(int id, Vector2Int pos)
   {
      if (id!=this.id) return;
      StartCoroutine(DelayedFillCellCor(pos));
   }
   private IEnumerator DelayedFillCellCor(Vector2Int pos)
   {
      yield return new WaitForSeconds(regenTime);
      FillCell(pos);
   }
}