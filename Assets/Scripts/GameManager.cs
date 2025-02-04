using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public PlayerController player;
    public Boss boss;
    public GameObject bossM;
    public GameObject startzone;
    public int stage;
    public int enemyCnt;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3f;
    public bool isBattle;

    public AudioSource winSound;
    public AudioSource loseSound;

    public Transform[] enemyZones;
    public GameObject enemies;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    public GameObject clearPanel;

    public Text playerHPtxt;

    public Image wp1;
    public Image wp2;
    public Image wp3;

    public RectTransform bossHPGroup;
    public RectTransform bossHPbar;

    

    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        overPanel.SetActive(false);
        clearPanel.SetActive(false);

        player.gameObject.SetActive(true);
    }
    public void GameRestart() 
    {
        SceneManager.LoadScene("Main");
    }
    public void GameOver()
    {
        loseSound.Play();
        menuCam.SetActive(true);
        gameCam.SetActive(false);

        menuPanel.SetActive(false);
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        clearPanel.SetActive(false);

        player.gameObject.SetActive(false);
    }
    public void GameClear()
    {
        winSound.Play();
        menuCam.SetActive(true);
        gameCam.SetActive(false);

        menuPanel.SetActive(false);
        gamePanel.SetActive(false);
        overPanel.SetActive(false);
        clearPanel.SetActive(true);

        player.gameObject.SetActive(false);
    }    
    public void stageStart()
    {
        startzone.SetActive(false);

        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(true);
        }

        isBattle = true;
        StartCoroutine(InStage());
    }
    public void stageEnd()
    {
        isBattle=false;
        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(false);
        }
    }
    public void BossStage()
    {
        stageEnd();
        bossM.SetActive(true);
    }
    IEnumerator InStage()
    {
        while(enemyCnt < 10) 
        {
            int ranZone = Random.Range(0, 6);
            GameObject instantEnemy = Instantiate(enemies, enemyZones[ranZone].position, enemyZones[ranZone].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.manager = this;
            yield return new WaitForSeconds(5);
        }
    }
    void LateUpdate()
    {
        playerHPtxt.text = player.health + " / " + player.maxHealth;

        wp1.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0); // 무기를 가지고 있으면 투명하게, 없으면 불투명하게
        wp2.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        wp3.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);

        if(boss != null)
            bossHPbar.localScale = new Vector3(boss.curHealth / boss.maxHealth, 1, 1);  // 체력 비율에 따라 보스 hp바 설정
    }
}
