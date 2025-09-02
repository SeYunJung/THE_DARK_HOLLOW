using UnityEngine;

// context = ���� ������ ���� ���� = StateMachine = StateManager(���� �Ŵ����� ����)
// ���¸ӽ��� context(��Ȳ, ����?)
public class StateManager : MonoBehaviour
{
    // ���� �ӽ��� �ϳ��� ���¸� ����. 
    private BaseState currentState;
    //public BaseState CurrentState { get; set; }

    // ��ü���� ���µ��� ����
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerMoveState moveState = new PlayerMoveState();
    public PlayerRunState runState = new PlayerRunState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerHitState hitState = new PlayerHitState();
    //public PlayerGrowingState growState = new PlayerGrowingState();
    //public PlayerWholeState wholeState = new PlayerWholeState();
    //public PlayerRottenState rottenState = new PlayerRottenState();
    //public PlayerChewedState chewedState = new PlayerChewedState();

    private PlayerController playerController;

    private void Start()
    {
        // �ʱ� ���� ����
        currentState = idleState;
        //currentState = moveState;
        //currentState = growState;

        // context�� �Ѱܼ� ���� ���� 
        currentState.EnterState(this);  

        playerController = GetComponent<PlayerController>();    
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
}
