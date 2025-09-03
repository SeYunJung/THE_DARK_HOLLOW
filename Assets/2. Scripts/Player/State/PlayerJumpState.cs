using UnityEngine;

public class PlayerJumpState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Jump State");
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
        Jump(stateMachine);
    }

    private void Jump(StateMachine stateMachine)
    {
        if (playerController.CanJump)
        {
            playerController.Rigid.AddForce(Vector2.up * CharacterManager.instance.PlayerStat.JumpPower, ForceMode2D.Impulse);
            playerController.CanJump = false;
        }
        else
        {
            // ���� ��

            // arrow left/right�� �Է��� ��� -> �̵�ó�� 
            if (stateMachine.PlayerController.IsMoving)
            {
                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
            }

            // ���� ������ -> Idle ���·� ��ȯ
            if (stateMachine.PlayerController.IsGrounded())
            {
                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
            }
        }
    }
}
