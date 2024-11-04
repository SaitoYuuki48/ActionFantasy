using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //public Rigidbody rd;

    //�ϐ�
    public Animator animator;
    //public float moveSpeed = 5.0f;
    //public float rotationSpeed = 1200.0f;

    //�ړ��֘A�̃p�����[�^
    //�����̍Œ�l
    private float minSpeed = 3.0f;
    //�����̍ō��l
    private float maxSpeed = 5.0f;
    //�v���C���[�̕����]���X�s�[�h�̒����l
    //0.0f���ƈ�،������ς�炸1.0f���Ɠ��͌シ�����͂��ꂽ�����֌���
    [SerializeField, Range(0.0f, 1.0f)]
    private float turnRate = 0.3f;

    //�ړ����x
    private Vector3 velocity;

    //�L�����N�^�[�R���g���[���[
    private CharacterController controller;

    //Inspector�ɂ�Range�̒l���w��͈̔͂Ɏ��܂�悤��Clamp����
    private void OnValidate()
    {
        this.turnRate = Mathf.Clamp(this.turnRate, 0.0f, 1.0f);
    }

    private Transform CamPos;
    private Vector3 Camforward;
    private Vector3 ido;
    private Vector3 Animdir = Vector3.zero;

    float runspeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        if (Camera.main != null)
        {
            CamPos = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
        }
    }

    //�����ݒ�
    void Awake()
    {
        //�L�����N�^�[�R���g���[���[�擾
        this.controller = this.GetComponent<CharacterController>();

        //���x���[���ɐݒ�
        this.velocity = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        KariMove();

        //Move();

        //if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        //{
        //    animator.SetBool("Run", false);
        //}
        //else
        //{
        //    var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        //    Vector3 direction = cameraForward * Input.GetAxis("Vertical") + Camera.main.transform.right * Input.GetAxis("Horizontal");
        //    animator.SetBool("Run", true);
        //    rd.MovePosition(rd.position + transform.TransformDirection(direction) * moveSpeed * Time.deltaTime);

        //    ChangeDirection(direction);
        //}
    }

    void LateUpdate()
    {
        CameraRotation();
    }

    //void ChangeDirection(Vector3 direction)
    //{
    //    Quaternion q = Quaternion.LookRotation(direction);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed * Time.deltaTime);
    //}

    //private void Move()
    //{
    //    float stick_H = Input.GetAxis("Horizontal");
    //    float stick_V = Input.GetAxis("Vertical");

    //    if (stick_V == 0 && stick_H == 0)
    //    {
    //        animator.SetBool("Run", false);
    //    }
    //    else
    //    {
    //        var direction = new Vector3(stick_H, 0, stick_V);
    //        rd.MovePosition(rd.position + transform.TransformDirection(direction) * moveSpeed * Time.deltaTime);
    //        animator.SetBool("Run", true);
    //    }
    //}

    private void KariMove()
    {
        Vector3 vec = this.velocity;
        float Speed = 0.0f;

        //���ɐڒn���Ă��������
        if (this.controller.isGrounded)
        {
            //�Q�[���p�b�h�̃X�e�B�b�N���͒l���擾���Ĉړ��x�N�g�����쐬
            //..���łɐڒn���Ă���̂�Y���̒l�����Z�b�g
            vec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            //���͒l���瓾���x�N�g���̒�����0.1f���z���Ă���Α�����ݒ�
            if (vec.magnitude > 0.1f)
            {
                //�X�e�B�b�N�̓|����ɂ���đ�����ύX
                Speed = Mathf.Lerp(this.minSpeed, this.maxSpeed, vec.magnitude);

                //�����̕ύX
                Vector3 dir = vec.normalized;
                float rotate = Mathf.Acos(dir.z);
                if (dir.x < 0) rotate = -rotate;
                rotate *= Mathf.Rad2Deg;
                Quaternion Q = Quaternion.Euler(0, rotate, 0);
                //�����Ń��f���̌����Ă�����������X�ɕς��悤�ɏ�������
                //���f���̌�����ύX
                this.transform.rotation = Quaternion.Slerp(
                          this.transform.rotation, Q, this.turnRate
                          );

                animator.SetBool("Run", true);
            }
            else
            {
                animator.SetBool("Run", false);
            }

            //�ړ��x�N�g���𐳋K��
            vec = vec.normalized;
        }

        //�ړ����x��ݒ�
        this.velocity.x = vec.x * Speed;
        this.velocity.y = vec.y;
        this.velocity.z = vec.z * Speed;

        //�d�͂ɂ�闎����ݒ�
        this.velocity.y += Physics.gravity.y * Time.deltaTime;

        //�ړ�������
        this.controller.Move(this.velocity * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        //�L�[�{�[�h���l�擾�B�v���C���[�̕����Ƃ��Ĉ���
        float h = Input.GetAxis("Horizontal");//��
        float v = Input.GetAxis("Vertical");//�c

        //�J������Transform���擾����Ă�Ύ��s
        if (CamPos != null)
        {
            //2�̃x�N�g���̊e�����̏�Z(Vector3.Scale)�B�P�ʃx�N�g����(.normalized)
            Camforward = Vector3.Scale(CamPos.forward, new Vector3(1, 0, 1)).normalized;
            //�ړ��x�N�g����ido�Ƃ����g�����X�t�H�[���ɑ��
            ido = v * Camforward * runspeed + h * CamPos.right * runspeed;
            //Debug.Log(ido);
        }

        //���݂̃|�W�V������ido�̃g�����X�t�H�[���̐��l������
        transform.position = new Vector3(
        transform.position.x + ido.x,
        0,
        transform.position.z + ido.z);

        //�����]���pTransform

        Vector3 AnimDir = ido;
        AnimDir.y = 0;
        //�����]��
        if (AnimDir.sqrMagnitude > 0.001)
        {
            Vector3 newDir = Vector3.RotateTowards(transform.forward, AnimDir, 5f * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        }

    }

    private void CameraRotation()
    {

    }
}
