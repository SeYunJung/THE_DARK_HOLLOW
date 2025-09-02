using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;              // Ÿ��
    public Rigidbody2D rb;
    public Animator animator;
    public BossStat stat;

    // FSM
    public BossStateMachine fsm { get; private set; }
    // States
    IdleState idle; ChaseState chase; ChooseState choose;
    AttackDashState atkDash; AttackRangedState atkRanged;
    RecoverState recover; DeadState dead;

    // ��ٿ�
    float readyDash, readyRanged;

    // ĳ��
   public float Dist => player ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;
    bool InAggro => Dist <= stat.detectRange;
    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        if (!stat) stat = GetComponent<BossStat>();

        fsm = new BossStateMachine();

        // ���� �ν��Ͻ�
        idle = new IdleState(this, fsm);
        chase = new ChaseState(this, fsm);
        choose = new ChooseState(this, fsm);
        atkDash = new AttackDashState(this, fsm);
        atkRanged = new AttackRangedState(this, fsm);
        recover = new RecoverState(this, fsm);
        dead = new DeadState(this, fsm);
    }

    void Start()
    {
        // CharacterManager�� ���� ��� (�� �ڵ� ȣȯ)
        if (CharacterManager.instance) CharacterManager.instance.Boss = GetComponent<Boss>();
        fsm.Change(idle);
    }

    void Update()
    {
        // 1) �ƿ� ���� ��׷� �۷ι� ����: ������ ����� � ���µ� ��� Idle�� ����
        if (!InAggro && fsm.Current != null && fsm.Current.Name != "Idle")
        {
            StopMove();
            fsm.Change(idle, reason: "Force"); // ��� �����ϰ� ��� ����
            return;
        }

        // 2) ���� ƽ
        fsm.Tick(Time.deltaTime);
    }

    void FixedUpdate() => fsm.FixedTick(Time.fixedDeltaTime);

    // ����������������������������������������������������������
    // ���� ��ƿ (���¿��� ȣ��)
    // ����������������������������������������������������������
    public bool CanSeePlayer() => player && InAggro; 
    public bool CDReadyDash() => Time.time >= readyDash;
    public bool CDReadyRanged() => Time.time >= readyRanged;
    public void StartCD_Dash() => readyDash = Time.time + stat.dashCooldown;
    public void StartCD_Ranged() => readyRanged = Time.time + stat.rangedCooldown;

    public void MoveTowards(Vector2 pos, float speed)
    {
        var dir = (pos - (Vector2)transform.position).normalized;
        rb.velocity = dir * speed;
        if (dir.x != 0) transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }
    public void StopMove() => rb.velocity = Vector2.zero;

    public void FaceToPlayer()
    {
        if (!player) return;
        var dir = (player.position - transform.position);
        if (Mathf.Abs(dir.x) > 0.01f) transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }

    public void Play(string trigger)
    {
        if (!animator || string.IsNullOrEmpty(trigger)) return;
        animator.ResetTrigger(trigger);
        animator.SetTrigger(trigger);
    }

    // ����ü ����
    public void FireProjectile()
    {
        if (!stat.bulletPrefab) return;
        Vector3 spawnPos = stat.firePoint ? stat.firePoint.position : transform.position;
        var go = Instantiate(stat.bulletPrefab, spawnPos, Quaternion.identity);
        // TODO: �߻�ü �̵�/������ ��ũ��Ʈ�� ����/�ӵ� ���� (����)
        if (player && go.TryGetComponent<Rigidbody2D>(out var rb2))
        {
            Vector2 dir = (player.position - spawnPos).normalized;
            rb2.velocity = dir * 10f;
        }
    }

    public void ToDie()
    {
        Destroy(this.gameObject, 3f);
    }


    // ���� ������(���� ���̿�)
    public IdleState SIdle => idle;
    public ChaseState SChase => chase;
    public ChooseState SChoose => choose;
    public AttackDashState SAtkDash => atkDash;
    public AttackRangedState SAtkRng => atkRanged;
    public RecoverState SRecover => recover;
    public DeadState SDead => dead;
}
