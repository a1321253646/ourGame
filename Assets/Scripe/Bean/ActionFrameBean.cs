using UnityEngine;
using System.Collections;

public class ActionFrameBean 
{
    public static int ACTION_NONE = 0;
    public static int ACTION_STANDY = 1;
    public static int ACTION_ATTACK = 2;
    public static int ACTION_HURT = 3;
    public static int ACTION_DIE = 4;
    public static int ACTION_MOVE = 5;
    public static int ACTION_WIN = 6;
    public int status;
    public int frame;
}
