using UnityEngine;
using System.Collections;

public class HeroLevelUpAnimal : MonoBehaviour
{
    GameObject mAnimal;
    ResourceBean mResource;
    PlayControl mHero;
    private bool isInLevelUp = false;

    AnimalControlBase mAnimalControl;
    SpriteRenderer mSpriteRender;

    GameObject mObject;

    public HeroLevelUpAnimal(GameObject animal ,ResourceBean res,PlayControl hero) {
        mAnimal = animal;
        mResource = res;
        mHero = hero;

    }
    private void init() {
        mObject = GameObject.Instantiate(
            mAnimal, new Vector2(mHero.transform.position.x + mHero.resourceData.getHurtOffset().x - mResource.getHurtOffset().x,
            mHero.transform.position.y + mHero.resourceData.getHurtOffset().y+ (float)mHero.resourceData.idel_y- mResource.getHurtOffset().y),
            Quaternion.Euler(0.0f, 0f, 0.0f));
        mSpriteRender = mObject.GetComponent<SpriteRenderer>();
        mAnimalControl = new AnimalControlBase(mResource, mSpriteRender);
        mAnimalControl.setEndCallBack(ActionFrameBean.ACTION_NONE, new AnimalStatu.animalEnd(animalEnd));
        mAnimalControl.setIsLoop(ActionFrameBean.ACTION_NONE,false);
        mAnimalControl.start();
    }
    void animalEnd(int status)
    {
        isInLevelUp = false;      
        Destroy(mObject);
    }

    public void levelUp() {     
        if (!isInLevelUp)
        {
            init();        
            isInLevelUp = true;
        }
        else {
            mAnimalControl.setStatus(ActionFrameBean.ACTION_NONE);
        }
        
    }


    public void updateAnimal() {
        if (!isInLevelUp) {
            return;
        }
        mAnimalControl.update();
        mObject.transform.position = new Vector2(mHero.transform.position.x + mHero.resourceData.getHurtOffset().x - mResource.getHurtOffset().x,
            mHero.transform.position.y + (float)mHero.resourceData.idel_y - mResource.getHurtOffset().y); 

    }    

}
