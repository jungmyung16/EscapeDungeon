using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject fireBall;     // Fireball을 저장
    public Transform fireBallPort;  // FireballPort를 저장
    public Transform fireRockPort;  // FireRockPort를 저장
    public GameObject fireRock;      // FireRock을 저장

    Vector3 lookVec;
    Vector3 tauntVec;   // taunt공격으로 어디에 떨어질지 저장하는 벡터
    public bool isLook;

    public AudioSource fbSound;
    public AudioSource frSound;

    // Start is called before the first frame update
    void Start()
    {
        isLook = true;
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        //mat = GetComponent<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        nav.isStopped = true;
        StartCoroutine(Think());    
    }
    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines(); // 사망시 모든 코루틴 중지
            return;
        }
        {
            
        }
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);    // 타겟 방향으로 예측해서 바라봄
        }
        else
        {
            nav.SetDestination(tauntVec);   // 점프공격시 목표지점으로 이동하도록 로직 추가
        }
    }
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        // 보스 행동 패턴
        int ranAction = Random.Range(0, 4);
        switch(ranAction)
        {
            case 0:
            case 1:
                // 파이어볼 패턴
                StartCoroutine(FireBall());

                break;
            case 2:
            case 3:
                // 파이어락 패턴
                StartCoroutine(FireRock());
                break;
            
        }
    }

    IEnumerator FireBall()
    {
        fbSound.Play();
        anim.SetTrigger("doFB");
        yield return new WaitForSeconds(0.2f);
        GameObject instantFireball = Instantiate(fireBall, fireBallPort.position, fireBallPort.rotation);
        Fireball fireball = instantFireball.GetComponent<Fireball>();
        fireball.target = target;

        yield return new WaitForSeconds(2f);

        StartCoroutine(Think());
    }
    IEnumerator FireRock()
    {
        frSound.Play();
        isLook = false;

        anim.SetTrigger("doFR");
        Instantiate(fireRock, fireRockPort.position, fireRockPort.rotation);

        yield return new WaitForSeconds(3f);
        isLook = true;
        StartCoroutine(Think());
    }
    /* 오류로 사용중단
    IEnumerator Taunt()
    {
        tauntVec = target.position + lookVec;

        isLook = false;
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doTa");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;

        StartCoroutine(Think());
    }
    */
}
