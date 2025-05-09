using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Utilities
{
    public class GameOverSplasher: MonoBehaviour
    {
        [SerializeField] private GameObject winImage;
        [SerializeField] private GameObject loseImage;
        [SerializeField] private GameObject player;
        [SerializeField] private List<GameObject> enemies;

        private void Awake()
        {
            StartCoroutine(CheckWinLoseRoutine());
        }

        private IEnumerator CheckWinLoseRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                if (!player.activeInHierarchy)
                {
                    loseImage.SetActive(true);
                    yield break;
                }

                bool allEnemiesDefeated = true;
                foreach (var enemy in enemies)
                {
                    if (enemy.activeInHierarchy)
                    {
                        allEnemiesDefeated = false;
                        break;
                    }
                }
                if (allEnemiesDefeated)
                {
                    winImage.SetActive(true);
                    yield break;
                }
            }
        }
    }
}