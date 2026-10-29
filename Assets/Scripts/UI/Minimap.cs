using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public static Minimap Instance { get; private set; }

    public Transform player;
    public RectTransform playerOnMap;
    public float scale  = 1;

    private List<Transform> enemies = new List<Transform>();
    private List<GameObject> enemiesOnMap = new List<GameObject>();
    public Sprite enemyOnMapSprite;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            playerOnMap.transform.rotation = player.rotation;
            for (int i = 0; i < enemies.Count; i++)
            {
                AlignPosition(i);
            }
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        GameObject temp = Instantiate(new GameObject("EnemyCircle"), playerOnMap);
        temp.AddComponent<RectTransform>();
        temp.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
        temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        Image enemySprite = temp.AddComponent<Image>();
        enemySprite.sprite = enemyOnMapSprite;
        enemySprite.color = Color.red;

        enemiesOnMap.Add(temp);
        enemies.Add(enemy.transform);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy.transform);
        enemiesOnMap.RemoveAt(enemy.gameObject);
    }

    void AlignPosition(int i)
    {
        var enemy = enemies[i];
        if(enemy != null &&  enemiesOnMap[i] != null)
        {
            Vector3 relativePosition;
            relativePosition = player.InverseTransformPoint(enemy.position);
            enemiesOnMap[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(relativePosition.x, relativePosition.y) * scale;
        }

    }
}
