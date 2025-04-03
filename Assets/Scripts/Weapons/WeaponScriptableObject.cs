using UnityEngine;

[CreateAssetMenu(fileName = "Weapon_NAME", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponScriptableObject : ScriptableObject
{
    public GameObject BulletPrefab;
    public Sprite HoldingWeaponSprite;
    public AudioClip ShootSound;
    public string Name;
    public int MaxAmmo;
    public int Damage;
    public bool EndlessAmmo, SingleSpread;
    public float RandomSpreadFactor;
    public float ReloadTime;
    public float FireRate;
    public float ShopPrice;

}
