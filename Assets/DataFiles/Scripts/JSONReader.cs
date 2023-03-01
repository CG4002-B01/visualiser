using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// init_player = {
// 	"hp": 100,
// 	"action": None,
// 	"bullets": 6,
// 	"grenades": 2,
// 	"shield_time": 0,
// 	"shield_health": 0,
// 	"num_deaths": 0,
// 	"num_shield": 3
// }

// # db = {
// #         'player' = {
// #                 'p1': init_player json,
// #                 'p2': ..
// #         },
// #         'status' = {
// #                 init_status json
// #         }
// # }

public class JSONReader : MonoBehaviour
{
    // public TextAsset textJSON;
    public string textJSON;

    [System.Serializable]
    public class Players
    {
        public Player p1;
        public Player p2;
    }

    [System.Serializable]
    public class Player
    {
        public int hp;
        public string action;
        public int bullets;
        public int grenades;
        public int shield_time;
        public int shield_health;
        public int num_deaths;
        public int num_shield;
    }

    [System.Serializable]
    public class Statuses
    {
        public Status p1;
        public Status p2;
    }

    [System.Serializable]
    public class Status
    {
        public int num_kills;
    }

    [System.Serializable]
    public class Db 
    {
        public Players player;
        public Statuses status;
    }

    public Db fullGameState = new Db();

    public void setTextJSON(string text)
    {
        textJSON = text;
        fullGameState = JsonUtility.FromJson<Db>(textJSON);
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
