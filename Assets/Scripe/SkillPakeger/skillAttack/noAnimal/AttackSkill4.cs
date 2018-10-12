using UnityEngine;
using System.Collections;

public class AttackSkill4 : AttackSkillNoAnimal
{

    public override bool add(float count)
    {
        return false;
    }

    public override float beAction(HurtStatus status)
    {
        return -1;
    }

    public new void inAction()
    {
        throw new System.NotImplementedException();
    }

    public override void initEnd()
    {
        GameObject.Find("Manager").GetComponent<LevelManager>().addNengliangDian(5);
    }

    public override void upDateEnd()
    {
    }
}
