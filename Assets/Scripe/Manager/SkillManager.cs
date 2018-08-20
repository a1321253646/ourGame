using UnityEngine;
using System.Collections;

public class SkillManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //技能管理的初始化
    private void init() {

    }
    //每一个释放的技能都需要添加到列表中进行统一管理
    private void addSkill() {

    }
    //update被调用的时候对列表中的技能也进行时间的更新
    private void upDateSkill() {

    }
    //当技能失效后需要将对应的技能移除出列表
    private void destroySkill() {

    }
    //技能释放，需要携带参数为技能id和对应释放的 x y坐标
    public void dischargerSkill(long id,float dischargerX ,float dischargerY) {

    }
    //技能释放 携带参数为技能id，和释放对象
    public void dischargerSkill(long id ,Attacker target) {

    }

}
