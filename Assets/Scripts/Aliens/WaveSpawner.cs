using System;
using System.Collections.Generic;
using UnityEngine;

enum SpawnerState { Idle, Moving, SpawningAliens, LAST };

public class WaveSpawner : MonoBehaviour
{

    [SerializeField] Transform _aliensContainer, _whereToSpawnAliens;
    [SerializeField] WaveScriptableObject[] _waves;
    [SerializeField] int _waveIndex = -1;
    [SerializeField] int _mancheIndex = -1;
    [SerializeField] int _mancheInstanciateIndex = -1;
    float _spawnRange = 100;
    float _yOffsetFromGround = 10;

    [SerializeField] float _counter, _timeToSpawnOneEnemy;
    [SerializeField] SpawnerState _spawnState;
    Vector3 _oldPosition,_nextPosition;
    [SerializeField] AnimationCurve _movingCurve;
    [SerializeField] float _lerpMultiplier;

    private List<Vector2> _possibilePositions;
    private Vector2 _selectedPosition = new Vector2(0,0);

    private void Awake()
    {
        _possibilePositions = new List<Vector2>();
        _possibilePositions.Add(new Vector2(1, 0));
        _possibilePositions.Add(new Vector2(0, 1));
        _possibilePositions.Add(new Vector2(1, 1));
        _possibilePositions.Add(new Vector2(0.5f, 0));
        _possibilePositions.Add(new Vector2(0.5f, 1));
        _possibilePositions.Add(new Vector2(0, 0.5f));
        _possibilePositions.Add(new Vector2(1, 0.5f));

        enabled = false;
        StartNewWave();
    }
    private void StartNewWave()
    {
        _waveIndex++;

        if (_waveIndex >= _waves.Length)
        {
            enabled = false;
            return;
        }

        _mancheIndex = -1;
        enabled = true;
        StartNewManche();
    }

    private void StartNewManche()
    {
        _mancheIndex++;

        if(_mancheIndex >= _waves[_waveIndex].SpawnManche.Length)
        {
            StartNewWave();
            return;
        }

        _mancheInstanciateIndex = -1;
        SwitchState(SpawnerState.Idle, _waves[_waveIndex].SpawnManche[_mancheIndex].TimeFromLastManche);
    }

    private void SwitchState(SpawnerState NewState, float newCounter = 0)
    {
        _spawnState = NewState;
        _counter = newCounter;
    }

    private void Update()
    {
        switch (_spawnState)
        {
            case SpawnerState.Idle:

                _counter -= Time.deltaTime;

                if (_counter < 0)
                {
                    int positionIndex = UnityEngine.Random.Range(0, _possibilePositions.Count);
                    Vector2 randomPosition = _possibilePositions[positionIndex];
                    _possibilePositions.RemoveAt(positionIndex);
                    _possibilePositions.Add(_selectedPosition);
                    _selectedPosition = randomPosition;
                    Vector3 newUfoPosition = new Vector3(_selectedPosition.x * _spawnRange, UnityEngine.Random.Range(-_yOffsetFromGround, _yOffsetFromGround), _selectedPosition.y * _spawnRange);
                    _nextPosition = newUfoPosition;
                    _oldPosition = transform.localPosition;
                    SwitchState(SpawnerState.Moving);
                }
                break;

            case SpawnerState.SpawningAliens: //spawn some aliens and go again into moving until aliens are end
                _counter -= Time.deltaTime;

                if(_counter < 0)
                {
                    _mancheInstanciateIndex++;
                    if(_mancheInstanciateIndex >= _waves[_waveIndex].SpawnManche[_mancheIndex].Aliens.Length)
                    {
                        SwitchState(SpawnerState.Idle); //then check if ended to spawn and go into wait for end wave state or maybe never wait ??? like vampire survivors
                        StartNewManche();
                        return;
                    }

                    GameObject alien = Instantiate(_waves[_waveIndex].SpawnManche[_mancheIndex].Aliens[_mancheInstanciateIndex], _aliensContainer);
                    alien.name = "Alien" + _mancheInstanciateIndex.ToString();
                    alien.transform.SetPositionAndRotation(_whereToSpawnAliens.position,Quaternion.identity);
                    Vector3 lookAtVector = House.Instance.transform.position - alien.transform.position;
                    lookAtVector.y = 0;
                    alien.transform.rotation = Quaternion.LookRotation(lookAtVector);
                    SwitchState(SpawnerState.SpawningAliens, _timeToSpawnOneEnemy);
                }
                break;

            case SpawnerState.Moving:
                float evaluatedIndex = _movingCurve.Evaluate(_counter);
                print(evaluatedIndex);
                transform.localPosition = Vector3.LerpUnclamped(_oldPosition,_nextPosition, evaluatedIndex);
                _counter += Time.deltaTime * _lerpMultiplier;
                float _timer = _movingCurve.keys[_movingCurve.keys.Length-1].time;
                if (_counter > 1f)
                {
                    SwitchState(SpawnerState.SpawningAliens, _timeToSpawnOneEnemy);
                }
                break;
        }
    }
}
