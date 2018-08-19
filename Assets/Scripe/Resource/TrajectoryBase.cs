using UnityEngine;
using System.Collections;

public class TrajectoryBase : MonoBehaviour
{
	public FightManager mFightManager;
	public BackgroundManager mBackManager;
	FightResource mFightResource;
	public float speed = 1f;
	public bool isRUn = false;
	Attacker mAttacker;
    public SpriteRenderer mSpriteRender;
    AnimalControlBase mAnimalControl;

    // Use this for initialization
    public void setId(long id)
    {
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        ResourceBean bean = JsonUtils.getIntance().getEnemyResourceData(id);
        mAnimalControl = new AnimalControlBase(bean, mSpriteRender);
        mAnimalControl.start();
    }

    public void startRun(Attacker attacker,BackgroundManager back,FightResource fight){
		mBackManager = back;
		mAttacker = attacker;
		mFightResource = fight;
        speed = JsonUtils.getIntance().getConfigValueForId(100001);
		isRUn = true;
	}

	// Update is called once per frame
	void Update ()
	{
  //      Debug.Log("traject update isdun"+isRUn);
		if (!isRUn)
			return;
		if (mBackManager.isRun) {
            gameObject.transform.Translate (Vector2.left * (mBackManager.moveSpeed + speed) * Time.deltaTime);
		} else {
            gameObject.transform.Translate (Vector2.left *  speed * Time.deltaTime);
		}
		isReach ();
        mAnimalControl.update();

    }
	private void isReach(){
        //  Debug.Log("traject update isReach");
        if (mAttacker.mAttackerTargets.Count > 0 && transform.position.x + mFightResource.mTrajectResource.getHurtOffset().x <=
            mAttacker.mAttackerTargets[0].transform.position.x+ mAttacker.mAttackerTargets[0].resourceData.getHurtOffset().x) {
			mFightResource.trajectoryActionIsEnd ();
			Destroy (gameObject);
		} 
	}
}

