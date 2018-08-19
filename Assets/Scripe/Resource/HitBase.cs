using UnityEngine;
using System.Collections;

public class HitBase : MonoBehaviour
{
	public Animator _anim;
	public FightResource mFightResource;
    public SpriteRenderer mSpriteRender;
    AnimalControlBase mAnimalControl;
    // Use this for initialization

    public void startRun(FightResource fight){
		mFightResource = fight;
	}
    public void setId(long id)
    {
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        ResourceBean bean = JsonUtils.getIntance().getEnemyResourceData(id);
        mAnimalControl = new AnimalControlBase(bean, mSpriteRender);
        mAnimalControl.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(playEnd));
        mAnimalControl.start();
    }
    void playEnd(int status) {
        Destroy(gameObject);
    }
    void Update()
    {
        mAnimalControl.update();

    }
    void Start ()
	{	       
        mFightResource.hurt();
	}
}

