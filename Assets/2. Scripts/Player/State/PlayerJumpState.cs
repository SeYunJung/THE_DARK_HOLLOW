using UnityEngine;

public class PlayerJumpState : BaseState
{
    private PlayerController playerController;

    public override void EnterState(StateMachine stateMachine)
    {
        Debug.Log("Hello from the Jump State");
        this.playerController = stateMachine.PlayerController;

        // ���� ���� ���� 
        stateMachine.SetPreState(stateMachine);
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        // �������� �¾�����
        if (playerController.IsHit)
        {
            // Hit���·� ���� 
            stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Hit));
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
        Jump(stateMachine);
    }

    private void Jump(StateMachine stateMachine)
    {
        //stateMachine.SetStateBeforeJump(stateMachine);
        

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
                // ����
                //Debug.Log($"���� ���´� = {stateMachine.GetCurrentState()}");
                //stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));

                // ����1
                //// �ȱ� �� �϶�
                //if (stateMachine.GetCurrentState().Equals(Constants.State.MOVE))
                //{
                //    stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
                //}

                //// �޸��� �� �϶� 
                //else if (stateMachine.GetCurrentState().Equals(Constants.State.RUN))
                //{
                //    stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Run));
                //}

                // ����2
                // ���� ���°� �ȱ� ���¸�
                if (stateMachine.GetPreState().Equals(Constants.State.MOVE))
                {
                    //// ���� ���� ����
                    //stateMachine.SetPreState(stateMachine.GetPreState());

                    // ��� �ȱ� ���¸� ���� 
                    stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Move));
                }

                // ���� ���°� �޸��� ���¸� 
                else if (stateMachine.GetPreState().Equals(Constants.State.RUN))
                {
                    //// ���� ���� ����
                    //stateMachine.SetPreState(stateMachine.GetPreState());

                    // ��� �޸��� ���¸� ���� 
                    stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Run));
                }

            }

            // ���� ������ -> Idle ���·� ��ȯ
            if (stateMachine.PlayerController.IsGrounded())
            {
                //Debug.Log("������ ���� ������.");

                //Debug.Log($"���� ����({stateMachine.GetPreState()}�� �����Ѵ�.");
                //// �߰� 
                //stateMachine.SetPreState(stateMachine.GetPreState());
                //Debug.Log($"���� ����({stateMachine.GetPreState()}");

                stateMachine.SwitchState(stateMachine.Getstates(PlayerStateType.Idle));
            }
        }
    }
}
