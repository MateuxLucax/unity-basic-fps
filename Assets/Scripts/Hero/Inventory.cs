using System.Collections;
using Sun_Temple.Scripts.Doors;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Weapons.Grenade;

namespace Hero
{
    public class Inventory : MonoBehaviour
    {
        public GameObject glock;
        public int score = 0;
        public Door bossDoor;
        public Door firstDoor;
        public Text scoreText;
        public GameObject gameOver;
        public Text gameOverText;
        public Text gameOverScoreText;
        public Grenades grenades;

        private void Start()
        {
            glock.SetActive(false);
        }

        public void KilledBoss()
        {
            UpdateScore(30);
            if (bossDoor.IsLocked)
            {
                bossDoor.IsLocked = false;
            }
        }

        public void KilledEnemy()
        {
            UpdateScore(10);
            if (score >= 100 && firstDoor.IsLocked)
            {
                firstDoor.IsLocked = false;
            }
        }

        private void UpdateScore(int addedScore)
        {
            score += addedScore;
            scoreText.text = "Score: " + score;
        }

        public IEnumerator GameOver(bool died)
        {
            Time.timeScale = 0;
            glock.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            gameOver.SetActive(true);
            gameOverText.text = died ? "You lost!" : "You won!";
            gameOverScoreText.text = "Score: " + score;

            yield return WaitForRealSeconds(5);
            SceneManager.LoadScene(0);
        }

        private static IEnumerator WaitForRealSeconds(float delay)
        {
            var start = Time.realtimeSinceStartup;

            while (Time.realtimeSinceStartup < start + delay)
            {
                yield return null;
            }
        }
    }
}