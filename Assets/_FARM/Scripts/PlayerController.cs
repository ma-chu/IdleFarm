using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TouchPad touchPad;
    [SerializeField] private float speed = 2f;
    
    private Animator _anim;
    public Collider coll;

    private bool _isWalking;
    public bool isAttacking;
    public bool isUnloading;
    public GameObject Sickle { get; private set; }

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        coll = GetComponentInChildren<Collider>();
        Sickle = GetComponentInChildren<Sickle>().gameObject;
        Sickle.SetActive(false);
    }
    
    private void FixedUpdate()
    {
        if (isAttacking) return;
        if (isUnloading) return;
        Walk();
    }

    private void Walk()
    {
        var dir = touchPad.GetDirection();
        
        if (dir == Vector2.zero)
        {
            if (_isWalking) SetWalking(false);
            return;
        }
        
        var dirV3 = new Vector3(dir.x, 0f, dir.y);
            
        transform.rotation = Quaternion.LookRotation(dirV3, Vector3.up);

        var scaledDirV3 = Time.deltaTime * speed * dirV3;
        var pos = transform.position + scaledDirV3;
        pos.z = Mathf.Clamp(pos.z, -3f, 6f);
        pos.x = Mathf.Clamp(pos.x, pos.z/3-4f, 4f-pos.z/3);    // линейную ф-ию посчитал по точкам: x = -z/3+4. Впоследствии привязать к параметрам камеры
        transform.position = pos;
        
        if (Barn.Instance.CheckStackNotEmpty() && transform.position.z <= -3) Unload(); 
        
        if (!_isWalking) SetWalking(true);
    }

    private void SetWalking(bool value)
    {
        _isWalking = value;
        _anim.SetBool("IsWalking", value);
    }

    private void Attack()
    {
        SetWalking(false);
        _anim.SetTrigger("Attack");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking) return;
        if (!other.CompareTag("Grass")) return;

        var dirV3 = other.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(dirV3, Vector3.up);
            
        Attack();
    }
    
    private void Unload()
    {
        transform.rotation = Quaternion.identity;
        SetWalking(false);
        isUnloading = true;
        _anim.SetTrigger("Unload");
    }
}
