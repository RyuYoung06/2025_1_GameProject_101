using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("기본 이동 설정")]
    public float jumpForce = 7f;
    public float moveSpeed = 5f;
    public float TurnSpeed = 10f;

    [Header("점프 개선 설정")]
    public float faliMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    [Header("지면 감지 설정")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrouned = true;

    [Header("글라이더 설정")]
    public GameObject gliderObject;
    public float gliderFallSpeed = 1.0f;
    public float gliderMoveSpeed = 7.0f;
    public float gliderMaxTime = 5.0f;
    public float gliderTimeLeft;
    public bool isGliding = false;  


    public bool isGrounded = true;

    public int coinCount = 0;
    public int totalCoins = 5;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        if(gliderObject != null)
        {
            gliderObject.SetActive(false);
        }
        gliderTimeLeft = gliderMaxTime;


        coyoteTimeCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //지면 감지 안정화
        UpdatGroundedState();


        float moveHoriziatal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHoriziatal, 0, moveVertical);

        if(movement.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);  
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.G) && !isGliding && gliderTimeLeft > 0)
        {
            if(!isGliding)
            {
                //글라이더 활성화 함수
                EnableGlider();
            }
            //글라이더 사용 시간 감소
            gliderTimeLeft -= Time.deltaTime;

            if(gliderTimeLeft <= 0)
            {
                //글라이더 비활성화 함수
                DisableGlider();
            }
        }
        else if (isGliding)
        {
            //G키를 때면 비활성화
            DisableGlider();
        }

        if(isGliding)
        {
            //글라이더 사용중 이동
            ApplyGliderMovement(moveHoriziatal, moveVertical);
        }
        else
        {
            //속도로 직접이동
            rb.velocity = new Vector3(moveHoriziatal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            //착지 점프 높이 구현
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (faliMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        //점프 입력
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            realGrouned = false;
            coyoteTimeCounter = 0;
        }

        if(isGliding)
        {
            if (isGliding)
            {
                DisableGlider();
            }

            gliderTimeLeft = gliderMaxTime;
        }

        //글라이더 ㅎ활성화 함수
        void EnableGlider()
        {
            isGliding = true;

            if(gliderObject != null)
            {
                gliderObject.SetActive(true);
            }

            rb.velocity = new Vector3(rb.velocity.x , -gliderFallSpeed, rb.velocity.z);
        }

       //글라이더 비활성화 함수
       void DisableGlider()
        {
            isGliding = false;

            if (gliderObject != null)
            {
                gliderObject.SetActive(false);
            }

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        }

        //글라이더 이동 적용
        void ApplyGliderMovement(float horizontal, float vertical)
        {
            Vector3 gliderVelcoity = new Vector3(
                horizontal * gliderMoveSpeed,
                -gliderFallSpeed,
                vertical * gliderMoveSpeed
            );

            rb.velocity = gliderVelcoity;
        }
}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            realGrouned = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log($"코인수집 : {coinCount}/{totalCoins}");
        }

        if (other.gameObject.tag == "Door" && coinCount == totalCoins)
        {
            Debug.Log("게임클리어");
        }
    }

    //지면 상태 업데이트
    void UpdatGroundedState()
    {
        if(realGrouned)
        {
            coyoteTimeCounter = coyoteTime;
            isGrounded = true;
        }
        else
        {
            if(coyoteTimeCounter > 0)
            {
                coyoteTimeCounter -= Time.deltaTime;
                isGrounded = true;
            }
            else
            {
                isGrounded = false; 
            }
        }
    }

}




