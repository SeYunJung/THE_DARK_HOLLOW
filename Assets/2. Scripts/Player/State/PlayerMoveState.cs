using UnityEngine;

// �����̴� ���� 
public class PlayerMoveState : BaseState
{
    private StateManager player;

    public override void EnterState(StateManager player)
    {
        Debug.Log("Hello from the Move State");
        this.player = player;
    }

    public override void UpdateState(StateManager player)
    {
        
    }

    public override void OnCollisionEnter(StateManager player, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateManager player)
    {
        Debug.Log("(FixedUpdateState): Player Move!!");
        Move();
        Jump();
    }

    private void Move()
    {
        //// �̵� ���� ����
        //movementDirection = player.movementInput;

        //// �̵� �ӵ� ����
        //movementDirection *= (moveSpeed * speedModifier);

        //// �߷��� velocity.y������ ����
        //movementDirection.y = rigid.velocity.y;

        //// �̵� ó��
        //rigid.velocity = movementDirection;
    }

    private void Jump()
    {
        //if (canJump)
        //{
        //    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        //    canJump = false;
        //}
    }
}
