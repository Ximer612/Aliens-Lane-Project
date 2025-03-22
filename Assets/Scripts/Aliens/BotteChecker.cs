using UnityEngine;

public class BotteChecker : MonoBehaviour
{
    [SerializeField] AlienMovement _alienMovement;
    [SerializeField] Weapon _myWeapon;
    [SerializeField] GameObject _currentObjective;
    [SerializeField] float _sqrMaxDistanceFromObjective = 9;

    private void Awake()
    {
        enabled = false;
    }

    private void Start()
    {
        _myWeapon.SetScriptableObject(_myWeapon.ScriptableObject);
        _myWeapon.InstantiateBullets(WeaponManager.EnemyBulletsLayerMask);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentObjective == null && other.CompareTag("Fence"))
        {
            _alienMovement.enabled = false;
            _currentObjective = other.transform.gameObject;
            enabled = true;
        }
    }

    private void Update()
    {
        if((_currentObjective.transform.position - transform.position).sqrMagnitude < _sqrMaxDistanceFromObjective && _currentObjective.activeSelf)
        {
            _myWeapon.Shoot(transform);
        }
        else
        {
            _alienMovement.enabled = true;
            _currentObjective = null;
            enabled = false;
        }
    }
}
