using UnityEngine;

// �����̴� ���� 
public class PlayerMoveState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Move State");
        this.playerController = stateMachine.PlayerController;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        
    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {
        //Debug.Log("(FixedUpdateState): Player Move!!");
        Move();
    }

    private void Move()
    {
        // �̵� ���� ����
        playerController.MovementDirection = playerController.MovementInput;

        // �̵� �ӵ� ����
        playerController.MovementDirection *= (CharacterManager.Instance.PlayerStat.MoveSpeed * CharacterManager.Instance.PlayerStat.SpeedModifier);

        // �߷��� velocity.y������ ����
        Vector2 dir = playerController.MovementDirection;
        dir.y = playerController.Rigid.velocity.y;
        playerController.MovementDirection = dir;
        //playerController.MovementDirection.y = playerController.Rigid.velocity.y;

        // �̵� ó��
        playerController.Rigid.velocity = playerController.MovementDirection;
    }
}
