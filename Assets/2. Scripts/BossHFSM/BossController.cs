using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;              // Ÿ��(������ �ڵ����� Player �±� �˻�)
    public Rigidbody2D rb;
    public Animator animator;
    public Animator preAnimator;
    public BossStat stat;

    // FSM
    public BossStateMachine fsm { get; private set; }
    // States
    IdleState idle;
    ChaseState chase;
    ChooseState choose;
    AttackDashState atkDash;
    AttackRangedState atkRanged;
    RecoverState recover;
    DeadState dead;

    // ��ٿ�
    float readyDash, readyRanged;

    // ���� Perception ĳ��(�����) ����
    bool canSee;
    float distCache = Mathf.Infinity;
    float pAcc;
    bool canHurt = true;

    // ĳ�� ������
    public float Dist => distCache;
    bool InAggro => Dist <= stat.detectRange;

    [SerializeField] GameObject preAttack;

    void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!animator) animator = GetComponentInChildren<Animator>();
        if (!stat) stat = GetComponent<BossStat>();

        // Player �±� �ڵ� �Ҵ�(���� �����Ǿ� ������ ����)
        if (!player)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go) player = go.transform;
        }

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
        if (CharacterManager.instance) CharacterManager.instance.Boss = GetComponent<Boss>();
        fsm.Change(idle);
    }

    void Update()
    {
  // ���� Perception �ֱ� ������Ʈ����
        pAcc += Time.deltaTime;
        if (pAcc >= 1f / Mathf.Max(0.1f, stat.perceptionHz))
        {
            pAcc = 0f;
            UpdatePerception2D();
        }

        // �νĹ��� ���̸� � ���µ� ��� Idle
        if (!InAggro && fsm.Current != null && fsm.Current.Name != "Idle")
        {
            StopMove();
            fsm.Change(idle, reason: "Force");
            return;
        }

        // ���� ƽ
        fsm.Tick(Time.deltaTime);
    }

    void FixedUpdate() => fsm.FixedTick(Time.fixedDeltaTime);

    // ����������������������������������������������������������
    // Perception(�����) : �Ÿ������ε� ���(���ϸ� FOV/LoS �߰� ����)
    // ����������������������������������������������������������
    void UpdatePerception2D()
    {
        if (!player) { canSee = false; distCache = Mathf.Infinity; return; }
        distCache = Vector2.Distance(transform.position, player.position);
        canSee = distCache <= stat.detectRange; // ���� ����: ���� ���� "�� �� ����"
    }

    // ����������������������������������������������������������
    // ���� ��ƿ (���¿��� ȣ��)
    // ����������������������������������������������������������
    public bool CanSeePlayer() => canSee;
    public bool CDReadyDash() => Time.time >= readyDash;
    public bool CDReadyRanged() => Time.time >= readyRanged;
    public void StartCD_Dash() => readyDash = Time.time + stat.dashCooldown;
    public void StartCD_Ranged() => readyRanged = Time.time + stat.rangedCooldown;

    public void MoveTowards(Vector2 pos, float speed)
    {
        var dir = (pos - (Vector2)transform.position).normalized;
        rb.velocity = dir * speed;
    }
    public void StopMove() => rb.velocity = Vector2.zero;

    public void FaceToPlayer()
    {
        if (!player) return;
        var dir = (player.position - transform.position);
    }

    public void Play(string trigger)
    {
        if (!animator || string.IsNullOrEmpty(trigger)) return;
        animator.ResetTrigger(trigger);
        animator.SetTrigger(trigger);
    }
    public void OnPreAttackEffect()
    {
        preAttack.SetActive(true);
        StartCoroutine(OnPreAttackRoutine());
    }
    IEnumerator OnPreAttackRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        preAttack.SetActive(false);
    }

    // ����ü ����(������ ��ġ�ϵ��� stat.bulletSpeed ���)
    public void FireProjectile()
    {
        if (!stat.bulletPrefab) return;
        Vector3 spawnPos = stat.firePoint ? stat.firePoint.position : transform.position;
        var go = Instantiate(stat.bulletPrefab, spawnPos, Quaternion.identity);

        if (player && go.TryGetComponent<Rigidbody2D>(out var rb2))
        {
            Vector2 dir = (player.position - spawnPos).normalized;
            rb2.velocity = dir * stat.bulletSpeed; // �� �ӵ� ����
        }
        // ������ �߻�ü ��ũ��Ʈ���� bulletLifetime�� ������ Destroy�ϵ��� ����
    }

    public void ToDie()
    {
        Destroy(this.gameObject, 3f);
    }

    public void TakeDamage(float damage)
    {
        if (canHurt == true)
        {
            canHurt = false;
            stat.hp01 -= damage;
            Play("TakeDamage");
            StartCoroutine(OnTakeDamageRoutine());
        }
    }
    IEnumerator OnTakeDamageRoutine()
    {
        yield return new WaitForSeconds(0.8f);
        canHurt = true;
    }

    public void ApplyDamage()
    {
        
    }


    // ���� ������(���� ���̿�)
    public IdleState SIdle => idle;
    public ChaseState SChase => chase;
    public ChooseState SChoose => choose;
    public AttackDashState SAtkDash => atkDash;
    public AttackRangedState SAtkRng => atkRanged;
    public RecoverState SRecover => recover;
    public DeadState SDead => dead;

    // ����������������������������������������������������������
    // Gizmos (�Ÿ�/����/���/����ü ����Ÿ�)
    // ����������������������������������������������������������
    void OnDrawGizmosSelected()
    {
        // ���۷����� �����Ϳ��� ������� �� �����Ƿ� ���� üũ
        var s = stat ? stat : GetComponent<BossStat>();
        if (!s) return;

        // �߽���
        Vector3 c = transform.position;

        // 1) ���� ��
        // detectRange : ���, nearRange : �ʷ�, farRange : �Ķ�
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(c, s.detectRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(c, s.nearRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(c, s.farRange);

        // 2) ���� ���� �̵��Ÿ�(����Ÿ ����)
        float dashDist = s.dashSpeed * s.dashActive;
        Vector3 dashDir = (transform.localScale.x >= 0) ? Vector3.right : Vector3.left;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(c, c + dashDir * dashDist);
        // ȭ��ǥ �Ӹ�
        Vector3 tip = c + dashDir * dashDist;
        Gizmos.DrawLine(tip, tip + Quaternion.Euler(0, 0, 150) * dashDir * 0.4f);
        Gizmos.DrawLine(tip, tip + Quaternion.Euler(0, 0, -150) * dashDir * 0.4f);

        // 3) ����ü ���� �̵��Ÿ�(�þ� ����)
        if (s.firePoint)
        {
            float projDist = s.bulletSpeed * s.bulletLifetime;
            Vector3 fp = s.firePoint.position;

            // �⺻�� �ٶ󺸴� ����, �÷��̾ ������ �÷��̾� �������� ǥ��
            Vector3 pDir = (player ? (player.position - fp).normalized
                                   : ((transform.localScale.x >= 0) ? Vector3.right : Vector3.left));
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(fp, fp + pDir * projDist);
            // ȭ��ǥ �Ӹ�
            Vector3 tip2 = fp + pDir * projDist;
            Gizmos.DrawLine(tip2, tip2 + Quaternion.Euler(0, 0, 150) * pDir * 0.35f);
            Gizmos.DrawLine(tip2, tip2 + Quaternion.Euler(0, 0, -150) * pDir * 0.35f);
        }
    }
}
