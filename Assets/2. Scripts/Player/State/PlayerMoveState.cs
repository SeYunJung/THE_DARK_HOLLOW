using UnityEngine;

// �����̴� ���� 
public class PlayerMoveState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateManager stateManager)
    {
        Debug.Log("Hello from the Move State");
        this.playerController = stateManager.PlayerController;
    }

    public override void UpdateState(StateManager stateManager)
    {
        
    }

    public override void OnCollisionEnter(StateManager stateManager, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateManager stateManager)
    {
        Debug.Log("(FixedUpdateState): Player Move!!");
        Move();
    }

    private void Move()
    {
        // �̵� ���� ����
        playerController.MovementDirection = playerController.MovementInput;

        // �̵� �ӵ� ����
        playerController.MovementDirection *= (playerController.MoveSpeed * playerController.SpeedModifier);

        // �߷��� velocity.y������ ����
        Vector2 dir = playerController.MovementDirection;
        dir.y = playerController.Rigid.velocity.y;
        playerController.MovementDirection = dir;
        //playerController.MovementDirection.y = playerController.Rigid.velocity.y;

        // �̵� ó��
        playerController.Rigid.velocity = playerController.MovementDirection;
    }
}
