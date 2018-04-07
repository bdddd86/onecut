using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public Transform spawnPoint;

    public GameObject objChacater;
    public GameObject blackScreen;

    Character character; 

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        InitGame();
        
    }

    void InitGame()
    {
        this.character = Instantiate(objChacater, spawnPoint.position, Quaternion.identity).GetComponent<Character>(); 
    }

    public void RestartGame()
    {
        character.transform.position = spawnPoint.position;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
