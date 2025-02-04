using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject fireBall;     // Fireball�� ����
    public Transform fireBallPort;  // FireballPort�� ����
    public Transform fireRockPort;  // FireRockPort�� ����
    public GameObject fireRock;      // FireRock�� ����

    Vector3 lookVec;
    Vector3 tauntVec;   // taunt�������� ��� �������� �����ϴ� ����
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
            StopAllCoroutines(); // ����� ��� �ڷ�ƾ ����
            return;
        }
        {
            
        }
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);    // Ÿ�� �������� �����ؼ� �ٶ�
        }
        else
        {
            nav.SetDestination(tauntVec);   // �������ݽ� ��ǥ�������� �̵��ϵ��� ���� �߰�
        }
    }
    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        // ���� �ൿ ����
        int ranAction = Random.Range(0, 4);
        switch(ranAction)
        {
            case 0:
            case 1:
                // ���̾ ����
                StartCoroutine(FireBall());

                break;
            case 2:
            case 3:
                // ���̾�� ����
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
    /* ������ ����ߴ�
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
