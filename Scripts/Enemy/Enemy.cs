using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{



    #region Declaration 
    [SerializeField] int _hp;
    [SerializeField]float _enemySpeed;
    [SerializeField]Player _player;
    [SerializeField]Transform smallBearsParent;
    Rigidbody rb;
    [SerializeField]float maxTpRange;
    [SerializeField]float waitUntilTp;
    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private AudioClip deathSoundClip;
    LevelGenerator levelGenerator;
    ItemSpawn itemSpawn;
    [SerializeField]GameObject BearPrefab;
    Animator animator;
    GameManager gameManager;
    Vector3 _newPos;
    bool IsTeleporting;
    bool IsSpawning;
    SoundManager audio;
    bool isDeath = false;


    [SerializeField]private AudioSource audioSource;

    #endregion


    #region MonoBehaviour
    private void Awake ()
    {
        levelGenerator = GameObject.Find("LevelGeneratorManager").GetComponent<LevelGenerator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        rb = gameObject.GetComponent<Rigidbody>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animator = gameObject.GetComponent<Animator>();
        audio = GameObject.Find("Sound manager").GetComponent<SoundManager>();
        itemSpawn = GameObject.Find("Boosters").GetComponent<ItemSpawn>();

    }
    #endregion
    private void Start ()
    {

        spawnBear();
        audioSource = GetComponent<AudioSource>();
        Player player = Player.instance;

    }
    private void Update ()
    {
        if (!isDeath)
        {
            Move();
        }




    }
    public void GetDamage (int damage)
    {
        _hp -= damage;

        if (_hp <= 0 && !isDeath)
        {
            isDeath = true;
            animator.SetTrigger("death");
            audio.FrogDeathSound();
            StartCoroutine("Death");
            gameManager.addDeathForEnemy();
            itemSpawn.getRandomBuster(transform.position);
        }
    }
    IEnumerator Death ()
    {

        yield return new WaitForSeconds(1);

        Destroy(gameObject);
        gameManager.addDeathForEnemy();




    }



    void Move ()
    {

        if (Vector3.Distance(transform.position, _player.transform.position) < 2 && !IsTeleporting)
        {
            StartCoroutine("teleport");

        }
        if (Vector3.Distance(transform.position, _player.transform.position) > 10 && !IsTeleporting)
        {
            StartCoroutine("teleport");

        }

    }

    void GenerateNewPos ()
    {
        _newPos = new Vector3(_player.transform.position.x + Random.Range(1, maxTpRange), transform.position.y, _player.transform.position.z - Random.Range(1, maxTpRange));

        //// Check if the new position is outside of the level boundaries
        //if (_newPos.x < levelGenerator.FloorSize[0].x || _newPos.x > levelGenerator.FloorSize[1].x ||
        //    _newPos.z < levelGenerator.FloorSize[0].z || _newPos.z > levelGenerator.FloorSize[1].z)
        //{
        //    // Regenerate new position until it's within boundaries
        //    GenerateNewPos();
        //}
    }


    IEnumerator teleport()
    {
        


        IsTeleporting = true;
        yield return new WaitForSeconds(waitUntilTp);
        GenerateNewPos();
        transform.position = _newPos;
        IsTeleporting=false;
        if (BearPrefab.GetComponentsInChildren<SmallBear>().Length<2)
        {
            spawnBear();
        }



    }
    private void spawnBear () 
    {
        
        
        GameObject Bear =  Instantiate(BearPrefab,smallBearsParent);
        GenerateNewPos();
        Bear.transform.position =_newPos;
        Bear.SetActive(true);
        
    }
}

