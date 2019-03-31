using UnityEngine;
using System.Collections;

public class NetHelper
{

    public void updateToNet() {
        if (GameManager.isTestVersion)
        {
            return;
        }
#if UNITY_ANDROID || UNITY_IOS

        SqlNetDate date = new SqlNetDate();
        date.action = 5;
        SQLNetManager.getIntance().addList(date);
#endif

    }

    public void changeInto(SQLDate data)
    {
        if (GameManager.isTestVersion)
        {
            return;
        }
#if UNITY_ANDROID || UNITY_IOS
        SqlNetDate date = new SqlNetDate();
        date.action = 1;
        date.date = data;
        SQLNetManager.getIntance().addList(date);
#endif

    }

    public void delectInfo(SQLDate data)
    {
        if (GameManager.isTestVersion)
        {
            return;
        }
#if UNITY_ANDROID || UNITY_IOS
        SqlNetDate date = new SqlNetDate();
        date.action = 2;
        date.date = data;
        SQLNetManager.getIntance().addList(date);
#endif

    }

    public void cleanLuihui()
    {
        if (GameManager.isTestVersion)
        {
            return;
        }
#if UNITY_ANDROID || UNITY_IOS
        SqlNetDate date = new SqlNetDate();
        date.action = 3;
        SQLNetManager.getIntance().addList(date);
#endif

    }
    public void cleanAll()
    {
        if (GameManager.isTestVersion)
        {
            return;
        }
#if UNITY_ANDROID || UNITY_IOS
        SqlNetDate date = new SqlNetDate();
        date.action = 4;
        SQLNetManager.getIntance().addList(date);
#endif

    }
    public void cleanAllNet()
    {
        if (GameManager.isTestVersion)
        {
            return;
        }
#if UNITY_ANDROID || UNITY_IOS
        SqlNetDate date = new SqlNetDate();
        date.action = 6;
        SQLNetManager.getIntance().addList(date);
#endif

    }
    public void cleanAllLocal()
    {
        if (GameManager.isTestVersion)
        {
            return;
        }
#if UNITY_ANDROID || UNITY_IOS
        SqlNetDate date = new SqlNetDate();
        date.action = 7;
        SQLNetManager.getIntance().addList(date);
#endif

    }
}
