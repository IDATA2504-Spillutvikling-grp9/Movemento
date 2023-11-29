using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerAttacks : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] Transform SideAttackTransform;         //position - the middle of the side attack area
    [SerializeField] Transform UpAttackTransform;           //position - the middle of the up attack area
    [SerializeField] Transform DownAttackTransform;         //position - the middle of the down attack area
    [Space(5)]

    [SerializeField] Vector2 SideAttackArea;                //Size - of the side attack area
    [SerializeField] Vector2 UpAttackArea;                  //Size - of the up attack area 
    [SerializeField] Vector2 DownAttackArea;                //Size - of the down attack are
    [Space(5)]

    [SerializeField] private float damage;                  //Damage the player does to an enemy
    [SerializeField] private GameObject slashEffect;        //Effects - Slash effect.
    [SerializeField] LayerMask attackableLayer;             //Layer - for the player to attack on
    float timeBetweenAttack, timeSinceAttack;
    bool attack = false;
    [Space(5)]

    [SerializeField] PlayerAttakingUI playerAttakingUI;

    [Header("Recoil Settings:")]
    [SerializeField] private int recoilXSteps = 5;          //how many FixedUpdates() the player recoils horizontally for
    [SerializeField] private int recoilYSteps = 5;          //how many FixedUpdates() the player recoils vertically for
    [SerializeField] private float recoilXSpeed = 100;      //the speed of horizontal recoil
    [SerializeField] private float recoilYSpeed = 100;      //the speed of vertical recoil
    private int stepsXRecoiled, stepsYRecoiled;             //the number of steps recoiled horizontally and vertically
    [Space(5)]

    private PlayerController pc;
    private PlayerMana pm;
    private Rigidbody2D rb;
    private float xAxis;
    private float yAxis;

    private float attackCooldown = 0.5f;
    private bool canAttack = true;

    private GameManager gameManager;


    void Start()
    {
        pc = GetComponent<PlayerController>();
        pm = GetComponent<PlayerMana>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
        playerAttakingUI = GetComponent<PlayerAttakingUI>();
    }



    /*
        Method is only used for Drawing the "hitboxes" onto the screen.
    */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(SideAttackTransform.position, SideAttackArea);
        Gizmos.DrawWireCube(UpAttackTransform.position, UpAttackArea);
        Gizmos.DrawWireCube(DownAttackTransform.position, DownAttackArea);
    }



    void Update()
    {
        if (pc.pState.dashing || pc.pState.healing) return;
        if (pc.pState.alive)
        if (canAttack) // Check if the player can attack based on cooldown
        {
            Attack();
        }
        {
            GetInputs();
        }
    }



    private void FixedUpdate()
    {
        if (pc.pState.dashing) return;
        Recoil();
    }



    /* 
        Sets the xAxis = to the input of the controller from -1 to 1 in horizontal direction.
    */
    void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");
        attack = Input.GetButtonDown("Attack");
    }



    void Attack()
    {
        if (gameManager.getIsPause())
        {
            return;
        }
        timeSinceAttack += Time.deltaTime;
        if (attack && timeSinceAttack >= timeBetweenAttack)
        {
            timeSinceAttack = 0;
            pc.anim.SetTrigger("Attacking");
            StartCoroutine(AttackCooldown());
            playerAttakingUI.UpdateSliderValue(1f, 0f);

            if (yAxis == 0 || yAxis < 0 && pc.Grounded())
            {
                int _recoilLeftOrRight = pc.pState.lookingRight ? 1 : -1;

                Hit(SideAttackTransform, SideAttackArea, ref pc.pState.recoilingX, Vector2.right, recoilXSpeed);
                Instantiate(slashEffect, SideAttackTransform);
            }
            else if (yAxis > 0)
            {
                Hit(UpAttackTransform, UpAttackArea, ref pc.pState.recoilingY, Vector2.up, recoilYSpeed);
                SlashEffectAtAngle(slashEffect, 60, UpAttackTransform);
            }
            else if (yAxis < 0 && !pc.Grounded())
            {
                Hit(DownAttackTransform, DownAttackArea, ref pc.pState.recoilingY, Vector2.down , recoilYSpeed);
                SlashEffectAtAngle(slashEffect, -90, DownAttackTransform);
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }   



    /*   
        Function used to check if an object is hittable
        The functions collects an array of objects that are within the position and area of the parameters passed in. Also checks if the attackable layer is added in.
        Loops through the array to check if it has hit something, if it has, do damage. 
    */
    private void Hit(Transform _attackTransform, Vector2 _attackArea, ref bool _recoilBool, Vector2 _recoilDir, float _recoilStrength)
    {
        Collider2D[] objectsToHit = Physics2D.OverlapBoxAll(_attackTransform.position, _attackArea, 0, attackableLayer);

        if (objectsToHit.Length > 0)
        {
            _recoilBool = true;
        }
        for (int i = 0; i < objectsToHit.Length; i++)
        {
            // Check if the object hit is a Caterpillar
            Caterpillar caterpillar = objectsToHit[i].GetComponent<Caterpillar>();
            if (caterpillar != null)
            {
                // Deal damage to the Caterpillar
                caterpillar.TakeDamage((int)damage);
            }
            else if (objectsToHit[i].GetComponent<Enemy>() != null)
            {
                // Existing enemy hit logic
                objectsToHit[i].GetComponent<Enemy>().EnemyHit(damage, _recoilDir, _recoilStrength);
                if (objectsToHit[i].CompareTag("Enemy"))
                {
                    pm.Mana += pm.manaGain;
                }
            }
        }
    }




    void SlashEffectAtAngle(GameObject _slashEffect, int _effectAngle, Transform _attackTransform)
    {
        _slashEffect = Instantiate(_slashEffect, _attackTransform);
        _slashEffect.transform.eulerAngles = new Vector3(0, 0, _effectAngle);
        _slashEffect.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }



    void Recoil()
    {
        if (pc.pState.recoilingX)
        {
            if (pc.pState.lookingRight)
            {
                rb.velocity = new Vector2(-recoilXSpeed, 0);
            }
            else
            {
                rb.velocity = new Vector2(recoilXSpeed, 0);
            }
        }

        if (pc.pState.recoilingY)
        {

            rb.gravityScale = 0;
            if (yAxis < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, recoilYSpeed);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -recoilYSpeed);
            }
            pc.airJumpCounter = 0;
        }
        else
        {
            rb.gravityScale = pc.gravity;
        }

        //stop recoil
        if (pc.pState.recoilingX && stepsXRecoiled < recoilXSteps)
        {
            stepsXRecoiled++;
        }
        else
        {
            StopRecoilX();
        }
        if (pc.pState.recoilingY && stepsYRecoiled < recoilYSteps)
        {
            stepsYRecoiled++;
        }
        else
        {
            StopRecoilY();
        }

        if (pc.Grounded())
        {
            StopRecoilY();
        }
    }



    void StopRecoilX()
    {
        stepsXRecoiled = 0;
        pc.pState.recoilingX = false;
    }



    void StopRecoilY()
    {
        stepsYRecoiled = 0;
        pc.pState.recoilingY = false;
    }
}