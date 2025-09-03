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

// context = ���� ������ ���� ���� = StateMachine = StateManager(���� �Ŵ����� ����)
// ���¸ӽ��� context(��Ȳ, ����?) Il || 
public class StateManager : MonoBehaviour
{
    // ���� �ӽ��� �ϳ��� ���¸� ����. 
    private BaseState currentState;
    //public BaseState CurrentState { get; set; }

    // ��ü���� ���µ��� ����
    private Dictionary<PlayerStateType, BaseState> states = new();
    //public PlayerIdleState idleState = new PlayerIdleState();
    //public PlayerMoveState moveState = new PlayerMoveState();
    //public PlayerRunState runState = new PlayerRunState();
    //public PlayerJumpState jumpState = new PlayerJumpState();
    //public PlayerHitState hitState = new PlayerHitState();

    //public PlayerGrowingState growState = new PlayerGrowingState();
    //public PlayerWholeState wholeState = new PlayerWholeState();
    //public PlayerRottenState rottenState = new PlayerRottenState();
    //public PlayerChewedState chewedState = new PlayerChewedState();

    //private PlayerController playerController;
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
        //currentState = idleState;
        currentState = states[PlayerStateType.Idle];
        //currentState = moveState;
        //currentState = growState;

        // context�� �Ѱܼ� ���� ���� 
        currentState.EnterState(this);  

    }

    private void Update()
    {
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
