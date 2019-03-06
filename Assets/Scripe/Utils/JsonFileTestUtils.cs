using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JsonFileTestUtils
{
    public static string test() {
        
        string str = testFile(1);
        str += testFile(1001);
        str += testFile(2001);
        str += testFile(3001);
        str += testFile(4001);
        return str;
    }
    private static string testFile(long leve) {
        string back0 = "关卡"+leve + "对应的表格测试\n";
        string back = "";
        JsonUtils.getIntance().reReadAboutLevelFile(leve);
        JsonUtils.getIntance().getLevelData();
        List<Level> levelData = JsonUtils.getIntance().levelData;

        Dictionary<long, List<LevelEnemyWellen>> mWellentList = JsonUtils.getIntance().mWellentList;
        Dictionary<long, Enemy> mEnemys = JsonUtils.getIntance().mEnemys;
        foreach (Level l in levelData) {
            long bossId = l.boss_DI;
            if (mEnemys.ContainsKey(bossId))
            {
                back = back + testEnemy(mEnemys[bossId]);
            }
            else
            {
                back = back + l.id + "关卡：" + bossId + " 怪物不存在\n";
            }

            string[] wellenStr = l.wellen.Split('#');
            foreach (string str in wellenStr)
            {
                long wellenID = int.Parse(str);
                if (mWellentList.ContainsKey(wellenID))
                {
                    List<LevelEnemyWellen> levelwellen = mWellentList[wellenID];
                    foreach (LevelEnemyWellen lw in levelwellen) {
                        if (mEnemys.ContainsKey(lw.id)) {
                            back = back + testEnemy(mEnemys[lw.id]);
                        }
                        else
                        {
                            back = wellenID + "波次" + lw.id + " 怪物不存在\n";
                        }
                    }
                }
                else {
                    back = back + l.id + "关卡：" +   wellenID + " 波次不存在\n";
                }
            }


        }
        if (back.Equals(""))
        {
            return back;
        }
        else {
            return back0+back;
        }
        
    }
    private static string testEnemy(Enemy em) {
        string back = "";
        List<long> fellList = em.getFellList();

        List<DropDevice> mDropDevoce = JsonUtils.getIntance().mDropDevoce;
        List<DropDeviceDetail> mDropDeviceDetailData = JsonUtils.getIntance().mDropDeviceDetailData;
        if (fellList != null && fellList.Count > 0) {
            foreach (long fell in fellList) {
                if (fell == 0) {
                    continue;
                }
                DropDevice dd = getDropDevice(fell, mDropDevoce);
                if (dd == null)
                {
                    back = back + "怪物" + em.id + "死亡掉落器" + fell + "不存在\n";
                }
                else {
                    long group = dd.groupId;
                    long up = dd.upGroupID;
                    DropDeviceDetail detail = null;
                    if (group != 0) {
                        detail = getDropDeviceDetail(group, mDropDeviceDetailData);
                        if (detail == null)
                        {
                            back = back + "死亡掉落器" + fell + "基础组" + group + "不存在\n";
                        }
                        else
                        {
                            back = back + testDetail(detail);
                        }
                    }

                    if (up != 0) {
                        detail = getDropDeviceDetail(up, mDropDeviceDetailData);
                        if (detail == null)
                        {
                            back = back + "死亡掉落器" + fell + "进阶组" + group + "不存在\n";
                        }
                        else
                        {
                            back = back + testDetail(detail);
                        }
                    }
                }
            }
        }
        return back;
    }

    public static string testDetail(DropDeviceDetail detail) {
        string back = "";
        for (int i = 0; i < detail.mItemList.Count; i++) {
            long goodId = detail.mItemList[i].itemId;
            if ((goodId > InventoryHalper.TABID_2_START_ID && goodId < InventoryHalper.TABID_3_START_ID)||
                goodId > InventoryHalper.TABID_5_START_ID)
            {
                AccouterJsonBean good = JsonUtils.getIntance().getAccouterInfoById(goodId);
                if (good == null)
                {
                    back = back+ detail.id + "掉落器详情" + goodId + "装备" + "不存在\n";
                }
                else if (good.tabid != 2)
                {
                    back = back+goodId + "装备" + "tabid不正确\n";
                }
            }
            else if (goodId > InventoryHalper.TABID_3_START_ID && goodId < InventoryHalper.TABID_4_START_ID)
            {
                CardJsonBean good = JsonUtils.getIntance().getCardInfoById(goodId);
                if (good == null)
                {
                    back = detail.id + "掉落器详情" + goodId + "卡牌" + "不存在\n";
                }
                else if (good.tabid != 3)
                {
                    back = goodId + "卡牌" + "tabid不正确\n";
                }
            }
        }
        return back;
    }

    private static DropDeviceDetail getDropDeviceDetail(long id, List<DropDeviceDetail> mDropDeviceDetailData)
    {

        foreach (DropDeviceDetail dd in mDropDeviceDetailData)
        {
            if (dd.id == id)
            {
                return dd;
            }
        }
        return null;
    }


    private static DropDevice getDropDevice(long id ,List<DropDevice> mDropDevoce) {
        
        foreach(DropDevice dd in mDropDevoce) {
            if (dd.id == id) {
                return dd;
            }
        }
        return null;
    }
}
