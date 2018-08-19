using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceBean
{
	public long id;
	public string name;
	public float zoom;
	public float attack_framce;
    public float attack_all_framce;
    public string blood_offset;
	public float blood_witch;
	public string hurt_offset;
	public string fight_offset;
	public float idel_y;
    public string target_border;
    public string action_list;
    public string action_frame;
    public Point BloodOffset,HurtOffset,FightOffset;
    private List<ActionFrameBean> mActionFrame;
    private List<int> mTargetBorder;

    public Point getBloodOffset(){
		if (BloodOffset == null) {
			BloodOffset = new Point (blood_offset);
		}
		return BloodOffset;
	}

	public Point getHurtOffset(){
		if (HurtOffset == null) {
			HurtOffset = new Point (hurt_offset);
		}
		return HurtOffset;
	}
	public Point getFightOffset(){
		if (FightOffset == null) {
			FightOffset = new Point (fight_offset);
		}
		return FightOffset;
	}
    public List<int> getTargetBorder() {
        return getListIntByString(target_border);
    }
    public List<ActionFrameBean> getActionFrameList() {
        if (mActionFrame != null) {
            return mActionFrame;
        }
        List<int> status = getListIntByString(action_list);
        List<int> frame = getListIntByString(action_frame);
        if (status == null || frame == null || status.Count != frame.Count) {
            return null;
        }
        mActionFrame = new List<ActionFrameBean>();
        for(int i = 0; i< status.Count; i++) {
            ActionFrameBean bean = new ActionFrameBean();
            bean.status = status[i];
            bean.frame = frame[i];
            mActionFrame.Add(bean);
        }
        return mActionFrame;
    }

    private List<int> getListIntByString(string str) {
        if (mTargetBorder != null) {
            return mTargetBorder;
        }
        if (str != null && str.Length > 0) {
            string[] array = str.Split(',');
            List<int> result = new List<int>();
            foreach (string str2 in array) {
                if (str2 == null || str2.Length < 1)
                {
                    continue;
                }
                result.Add(int.Parse(str2));
            }
            return result;

        }
        return null;
    }
}

