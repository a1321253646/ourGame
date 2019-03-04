using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level
{
	public long id;
	public string name;
	public string wellen;
	public long boss_DI;
	public float boss_gas;
	public string map;
	public string boss_bg;
	public string abc;
	public string reincarnation;
	public string offlinereward;
	public float levelspeedup;
	public string lunhui;
	public string hunjing;
    public string card;


    public BigNumber mOfflinereward;
    public BigNumber mReincarnation;
    public BigNumber mAdHunjing;
    public BigNumber mAdLunhui;

    public List<long> mBossCardList;

    public long getCardListId() {
        if (SQLHelper.getIntance().mCardLevel == -1 || SQLHelper.getIntance().mCardLevel != BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel))
        {
            List<long> list = getBossCardList();
            long id = Random.Range(0, list.Count);
            id = list[(int)id];
            SQLHelper.getIntance().updateLevelCardId(BaseDateHelper.decodeLong(GameManager.getIntance().mCurrentLevel), id);
            return id;
        }
        else {
            return SQLHelper.getIntance().mCardListId;
        }
    }

    public List<long> getBossCardList() {
        if (mBossCardList == null) {
            mBossCardList = new List<long>();
            string[] strs =  card.Split(',');
            foreach (string str in strs) {
                if (string.IsNullOrEmpty(str)) {
                    continue;
                }
                mBossCardList.Add(long.Parse(str));
            }
        }
        return mBossCardList;
    }

    public BigNumber getAdHunjing()
    {
        if (mAdHunjing == null)
        {
            mAdHunjing = BigNumber.getBigNumForString(hunjing);
        }
        return mAdHunjing;
    }
    public BigNumber getAdLunhui()
    {
        if (mAdLunhui == null)
        {
            mAdLunhui = BigNumber.getBigNumForString(lunhui);
        }
        return mAdLunhui;
    }

    public BigNumber getReincarnation()
    {
        if (mReincarnation == null)
        {
            mReincarnation = BigNumber.getBigNumForString(reincarnation);
        }
        return mReincarnation;
    }

    public BigNumber getOfflinereward()
    {
        if (mOfflinereward == null)
        {
            mOfflinereward = BigNumber.getBigNumForString(offlinereward);
        }
        return mOfflinereward;
    }
}

