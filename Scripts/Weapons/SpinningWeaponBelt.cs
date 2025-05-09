using System;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class SpinningWeaponBelt: MonoBehaviour
    {
        [SerializeField] private PoolManager poolManager;
        [SerializeField] private Transform weaponsParentTransform;
        [SerializeField] private int spawnWeaponCount = 3;

        public int id = 0;
        
        private readonly List<MeleeWeapon> _weapons = new List<MeleeWeapon>();
        
        private void Awake()
        {
            id = GetInstanceID();
        }
        
        private void Start()
        {
            for (var i = 0; i < spawnWeaponCount; i++)
            {
                AddWeapon();
            }
        }

        private void Update()
        {
            for (int i = 0; i < _weapons.Count; i++) // todo: optimization: this is kinda bad for performance
                                                     // maybe check every 5-10 frames or so
            {
                if (_weapons[i].Clashed)
                {
                    RemoveWeaponWithFlyOffAnimation(i);
                }
            }
        }

        public int GetWeaponCount()
        {
            return _weapons.Count;
        }

        public void AddWeapon()
        {
            var newRedSword = poolManager.GetRedSword();
            newRedSword.transform.parent = weaponsParentTransform;
            newRedSword.transform.localPosition = new Vector3(0, 0, 0);
            newRedSword.transform.localEulerAngles = new Vector3(0, 0, 0);
            var meleeWeaponComponent = newRedSword.GetComponent<MeleeWeapon>();
            meleeWeaponComponent.ParentWeaponBeltID = id;
            meleeWeaponComponent.selfCollider.enabled = true;
            _weapons.Add(meleeWeaponComponent);
            
            CalculateAnglesAndInformChildren();
        }
        
        private void RemoveWeaponWithFlyOffAnimation(int index)
        {
            if (index > _weapons.Count - 1)
            {
                return;
            }

            var weaponToRemove = _weapons[index];
            _weapons.RemoveAt(index);
            CalculateAnglesAndInformChildren();

            var flightDirection = Random.onUnitSphere;
            flightDirection.z = 0;
            var flightEndpoint = weaponToRemove.transform.position + (flightDirection.normalized * 20);
            
            weaponToRemove.transform.DORotate(new Vector3(0, 0, -720), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear);
            weaponToRemove.transform.DOMove(flightEndpoint, 1f) // todo: durations ease etc
                .SetEase(Ease.Linear)
                .OnComplete(
                    () =>
                    {
                        weaponToRemove.ParentWeaponBeltID = 0;
                        weaponToRemove.Clashed = false;
                        poolManager.ReleaseRedSword(weaponToRemove.GameObject());
                    });
        }
        
        private void CalculateAnglesAndInformChildren()
        {
            const float initialAngle = 0;
            var weaponCount = _weapons.Count;
            if (weaponCount == 0)
            {
                return;
            }
            var step = 360f / weaponCount;
            float stepsTaken = 0;
            for (var j = weaponCount - 1; j >= 0; j--)
            {
                var angle = initialAngle - (step * stepsTaken);
                _weapons[j].RotateSelfToAngle(angle);
                stepsTaken++;
            }
        }

        public void ReleaseAllWeaponsOnDeath()
        {
            foreach (var weapon in _weapons)
            {
                weapon.ParentWeaponBeltID = 0;
                weapon.Clashed = false;
                poolManager.ReleaseRedSword(weapon.GameObject());
            }
            
            _weapons.Clear();
        }
    }
}