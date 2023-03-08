using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalStates
{
    static int playerNo;

    public static void SetPlayerNo(int no)
    {
        playerNo = no;
    }

    public static int GetPlayerNo()
    {
        return playerNo;
    }
}
