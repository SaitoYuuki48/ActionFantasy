using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardExample : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("�N��");
    }

    // Update is called once per frame
    private�@void Update()
    {
        // ���݂̃L�[�{�[�h���
        var current = Keyboard.current;

        // �L�[�{�[�h�ڑ��`�F�b�N
        if (current == null)
        {
            // �L�[�{�[�h���ڑ�����Ă��Ȃ���
            // Keyboard.current��null�ɂȂ�
            return;
        }

        // A�L�[�̓��͏�Ԏ擾
        var aKey = current.aKey;

        // A�L�[�������ꂽ�u�Ԃ��ǂ���
        if (aKey.wasPressedThisFrame)
        {
            Debug.Log("A�L�[�������ꂽ�I");
        }

        // A�L�[�������ꂽ�u�Ԃ��ǂ���
        if (aKey.wasReleasedThisFrame)
        {
            Debug.Log("A�L�[�������ꂽ�I");
        }

        // A�L�[��������Ă��邩�ǂ���
        if (aKey.isPressed)
        {
            Debug.Log("A�L�[��������Ă���I");
        }

        if (Input.GetKeyDown("joystick button 0"))
        {
            Debug.Log("button0");
        }
        if (Input.GetKeyDown("joystick button 1"))
        {
            Debug.Log("button1");
        }
        if (Input.GetKeyDown("joystick button 2"))
        {
            Debug.Log("button2");
        }
        if (Input.GetKeyDown("joystick button 3"))
        {
            Debug.Log("button3");
        }
        if (Input.GetKeyDown("joystick button 4"))
        {
            Debug.Log("button4");
        }
        if (Input.GetKeyDown("joystick button 5"))
        {
            Debug.Log("button5");
        }
        if (Input.GetKeyDown("joystick button 6"))
        {
            Debug.Log("button6");
        }
        if (Input.GetKeyDown("joystick button 7"))
        {
            Debug.Log("button7");
        }
        if (Input.GetKeyDown("joystick button 8"))
        {
            Debug.Log("button8");
        }
        if (Input.GetKeyDown("joystick button 9"))
        {
            Debug.Log("button9");
        }
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        if ((hori != 0) || (vert != 0))
        {
            Debug.Log("stick:" + hori + "," + vert);
        }
    }
}
