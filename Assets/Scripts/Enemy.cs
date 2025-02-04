using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { normalM, Boss };
    public Type enemyType;
    public GameManager manager;
    public int maxHealth;               // 적의 최대체력
    public int curHealth;               // 현재 체력
    public Transform target;            // 적이 쫓을 목표
    public bool isChase;                // 추적을 결정하는 변수
    public BoxCollider meleeArea;   // 적의 공격범위
    public bool isAttack;               // 공격여부
    public bool isDead;

    public Rigidbody rb;
    public BoxCollider boxCollider;
    public Material mat;
    public NavMeshAgent nav;
    public Animator anim;
    public int cnt = 0;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        //mat = GetComponent<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if(enemyType != Type.Boss)
        {
            Invoke("ChasetStart", 2);
        }
        Invoke("ChaseStart", 1.5f);
    }
    void Targeting()
    {
        if(!isDead && enemyType != Type.Boss)
        {
            // sphereCast의 반지름 길이
            float targetRadius = 1.5f;
            float targetRange = 3f;

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position,           // 자신의 현재 위치
                                      targetRadius,
                                      transform.forward,            // 앞으로 rayCast발사
                                      targetRange,
                                      LayerMask.GetMask("Player"));

            // rayhit 변수에 데이터가 들어오면 공격 코루틴 실행
            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }
    IEnumerator Attack()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = false;

        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }
    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }
    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }
    void Update()
    {
        if (nav.enabled && enemyType != Type.Boss) // 내비게이션이 활성화 되어 있을때만
        {
            nav.SetDestination(target.position); // 도착할 목표 위치 지정
            nav.isStopped = !isChase;
        }
        
    }
    void FreezeVelocity()
    {
        if (isChase)
        {
            rb.velocity = Vector3.zero;
            // 물리회전속도
            rb.angularVelocity = Vector3.zero;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;         // 현재 체력에서 무기의 데미지 만큼 뺀다
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }    
    }
    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;

            mat.SetColor("_Color", Color.white);
            rb.AddForce(reactVec * 5, ForceMode.Impulse);
        }
        else
        {   // 적이 죽었을 경우 
            mat.SetColor("_Color", Color.gray);
            gameObject.layer = 7;
            isDead = true;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rb.AddForce(reactVec * 5, ForceMode.Impulse);
            cnt++;
            
            
            Destroy(gameObject, 2);
            if(enemyType == Type.Boss)
            {
                manager.GameClear();
            }
            
        }
    }
}
