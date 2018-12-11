using UnityEngine;
using System.Collections;

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

    public BigNumber mOfflinereward;
    public BigNumber mReincarnation;

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

