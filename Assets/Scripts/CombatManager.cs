using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField]
    GameObject enemyPrefab;

    List<GameObject> enemies;

    private void Awake()
    {
        enemies.Add(Instantiate(enemyPrefab));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
