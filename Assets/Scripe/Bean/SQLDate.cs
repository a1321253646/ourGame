using UnityEngine;
using System.Collections;

public class SQLDate
{
    public static long GOOD_TYPE_BACKPACK = 1;
    public static long GOOD_TYPE_ZHUANGBEI = 2;
    public static long GOOD_TYPE_CARD = 3;
    public static long GOOD_TYPE_USER_CARD = 4;
    public static long GOOD_TYPE_PET = 5;
    public static long GOOD_TYPE_USER_PET = 6;
    public static long GOOD_TYPE_NOGOOD = 7;

    public static long CLEAR = 1;
    public static long CLEAR_NO = 2;

    public static long DEFAULT_GOOD_ID = -1;

    public long type = -1;
    public long id = -1;
    public string extan = "-1";
    public long goodId = SQLDate.DEFAULT_GOOD_ID;
    public long goodType = SQLDate.GOOD_TYPE_NOGOOD;
    public long isClean = 1;// 1为清除，2为不清除
    public void getClean() {
        isClean = SQLDate.CLEAR;
        if (type == SQLHelper.TYPE_GAME)
        {
            if (id == SQLHelper.GAME_ID_AUTO ||
               id == SQLHelper.GAME_ID_LUNHUI ||
               id == SQLHelper.GAME_ID_TIME ||
               id == SQLHelper.GAME_ID_POINT_LUNHUI ||
               id == SQLHelper.GAME_ID_NO_LUNHUI ||
               id == SQLHelper.GAME_ID_FRIST_START ||
               id == SQLHelper.GAME_ID_IS_UPDATE ||
               id == SQLHelper.GAME_ID_IS_VOICE||
               id == SQLHelper.GAME_ID_PLAYER_NAME ||
               id == SQLHelper.GAME_ID_VERSION_CODE ||
               id == SQLHelper.GAME_ID_MAX_TIME ||
               id == SQLHelper.GAME_ID_PLAYER_MAX_LEVEL ||
               id == SQLHelper.GAME_ID_HAD_LUNHUI)
            {
                isClean = SQLDate.CLEAR_NO;
            }
        }
        else if (type == SQLHelper.TYPE_LUNHUI || type == SQLHelper.TYPE_GUIDE)
        {
            isClean = SQLDate.CLEAR_NO;
        }
        else if (type == SQLHelper.TYPE_GOOD && (goodType == SQLDate.GOOD_TYPE_PET || goodType == SQLDate.GOOD_TYPE_USER_PET)) {
            isClean = SQLDate.CLEAR_NO;
        }
    }

    public string toString() {
        return "type = " + type + " id =" + id + " extan=" + extan;
    }
}
