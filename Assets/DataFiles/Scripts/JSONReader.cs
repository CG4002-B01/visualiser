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
        public int id;
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
    public int getOwnDeaths(int connectedPlayer)
    {
        int noDeaths = 0;
        switch(connectedPlayer)
        {
            case 1:
                noDeaths = fullGameState.player.p1.num_deaths;
                break;
            case 2:
                noDeaths = fullGameState.player.p2.num_deaths;
                break;
        }
        return noDeaths;
    }

    public int getOwnKills(int connectedPlayer)
    {
        int noKills = 0;
        switch(connectedPlayer)
        {
            case 1:
                noKills = fullGameState.status.p1.num_kills;
                break;
            case 2:
                noKills = fullGameState.status.p2.num_kills;
                break;
        }
        return noKills;
    }

    public int getOwnHealth(int connectedPlayer)
    {
        int health = 0;
        switch(connectedPlayer)
        {
            case 1:
                health = fullGameState.player.p1.hp;
                break;
            case 2:
                health = fullGameState.player.p2.hp;
                break;
        }
        return health;
    }

    public int getOwnShieldHealth(int connectedPlayer)
    {
        int shieldHealth = 0;
        switch(connectedPlayer)
        {
            case 1:
                shieldHealth = fullGameState.player.p1.shield_health;
                break;
            case 2:
                shieldHealth = fullGameState.player.p2.shield_health;
                break;
        }
        return shieldHealth;
    }

    public int getOwnShieldNum(int connectedPlayer)
    {
        int shieldNum = 0;
        switch(connectedPlayer)
        {
            case 1:
                shieldNum = fullGameState.player.p1.num_shield;
                break;
            case 2:
                shieldNum = fullGameState.player.p2.num_shield;
                break;
        }
        return shieldNum;
    }

    public int getOwnShieldTime(int connectedPlayer)
    {
        int shieldTime = 0;
        switch(connectedPlayer)
        {
            case 1:
                shieldTime = fullGameState.player.p1.shield_time;
                break;
            case 2:
                shieldTime = fullGameState.player.p2.shield_time;
                break;
        }
        return shieldTime;
    }

    public int getOwnBulletNum(int connectedPlayer)
    {
        int bulletNo = 0;
        switch(connectedPlayer)
        {
            case 1:
                bulletNo = fullGameState.player.p1.bullets;
                break;
            case 2:
                bulletNo = fullGameState.player.p2.bullets;
                break;
        }
        return bulletNo;
    }

    public int getOwnGrenade(int connectedPlayer)
    {
        int grenadeNo = 0;
        switch(connectedPlayer)
        {
            case 1:
                grenadeNo = fullGameState.player.p1.grenades;
                break;
            case 2:
                grenadeNo = fullGameState.player.p2.grenades;
                break;
        }
        return grenadeNo;
    }

    public string getOwnAction(int connectedPlayer)
    {
        string action = "";
        switch(connectedPlayer)
        {
            case 1:
                action = fullGameState.player.p1.action;
                break;
            case 2:
                action = fullGameState.player.p2.action;
                break;
        }
        return action;
    }

    public int getOwnId(int connectedPlayer)
    {
        int id = -1;
        switch(connectedPlayer)
        {
            case 1:
                id = fullGameState.status.p1.id;
                break;
            case 2:
                id = fullGameState.status.p2.id;
                break;
        }
        return id;
    }

    public int isOwnShieldActivated(int connectedPlayer)
    {
        int status = -1;
        switch(connectedPlayer)
        {
            case 1:
                status = fullGameState.status.p1.shield_activated;
                break;
            case 2:
                status = fullGameState.status.p2.shield_activated;
                break;
        }
        return status;
    }

    // Player 2 Stuff
    public int getEnemyDeaths(int enemyPlayer)
    {
        int noDeaths = 0;
        switch(enemyPlayer)
        {
            case 1:
                noDeaths = fullGameState.player.p1.num_deaths;
                break;
            case 2:
                noDeaths = fullGameState.player.p2.num_deaths;
                break;
        }
        return noDeaths;
    }

    public int getEnemyKills(int enemyPlayer)
    {
        int noKills = 0;
        switch(enemyPlayer)
        {
            case 1:
                noKills = fullGameState.status.p1.num_kills;
                break;
            case 2:
                noKills = fullGameState.status.p2.num_kills;
                break;
        }
        return noKills;
    }

    public int getEnemyHealth(int enemyPlayer)
    {
        int health = 0;
        switch(enemyPlayer)
        {
            case 1:
                health = fullGameState.player.p1.hp;
                break;
            case 2:
                health = fullGameState.player.p2.hp;
                break;
        }
        return health;
    }

    public int getEnemyShieldHealth(int enemyPlayer)
    {
        int shieldHealth = 0;
        switch(enemyPlayer)
        {
            case 1:
                shieldHealth = fullGameState.player.p1.shield_health;
                break;
            case 2:
                shieldHealth = fullGameState.player.p2.shield_health;
                break;
        }
        return shieldHealth;
    }

    public int getEnemyShieldNum(int enemyPlayer)
    {
        int shieldNum = 0;
        switch(enemyPlayer)
        {
            case 1:
                shieldNum = fullGameState.player.p1.num_shield;
                break;
            case 2:
                shieldNum = fullGameState.player.p2.num_shield;
                break;
        }
        return shieldNum;
    }

    public int getEnemyShieldTime(int enemyPlayer)
    {
        int shieldTime = 0;
        switch(enemyPlayer)
        {
            case 1:
                shieldTime = fullGameState.player.p1.shield_time;
                break;
            case 2:
                shieldTime = fullGameState.player.p2.shield_time;
                break;
        }
        return shieldTime;
    }

    public int getEnemyBulletNum(int enemyPlayer)
    {
        int bulletNo = 0;
        switch(enemyPlayer)
        {
            case 1:
                bulletNo = fullGameState.player.p1.bullets;
                break;
            case 2:
                bulletNo = fullGameState.player.p2.bullets;
                break;
        }
        return bulletNo;
    }

    public int getEnemyGrenadeNum(int enemyPlayer)
    {
        int grenadeNo = 0;
        switch(enemyPlayer)
        {
            case 1:
                grenadeNo = fullGameState.player.p1.grenades;
                break;
            case 2:
                grenadeNo = fullGameState.player.p2.grenades;
                break;
        }
        return grenadeNo;
    }

    public string getEnemyAction(int enemyPlayer)
    {
        string action = "";
        switch(enemyPlayer)
        {
            case 1:
                action = fullGameState.player.p1.action;
                break;
            case 2:
                action = fullGameState.player.p2.action;
                break;
        }
        return action;
    }

    public int getEnemyId(int enemyPlayer)
    {
        int id = -1;
        switch(enemyPlayer)
        {
            case 1:
                id = fullGameState.status.p1.id;
                break;
            case 2:
                id = fullGameState.status.p2.id;
                break;
        }
        return id;
    }

    public int isEnemyShieldActivated(int enemyPlayer)
    {
        int status = -1;
        switch(enemyPlayer)
        {
            case 1:
                status = fullGameState.status.p1.shield_activated;
                break;
            case 2:
                status = fullGameState.status.p2.shield_activated;
                break;
        }
        return status;
    }
}
