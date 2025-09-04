using UnityEngine;

// �����̴� ���� 
public class PlayerMoveState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Move State");
        this.playerController = stateMachine.PlayerController;

        // ���� ���� ���� 
        stateMachine.SetPreState(stateMachine);

        playerController.GetComponentInChildren<SpriteRenderer>().flipX = playerController.MovementInput.x < 0 ? true : false;

        // Move Animation
        playerController.AnimationController.Run(Vector2.zero);
        playerController.AnimationController.Move(playerController.MovementInput);

        ChangeSpeed();
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // �������� �¾�����
        if (playerController.IsHit)
        {
            // Hit���·� ���� 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Hit));
        }

        // �̵� �� ����Ű(x)�� ������ 
        if (Input.GetKeyDown(KeyCode.X))
        {
            // ���� ���·� ��ȯ 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Attack));
        }
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

    private void ChangeSpeed()
    {
        CharacterManager.instance.PlayerStat.SpeedModifier = 1.0f;
        //this.playerController.SpeedModifier = 1.0f;
    }
}
