using UnityEngine;
using System.Collections;

public class NetHelper
{

    public void updateToNet() {
        if (!GameManager.isAndroid)
        {
            return;
        }
        SqlNetDate date = new SqlNetDate();
        date.action =5;
        SQLNetManager.getIntance().addList(date);
    }

    public void changeInto(SQLDate data)
    {
        if (!GameManager.isAndroid)
        {
            return;
        }
        SqlNetDate date = new SqlNetDate();
        date.action = 1;
        date.date = data;
        SQLNetManager.getIntance().addList(date);
    }

    public void delectInfo(SQLDate data)
    {
        if (!GameManager.isAndroid)
        {
            return;
        }
        SqlNetDate date = new SqlNetDate();
        date.action = 2;
        date.date = data;
        SQLNetManager.getIntance().addList(date);
    }

    public void cleanLuihui()
    {
        if (!GameManager.isAndroid)
        {
            return;
        }
        SqlNetDate date = new SqlNetDate();
        date.action = 3;
        SQLNetManager.getIntance().addList(date);
    }
    public void cleanAll()
    {
        if (!GameManager.isAndroid)
        {
            return;
        }
        SqlNetDate date = new SqlNetDate();
        date.action = 4;
        SQLNetManager.getIntance().addList(date);
    }
}
