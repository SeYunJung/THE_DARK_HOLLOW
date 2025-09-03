using System.Collections.Generic;
using UnityEngine;

public enum PlayerStateType
{
    Idle,
    Move,
    Run,
    Jump,
    Hit
}

// context = ���� ������ ���� ���� = StateMachine 
public class StateMachine : MonoBehaviour
{
    // ���� �ӽ��� �ϳ��� ���¸� ����. 
    private BaseState currentState;

    // ��ü���� ���µ��� ������ Dictionary 
    private Dictionary<PlayerStateType, BaseState> states = new();

    public PlayerController PlayerController { get; set; }

    private void Awake()
    {
        states.Add(PlayerStateType.Idle, new PlayerIdleState());
        states.Add(PlayerStateType.Move, new PlayerMoveState());
        states.Add(PlayerStateType.Run, new PlayerRunState());
        states.Add(PlayerStateType.Jump, new PlayerJumpState());
        states.Add(PlayerStateType.Hit, new PlayerHitState());
    }

    private void Start()
    {
        PlayerController = GetComponent<PlayerController>();    

        // �ʱ� ���� ����
        currentState = states[PlayerStateType.Idle];

        // context�� �Ѱܼ� ���� ���� 
        currentState.EnterState(this);  

    }

    private void Update()
    {
        //Debug.Log($"���� ���� : {currentState.ToString()}");
        switch (currentState)
        {
            case PlayerIdleState:
            case PlayerHitState:
                // �� �����Ӹ��� ����� ����
                currentState.UpdateState(this);
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerMoveState:
            case PlayerJumpState:
            case PlayerRunState:
            case PlayerHitState:
                currentState.FixedUpdateState(this);
                break;
        }
    }

    // ���� ��ȯ => ���� �Ŵ����� ��ü���� ���µ��� ������ �����ϱ� 
    public void SwitchState(BaseState state)
    {
        currentState = state; 
        state.EnterState(this); // ���� �Ŵ����� �˷��༭ ���� ���� 
    }

    // ���� �Ŵ����� ���� ������Ʈ���� �浹�� �߻��ϸ� �浹 ���·� �����Ѵ� 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentState.OnCollisionEnter(this, collision);
    }

    public BaseState Getstates(PlayerStateType type)
    {
        return states[type];
    }
}
