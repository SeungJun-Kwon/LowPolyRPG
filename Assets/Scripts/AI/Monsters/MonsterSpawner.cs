using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] GameObject _monsterPrefab;
    [SerializeField] NormalMonster _monster;
    [SerializeField] float _spawnRate = 0f;
    [SerializeField] float _monsterSpawnDistance = 1.5f;
    [SerializeField] int _monsterSpawnQuantity = 2;

    [SerializeField] List<GameObject> _spawnMonsters = new List<GameObject>();
    [SerializeField] List<GameObject> _restMonsters = new List<GameObject>();
    SphereCollider _sphereColl;
    Vector3 _randomVector, _distanceFromMonster;

    int _currentMonsterQuantity = 0;
    float _xPos, _zPos;
    float _cognizance = 0f;
    float _radiusOfAction = 0f;

    private void Awake()
    {
        TryGetComponent<SphereCollider>(out _sphereColl);

        for (int i = 0; i < _monsterSpawnQuantity; i++)
        {
            AddMonster();
        }

        StartCoroutine(SpawnMonster());
    }

    private void Start()
    {
        _cognizance = _monster._cognizance;
        _radiusOfAction = _monster._radiusOfAction;
        _sphereColl.radius = _cognizance;
    }

    private void Update()
    {
        foreach(var monster in _spawnMonsters)
        {
            _distanceFromMonster = transform.position - monster.transform.position;
            if(_distanceFromMonster.magnitude > _radiusOfAction)
            {
                monster.TryGetComponent<MonsterAI>(out var monsterAI);
                monsterAI.SetTarget(null);
            }
        }
    }

    IEnumerator SpawnMonster()
    {
        while(true)
        {
            if(_currentMonsterQuantity < _monsterSpawnQuantity)
            {
                var monster = GetMonster();
                monster.transform.position = RandomLocation();
                monster.transform.rotation = RandomRotation();
            }
            yield return new WaitForSeconds(_spawnRate);
        }
    }

    private Vector3 RandomLocation()
    {
        _xPos = Random.Range(-_monsterSpawnDistance, _monsterSpawnDistance);
        _zPos = Random.Range(-_monsterSpawnDistance, _monsterSpawnDistance);
        _randomVector = new Vector3(_xPos, 0, _zPos);
        return transform.position + _randomVector;
    }

    private Quaternion RandomRotation()
    {
        return Quaternion.Euler(0, Random.Range(-180, 180), 0); ;
    }

    public GameObject AddMonster()
    {
        var newMonster = Instantiate(_monsterPrefab, RandomLocation(), RandomRotation());
        newMonster.transform.SetParent(transform);
        newMonster.SetActive(true);
        _spawnMonsters.Add(newMonster);
        _currentMonsterQuantity++;
        return newMonster;
    }

    public GameObject GetMonster()
    {
        if(_restMonsters.Count > 0)
        {
            var monster = _restMonsters[0];
            monster.SetActive(true);
            _spawnMonsters.Add(monster);
            _restMonsters.Remove(monster);
            _currentMonsterQuantity++;
            return monster;
        }
        else
        {
            var monster = AddMonster();
            _monsterSpawnQuantity++;
            return monster;
        }
    }

    public void RemoveMonster(GameObject monster)
    {
        _restMonsters.Add(monster);
        _spawnMonsters.Remove(monster);
        _currentMonsterQuantity--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _monster._isPreempt)
        {
            foreach(var monster in _spawnMonsters)
            {
                monster.TryGetComponent<MonsterAI>(out var monsterAI);
                monsterAI.SetTarget(other.transform);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && _monster._isPreempt)
            OnTriggerEnter(other);
    }
}