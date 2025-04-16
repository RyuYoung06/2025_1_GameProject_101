using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("�⺻ �̵� ����")]
    public float jumpForce = 7f;
    public float moveSpeed = 5f;
    public float TurnSpeed = 10f;

    [Header("���� ���� ����")]
    public float faliMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.0f;

    [Header("���� ���� ����")]
    public float coyoteTime = 0.15f;
    public float coyoteTimeCounter;
    public bool realGrouned = true;

    [Header("�۶��̴� ����")]
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
        //���� ���� ����ȭ
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
                //�۶��̴� Ȱ��ȭ �Լ�
                EnableGlider();
            }
            //�۶��̴� ��� �ð� ����
            gliderTimeLeft -= Time.deltaTime;

            if(gliderTimeLeft <= 0)
            {
                //�۶��̴� ��Ȱ��ȭ �Լ�
                DisableGlider();
            }
        }
        else if (isGliding)
        {
            //GŰ�� ���� ��Ȱ��ȭ
            DisableGlider();
        }

        if(isGliding)
        {
            //�۶��̴� ����� �̵�
            ApplyGliderMovement(moveHoriziatal, moveVertical);
        }
        else
        {
            //�ӵ��� �����̵�
            rb.velocity = new Vector3(moveHoriziatal * moveSpeed, rb.velocity.y, moveVertical * moveSpeed);

            //���� ���� ���� ����
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (faliMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        //���� �Է�
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

        //�۶��̴� ��Ȱ��ȭ �Լ�
        void EnableGlider()
        {
            isGliding = true;

            if(gliderObject != null)
            {
                gliderObject.SetActive(true);
            }

            rb.velocity = new Vector3(rb.velocity.x , -gliderFallSpeed, rb.velocity.z);
        }

       //�۶��̴� ��Ȱ��ȭ �Լ�
       void DisableGlider()
        {
            isGliding = false;

            if (gliderObject != null)
            {
                gliderObject.SetActive(false);
            }

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        }

        //�۶��̴� �̵� ����
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
            Debug.Log($"���μ��� : {coinCount}/{totalCoins}");
        }

        if (other.gameObject.tag == "Door" && coinCount == totalCoins)
        {
            Debug.Log("����Ŭ����");
        }
    }

    //���� ���� ������Ʈ
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




