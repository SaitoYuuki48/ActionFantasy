using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //public Rigidbody rd;

    //変数
    public Animator animator;
    //public float moveSpeed = 5.0f;
    //public float rotationSpeed = 1200.0f;

    //移動関連のパラメータ
    //速さの最低値
    private float minSpeed = 3.0f;
    //速さの最高値
    private float maxSpeed = 5.0f;
    //プレイヤーの方向転換スピードの調整値
    //0.0fだと一切向きが変わらず1.0fだと入力後すぐ入力された方向へ向く
    [SerializeField, Range(0.0f, 1.0f)]
    private float turnRate = 0.3f;

    //移動速度
    private Vector3 velocity;

    //キャラクターコントローラー
    private CharacterController controller;

    //InspectorにてRangeの値が指定の範囲に収まるようにClamp処理
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

    //初期設定
    void Awake()
    {
        //キャラクターコントローラー取得
        this.controller = this.GetComponent<CharacterController>();

        //速度をゼロに設定
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

        //床に接地していたら歩く
        if (this.controller.isGrounded)
        {
            //ゲームパッドのスティック入力値を取得して移動ベクトルを作成
            //..ついでに接地しているのでY軸の値をリセット
            vec = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

            //入力値から得たベクトルの長さが0.1fを越えていれば速さを設定
            if (vec.magnitude > 0.1f)
            {
                //スティックの倒し具合によって速さを変更
                Speed = Mathf.Lerp(this.minSpeed, this.maxSpeed, vec.magnitude);

                //向きの変更
                Vector3 dir = vec.normalized;
                float rotate = Mathf.Acos(dir.z);
                if (dir.x < 0) rotate = -rotate;
                rotate *= Mathf.Rad2Deg;
                Quaternion Q = Quaternion.Euler(0, rotate, 0);
                //ここでモデルの向いている方向が徐々に変わるように処理しつつ
                //モデルの向きを変更
                this.transform.rotation = Quaternion.Slerp(
                          this.transform.rotation, Q, this.turnRate
                          );

                animator.SetBool("Run", true);
            }
            else
            {
                animator.SetBool("Run", false);
            }

            //移動ベクトルを正規化
            vec = vec.normalized;
        }

        //移動速度を設定
        this.velocity.x = vec.x * Speed;
        this.velocity.y = vec.y;
        this.velocity.z = vec.z * Speed;

        //重力による落下を設定
        this.velocity.y += Physics.gravity.y * Time.deltaTime;

        //移動させる
        this.controller.Move(this.velocity * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        //キーボード数値取得。プレイヤーの方向として扱う
        float h = Input.GetAxis("Horizontal");//横
        float v = Input.GetAxis("Vertical");//縦

        //カメラのTransformが取得されてれば実行
        if (CamPos != null)
        {
            //2つのベクトルの各成分の乗算(Vector3.Scale)。単位ベクトル化(.normalized)
            Camforward = Vector3.Scale(CamPos.forward, new Vector3(1, 0, 1)).normalized;
            //移動ベクトルをidoというトランスフォームに代入
            ido = v * Camforward * runspeed + h * CamPos.right * runspeed;
            //Debug.Log(ido);
        }

        //現在のポジションにidoのトランスフォームの数値を入れる
        transform.position = new Vector3(
        transform.position.x + ido.x,
        0,
        transform.position.z + ido.z);

        //方向転換用Transform

        Vector3 AnimDir = ido;
        AnimDir.y = 0;
        //方向転換
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
