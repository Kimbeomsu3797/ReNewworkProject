using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    Animator anim;
    Transform transform;
    private Vector3 moveDir;

    PlayerInput playerInput;
    InputActionMap mainActionMap;
    InputAction moveAction;
    InputAction attackAction;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        playerInput = GetComponent<PlayerInput>();
        //ActionMap ����
        mainActionMap = playerInput.actions.FindActionMap("PlayerActions");
        //Move, Attack �׼� ����
        moveAction = mainActionMap.FindAction("Move");
        attackAction = mainActionMap.FindAction("Attack");

        //Move �׼��� performed �̺�Ʈ ����
        moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            //�ִϸ��̼� ����
            anim.SetFloat("Movement", dir.magnitude);
        };

        //Move �׼��� canceled �̺�Ʈ ����//Ű���� ��ư�� ������ ���� ��
        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;

            anim.SetFloat("Movement", 0.0f);
        };

        //Attack �׼��� performed �̺�Ʈ ����
        attackAction.performed += ctx =>
        {
            Debug.Log("Attack by c# event");
            anim.SetTrigger("Attack");
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }

    }
    #region SEND_MESSEGE
    private void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
        anim.SetFloat("Movement", dir.magnitude);
        Debug.Log($"Move = ({dir.x}.{dir.y})");
    }
    void OnAttack()
    {
        Debug.Log("Attack");
        anim.SetTrigger("Attack");
    }
    #endregion
    #region UNITY_EVENTS
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        moveDir = new Vector3(dir.x, 0, dir.y);
        anim.SetFloat("Movement", dir.magnitude);
        
    }
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ctx.phase = {ctx.phase}");
        if (ctx.performed)
        {
            Debug.Log("Attack");
            anim.SetTrigger("Attack");
        }
    }

    #endregion
}