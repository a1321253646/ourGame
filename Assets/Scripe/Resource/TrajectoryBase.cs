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
	Attacker mTarget;
    public SpriteRenderer mSpriteRender;
    AnimalControlBase mAnimalControl;
    private bool isWork = false;


    // Use this for initialization
    public void setId(long id)
    {
        mSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        ResourceBean bean = JsonUtils.getIntance().getEnemyResourceData(id);
        mAnimalControl = new AnimalControlBase(bean, mSpriteRender);
        mAnimalControl.start();
    }

    public void startRun(Attacker attacker,BackgroundManager back,FightResource fight)
    {
		mBackManager = back;
		mAttacker = attacker;
      //  mTarget = target;
        mFightResource = fight;
        speed = JsonUtils.getIntance().getConfigValueForId(100001);
		isRUn = true;
        isWork = true;

    }

	// Update is called once per frame
	void Update ()
	{

  //      Debug.Log("traject update isdun"+isRUn);
		if (!isRUn)
			return;
        if (GameManager.getIntance().isEnd || mAttacker.mAttackerTargets.Count==0)
        {
            isRUn = false;
            isWork = false;
            Destroy(gameObject);
            return;
        }
        float sp = speed;
        if (mBackManager.isRun)
        {
            sp = mBackManager.moveSpeed + speed;
        }
        float disx = transform.position.x + mFightResource.mTrajectResource.getHurtOffset().x - mAttacker.mAttackerTargets[0].transform.position.x + mAttacker.mAttackerTargets[0].resourceData.getHurtOffset().x;
        float disy = transform.position.y + mFightResource.mTrajectResource.getHurtOffset().y - mAttacker.mAttackerTargets[0].transform.position.y - mAttacker.mAttackerTargets[0].resourceData.getHurtOffset().y;
        if (disx < 0.1f && disx > -0.1f) {
            mFightResource.trajectoryActionIsEnd();
            Destroy(gameObject);
            mAnimalControl.update();
            return;
        }

        float distance = sp * Time.deltaTime;
        float bili = distance / Mathf.Sqrt(disx * disx + disy * disy);
        disx = bili * disx;
        disy = bili * disy;
        transform.Translate(Vector2.left * disx);
        transform.Translate(Vector2.down * disy);

		//isReach ();
        mAnimalControl.update();

    }
	private void isReach(){
        if (!isWork) {
            return;
        
        }
        //  Debug.Log("traject update isReach");
        if (mAttacker.mAttackerTargets.Count > 0 && transform.position.x + mFightResource.mTrajectResource.getHurtOffset().x <=
            mAttacker.mAttackerTargets[0].transform.position.x+ mAttacker.mAttackerTargets[0].resourceData.getHurtOffset().x+0.2) {
			mFightResource.trajectoryActionIsEnd ();
			Destroy (gameObject);
		} 
	}
}

