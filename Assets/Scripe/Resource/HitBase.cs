using UnityEngine;
using System.Collections;

public class HitBase : MonoBehaviour
{
	public Animator _anim;
	public FightResource mFightResource;
	private string mAnimRunName = "run";
	// Use this for initialization

	public void startRun(FightResource fight){
		mFightResource = fight;
	}

	void Start ()
	{	
		_anim = gameObject.GetComponent<Animator> ();
		RuntimeAnimatorController rc = _anim.runtimeAnimatorController;
		AnimationClip[] cls = rc.animationClips;
		foreach(AnimationClip cl in cls){
			if (cl.name.Equals ("Dead")) {
				//isAddEvent = true;
				AnimationEvent event1 = new AnimationEvent ();
				event1.functionName = "runEnd";
				event1.time = cl.length-0.1f;
				cl.AddEvent (event1);
			}/* else if (cl.name.Equals ("Attack")) {
				//isAddEvent = true;
				AnimationEvent event1 = new AnimationEvent ();
				event1.functionName = "standyEvent";
				event1.time = cl.length-0.1f;
				cl.AddEvent (event1);
			} */
		
		}
	}
	public void runEnd(){
		mFightResource.hurt ();
		Destroy (this);
	}
}

