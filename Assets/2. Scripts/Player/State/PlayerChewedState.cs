using UnityEngine;

public class PlayerChewedState : BaseState
{
    private float destroyCountdown = 5.0f;

    public override void EnterState(StateManager player)
    {
        Debug.Log("Hello From The Chewed State");

        // �Դ� ���¿� �������� ��, �ִϸ��̼ǵ� �����ϰ� ���� �� �ִ�. 
        //Animator animator = player.GetComponent<Animator>();
        //animator.Play("Base Layer.eat", 0, 0);
    }

    public override void UpdateState(StateManager player)
    {
        if (destroyCountdown > 0)
        {
            destroyCountdown -= Time.deltaTime;
        }
        else
        {
            Object.Destroy(player.gameObject);
        }
    }

    public override void OnCollisionEnter(StateManager player, Collision2D collision)
    {

    }

    public override void FixedUpdateState(StateManager player)
    {

    }
}
