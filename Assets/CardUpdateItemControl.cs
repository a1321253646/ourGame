using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUpdateItemControl : MonoBehaviour
{
    CardUpUiControl[] list = null;
    YongjiuCardBean mBean;
    public void init(long id) {
        mBean = JsonUtils.getIntance().getYongjiuCardInfoById(id);
        list = GetComponentsInChildren<CardUpUiControl>();
        string[] ids = mBean.jihuo_card_group.Split(',');
        for (int i = 0; i < list.Length; i++) {
            if (i < ids.Length)
            {
                list[i].gameObject.transform.localScale = new Vector2(1, 1);
                Debug.Log("ids[" + i + "]=" + ids[i]);
                long idInt = long.Parse(ids[i]);
                list[i].init(idInt,false);
            }
            else {
                list[i].gameObject.transform.localScale = new Vector2(0, 0);
                
            }
        }
        list[list.Length - 1].init(mBean.id,true);
        list[list.Length - 1].gameObject.transform.localScale = new Vector2(1, 1);
    }

    public bool updateItem(long id) {
        if (id == -1) {
            foreach (CardUpUiControl cc in list)
            {
                cc.UpdateUi();
            }
            return true;
        }
        bool isUpdate = false;
        bool isHave = false;
        foreach(CardUpUiControl cc in list){
            Debug.Log("================================================CardUpdateItemControl cc.isHave =" + cc.isHave+ " cc.mId="+ cc.mId);
            if (cc.mId == id) {
                isHave = true;
                if (!cc.isHave) {
                    cc.UpdateUi();
                    isUpdate = true;
                }
                break;
            }
        }
        if (!isHave || (isHave && !isUpdate)) {
            return isHave;
        }
        if (isUpdate) {
            bool isLive = true;
            for (int i = 0; i < list.Length -1; i++) {
                Debug.Log("================================================CardUpdateItemControl list[i].mId =" + list[i].mId + " list[i].isHave=" + list[i].isHave);
                if (list[i].mId != -1 && !list[i].isHave) {
                    isLive = false;
                    break;
                }
            }
            if (isLive && !list[list.Length - 1].isHave) {
                BackpackManager.getIntance().addGoods(mBean.id, 1,true);
                list[list.Length - 1].UpdateUi();
            }
        }
        return isHave;
    }
}
