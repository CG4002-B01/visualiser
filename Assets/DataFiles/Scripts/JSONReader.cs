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
        public int shield_activated;
        public int last_shield_active_time;
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

    // Getters and Setters

    // Player 1 Stuff
    public int getOwnDeaths()
    {
        return fullGameState.player.p1.num_deaths;
    }

    public int getOwnKills()
    {
        return fullGameState.status.p1.num_kills;
    }

    public int getOwnHealth()
    {
        return fullGameState.player.p1.hp;
    }

    public int getOwnShieldHealth()
    {
        return fullGameState.player.p1.shield_health;
    }

    public int getOwnShieldNum()
    {
        return fullGameState.player.p1.num_shield;
    }

    public int getOwnShieldTime()
    {
        return fullGameState.player.p1.shield_time;
    }

    public int getOwnBulletNum()
    {
        return fullGameState.player.p1.bullets;
    }

    public int getOwnGrenade()
    {
        return fullGameState.player.p1.grenades;
    }

    public string getOwnAction()
    {
        return fullGameState.player.p1.action;
    }

    // Player 2 Stuff
    public int getEnemyDeaths()
    {
        return fullGameState.player.p2.num_deaths;
    }

    public int getEnemyKills()
    {
        return fullGameState.status.p2.num_kills;
    }

    public int getEnemyHealth()
    {
        return fullGameState.player.p2.hp;
    }

    public int getEnemyShieldHealth()
    {
        return fullGameState.player.p2.shield_health;
    }

    public int getEnemyShieldNum()
    {
        return fullGameState.player.p2.num_shield;
    }

    public int getEnemyShieldTime()
    {
        return fullGameState.player.p2.shield_time;
    }

    public int getEnemyBulletNum()
    {
        return fullGameState.player.p2.bullets;
    }

    public int getEnemyGrenadeNum()
    {
        return fullGameState.player.p2.grenades;
    }

    public string getEnemyAction()
    {
        return fullGameState.player.p2.action;
    }
}
