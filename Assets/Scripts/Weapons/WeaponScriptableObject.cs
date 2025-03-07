using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_NAME", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    public GameObject BulletPrefab;
    public Sprite HoldingWeaponSprite;
    public AudioClip ShootSound;
    public string Name;
    public int MaxAmmo;
    public bool EndlessAmmo;
    public int Damage;
    public float RandomSpread;
    public float ReloadTime;
    public float FireRate;

}
