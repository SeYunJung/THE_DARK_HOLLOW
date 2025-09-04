using UnityEngine;

public class PlayerRunState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Run State");
        this.playerController = stateMachine.PlayerController;
        ChangeSpeed();

        playerController.GetComponentInChildren<SpriteRenderer>().flipX = playerController.MovementInput.x < 0 ? true : false;

        // Run Animation
        playerController.AnimationController.Move(Vector2.zero);
        playerController.AnimationController.Run(playerController.MovementInput);
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // �������� �¾�����
        if (playerController.IsHit)
        {
            // Hit���·� ���� 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Hit));
        }

        // �޸��� Ű�� ����

    }

    public override void OnCollisionEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void OnTriggerEnter(StateMachine stateMachine, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateMachine stateMachine)
    {
        //Debug.Log("(FixedUpdateState): Player Run!!");
        Run();
    }

    private void ChangeSpeed()
    {
        CharacterManager.Instance.PlayerStat.SpeedModifier = CharacterManager.Instance.PlayerStat.SpeedModifierInput;
        //this.playerController.SpeedModifier = playerController.SpeedModifierInput;
    }

    private void Run()
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
