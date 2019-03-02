using System.Collections.Generic;

public class SkillIndexUtil 
{
    private static SkillIndexUtil mIntance = new SkillIndexUtil();
    public static SkillIndexUtil getIntance() {
        return mIntance;
    }


    private Dictionary<long, long> mCardIndexList = new Dictionary<long, long>();
    private Dictionary<long, long> mPetIndexList = new Dictionary<long, long>();
    private Dictionary<long, long> mEquitIndexList = new Dictionary<long, long>();
    private Dictionary<long, long> mVocationIndexList = new Dictionary<long, long>();
    private Dictionary<long, long> mSamIndexList = new Dictionary<long, long>();
    private long mSkillIndex = 0;
    public long getSkillIndex()
    {
        mSkillIndex++;
        return mSkillIndex;
    }

    public long getSkillIndexByCardId(bool isBoss,long id) {
        if (isBoss) {
            id = id * 10;
        }
        if (!mCardIndexList.ContainsKey(id))
        {
            mCardIndexList.Add(id, getSkillIndex());
            
        }
        return mCardIndexList[id];
    }
    public void deleteSkillIndexByCardId(bool isBoss, long id)
    {
        if (isBoss)
        {
            id = id * 10;
        }
        if (mCardIndexList.ContainsKey(id))
        {
            mCardIndexList.Remove(id);
        }
    }


    public long getPetIndexByPetId(bool isBoss, long id) {
        if (isBoss)
        {
            id = id * 10;
        }
        if (!mPetIndexList.ContainsKey(id))
        {
            mPetIndexList.Add(id, getSkillIndex());

        }
        return mPetIndexList[id];
    }

    public long getEquitIndexByGoodId(bool isBoss, long id)
    {
        if (isBoss)
        {
            id = id * 10;
        }
        if (!mEquitIndexList.ContainsKey(id))
        {
            mEquitIndexList.Add(id, getSkillIndex());

        }
        return mEquitIndexList[id];
    }
    public void deleteEquitIndexByGoodId(bool isBoss, long id)
    {
        if (isBoss)
        {
            id = id * 10;
        }
        if (mEquitIndexList.ContainsKey(id))
        {
            mEquitIndexList.Remove(id);
        }
    }

    public long getVocationIndexByVocationId(bool isBoss, long id)
    {
        if (isBoss)
        {
            id = id * 10;
        }
        if (!mVocationIndexList.ContainsKey(id))
        {
            mVocationIndexList.Add(id, getSkillIndex());

        }
        return mVocationIndexList[id];
    }
    public long getSamIndexBySamId(bool isBoss, long id)
    {
        if (isBoss)
        {
            id = id * 10;
        }
        if (!mSamIndexList.ContainsKey(id))
        {
            mSamIndexList.Add(id, getSkillIndex());

        }
        return mSamIndexList[id];
    }
}