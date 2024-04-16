using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{



    #region Declaration 
    [SerializeField] int _hp;
    [SerializeField]float _enemySpeed;
    [SerializeField]Player _player;
    Rigidbody rb;
    [SerializeField]float maxTpRange;
    [SerializeField]float waitUntilTp;
    LevelGenerator levelGenerator;
    [SerializeField]GameObject BearPrefab;
    [SerializeField]float waitUntilSpawn;
    Vector3 _newPos;
    bool IsTeleporting;
    bool IsSpawning;


    #endregion


    #region MonoBehaviour
    private void Awake ()
    {
        levelGenerator = GameObject.Find("LevelGeneratorManager").GetComponent<LevelGenerator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        rb = gameObject.GetComponent<Rigidbody>();
    }
    #endregion
    private void Start ()
    {

    }
    private void Update ()
    {
        Move();
        if (!IsSpawning ) 
        {
            StartCoroutine("spawnBear");
        }
        
    }
    public void GetDamage (int damage)
    {
        _hp -= damage;
        if (_hp < 0)
        {
            Destroy(gameObject);
        }
    }


    void Move ()
    {

        if (Vector3.Distance(transform.position, _player.transform.position) < 2&& !IsTeleporting)
        {
            StartCoroutine("teleport");



        }

    }

    void GenerateNewPos ()
    {
        _newPos = new Vector3(_player.transform.position.x - Random.Range(2, maxTpRange), transform.position.y, _player.transform.position.z - Random.Range(2, maxTpRange));
        if (_newPos.x < levelGenerator.FloorSize[0].x && _newPos.x > levelGenerator.FloorSize[1].x || _newPos.z < levelGenerator.FloorSize[0].z&&_newPos.x > levelGenerator.FloorSize[1].z) 
        {
            GenerateNewPos();
        }


    }


    IEnumerator teleport()
    {
        


        IsTeleporting = true;
        yield return new WaitForSeconds(waitUntilTp);
        GenerateNewPos();
        transform.position = _newPos;
        IsTeleporting=false;
    
    
    
    }
    IEnumerator spawnBear () 
    {
        IsSpawning =true;
        yield return new WaitForSeconds(waitUntilSpawn);
        GameObject Bear =  Instantiate(BearPrefab);
        GenerateNewPos();
        Bear.transform.position =_newPos;
        Bear.SetActive(true);
        IsSpawning=false;
    }
}

