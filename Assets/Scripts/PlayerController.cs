using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;     // 플레이어 기본 속도
    public float jumpPower = 10.0f; // 플레이어 점프 높이
    public GameObject[] weapons;    // 무기 저장
    public bool[] hasWeapons;        // 

    public AudioSource weaponSound;
   

    public int key;     // 열쇠 아이템
    public int coin;    // 동전
    public int health;  // 체력 아이템
    public GameManager manager;
    public int maxKey;     // 최대 열쇠 아이템
    public int maxCoin;    // 최대 동전
    public int maxHealth;  // 최대 체력 아이템

    int jumpCount = 0;              // 플레이어 높이횟수 카운트

    float hAxis;
    float vAxis;

    bool jDown;
    bool iDown;         // 아이템 획득키
    bool sDown1;        // sword8
    bool sDown2;        // sword12
    bool sDown3;        // sword14
    bool ADown;         // 공격키
    bool isAttackReady; // 공격준비 여부
    bool isDamage;      // 플레이어가 피격당한 후 잠시 무적시간을 가지기 위해 선언
    bool isDead;

    Vector3 moveVec;
    Animator anim;
    Rigidbody rb;
    
    Material mesh;
    Color oriColor;     // 피격후 원래색깔로 돌아오기 위한 변수

    GameObject nearObject;      // 획득한 아이템을 저장
    Weapon equipWeapon;     // 기존에 장착된 무기를 저장하는 변수
    int equipWeaponIndex = -1;
    float fireDelay;            // 공격 딜레이

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();    
        mesh = GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Attack();
        Interation();

        Swap();
    }
    void FixedUpdate()
    {
        FreezeRotation();    
     
    }
    // 입력
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        jDown = Input.GetButtonDown("Jump");
        ADown = Input.GetButtonDown("Attack");

        iDown = Input.GetButtonDown("Interation");  // 아이템 획득키(단축키 c로 InputManager에서 설정)

        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }
    // 움직임
    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        if(isDead)
        {
            moveVec = Vector3.zero;
        }
        transform.position += moveVec * speed * Time.deltaTime;

        anim.SetBool("isRun", moveVec != Vector3.zero);
    }
    
    // 외부의 물리적 충돌에 의한 회전현상 제거
    void FreezeRotation()
    {   
        // 물리회전속도
        rb.angularVelocity = Vector3.zero;
    }
    // 플레이어 방향키에 따라 회전
    void Turn()
    {
        transform.LookAt(transform.position + moveVec * speed * Time.deltaTime);
    }
    // 점프
    public void Jump()
    {
        if(jDown && jumpCount < 1 && !isDead)    // 점프 상태가 아닐 때에만 점프
        {
            //rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);

            /*
            속도 계산 : 중력 값과 목표 높이를 기반으로 필요한 초기 속도 계산
            velocity = v
            unity 기본 중력값 = -9.81
            v = root[2gh] => g: 중력 가속도, h: 목표 높이
            AddForce 대신 velocity를 직접 조작하여 정확한 점프 높이를 계산
            */

            // Y축으로 10 정도의 점프를 위해 필요한 속도 계산
            float gravity = Mathf.Abs(Physics.gravity.y); // 중력의 크기
            float requiredVelocity = Mathf.Sqrt(2 * gravity * 3); // v = sqrt(2gh)

            // Rigidbody의 velocity를 직접 설정하여 점프 구현
            rb.velocity = new Vector3(rb.velocity.x, requiredVelocity, rb.velocity.z);

            // Animation Update
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

            jumpCount++;
        }
        
    }
    // 공격
    void Attack()
    {
        if(equipWeapon == null && !isDead) // 무기가 있을 때만 실행되도록 장비 체크
        {
            return;
        }
        // 공격 딜레이 시간을 더해주고 공격 가능 여부 확인
        fireDelay += Time.deltaTime;
        isAttackReady = equipWeapon.rate < fireDelay;

        if(ADown && isAttackReady)
        {
            equipWeapon.Use();
            weaponSound.Play();
            anim.SetTrigger("doAttack");
            fireDelay = 0;  // 공격을 한 번 했기 때문에 딜레이 0
        }
    }
    void Swap()
    {
        // 무기 중복교체, 없는 무기 확인
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;

        int weaponIndex = -1;

        if (sDown1) weaponIndex = 0;
        if (sDown2) weaponIndex = 1;
        if (sDown3) weaponIndex = 2;
        

        if (sDown1 || sDown2 || sDown3)
        {
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
        }
    }
    void Interation()
    {
        if(iDown && nearObject != null && !isDead)
        {
            if(nearObject.tag == "Weapon")
            {
                ItemGet item = nearObject.GetComponent<ItemGet>();
                int weaponIndex = item.value;   // weaPon아이템의 value값을 저장
                hasWeapons[weaponIndex] = true; // 아이템의 정보를 가져와서 해당 무기 입수여부를 체크

                Destroy(nearObject);
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            jumpCount = 0;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearObject = other.gameObject;
            Debug.Log("획득 오브젝트: " + nearObject.name);
        }
        
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearObject = null;
        }    
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            ItemGet item = other.GetComponent<ItemGet>();
            switch (item.type)
            {
                case ItemGet.Type.ClearKey:
                    key += item.value;
                    if (key > maxKey)
                        key = maxKey;
                    break;
                case ItemGet.Type.Heart:
                    health += item.value;
                    if (health > maxHealth)
                        health = maxHealth;
                    break;
                case ItemGet.Type.Coin:
                    coin += item.value;
                    if (coin > maxCoin)
                        coin = maxCoin;
                    break;
            }
            Destroy(other.gameObject);
        }
        // 플레이어 피격
        else if(other.tag == "EnemyAttack"){
            if(!isDamage)
            {
                Weapon enemyAttack = other.GetComponent<Weapon>();
                health -= enemyAttack.damage;

                bool isBossAtk = other.name == "BossMelee Area"; 
                StartCoroutine(OnDamage(isBossAtk));
            }
            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }
        }
    }
    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;

        oriColor = mesh.color;  // 원래 플레이어의 색깔을 저장
        mesh.color = Color.red;

        if (isBossAtk)
        {
            rb.AddForce(transform.forward * -25, ForceMode.Impulse);    // 넉백
        }

        yield return new WaitForSeconds(1f);    // 피격 후 무적시간
        isDamage = false;
        mesh.color = oriColor;

        if (isBossAtk)
        {
            rb.velocity = Vector3.zero;
        }
        if(health <= 0 && !isDead)
        {
            OnDie();
        }
    }
    void OnDie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
        manager.GameOver();
    }
}

