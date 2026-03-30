using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] LineRenderer line;

    // Start is called before the first frame update
    void OnEnable()
    {
        LevelActions.OnGameOver += GameOver;
    }

    private void OnDisable()
    {
        LevelActions.OnGameOver -= GameOver;
    }

    private void Start()
    {
        /*if (line.gameObject.activeInHierarchy && line.enabled)
            line.enabled = false;*/
    }

    void GameOver()
    {
        menu.SetActive(true);
        line.enabled = true;
    }
}
