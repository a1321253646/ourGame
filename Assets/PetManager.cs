using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetManager : MonoBehaviour {

    // Update is called once per frame
    public GameObject mPetGj;
    public PlayControl mHero;

    public class PetLocalDate {
        public float x;
        public float y;
        public long id;
    }
    List<PetLocalDate> mLocalList = new List<PetLocalDate>();
    Dictionary<long, PetItemControl> mControlList = new Dictionary<long, PetItemControl>();
    int mPetCount = 0;
    public bool isFull() {
        return mPetCount >= JsonUtils.getIntance().getConfigValueForId(100038);
    }
    void Update () {



	}
    float xMin;
    float xMax;
    float yMax;
    float ymin;
    float xDistance;
    public float yDistance;
    public void init() {
         mHero = GameObject.Find("Manager").GetComponent<LevelManager>().mPlayerControl;
        xMin = JsonUtils.getIntance().getConfigValueForId(100034);
        xMax = JsonUtils.getIntance().getConfigValueForId(100035);
        yMax = JsonUtils.getIntance().getConfigValueForId(100036);
        ymin = JsonUtils.getIntance().getConfigValueForId(100037);


         xDistance = (xMax - xMin) / 6;
         yDistance = (yMax - ymin) / 6;

        for (int i = 0; i < 3; i++)
        {
            for (int ii = 0; ii < 3; ii++)
            {
                PetLocalDate local = new PetLocalDate();
                local.x = xMin + (ii * 2 + 1) * xDistance;
                local.y = ymin + (i * 2 + 1) * xDistance;
                local.id = -1;
                mLocalList.Add(local);
            }
        }
        List<PlayerBackpackBean> list = InventoryHalper.getIntance().getPet();
        foreach (PlayerBackpackBean b in list) {
            if (b.goodType == SQLDate.GOOD_TYPE_USER_PET) {
                petFight(b.goodId);
            }
        }
        

    }
    public void addPet(long id) {
        List<PlayerBackpackBean> list = InventoryHalper.getIntance().getPet();
        foreach (PlayerBackpackBean b in list) {
            if (b.goodId == id) {
                addPet(b);
            }
        }
    }

    public void addPet(PlayerBackpackBean  bean) {
        GameObject.Find("pet").GetComponent<PetControl>().addPet(bean);
    }
    public bool petReset(long id) {
        mPetCount--;
        mHero.mSkillManager.removeSkill(mControlList[id].gameObject.GetComponent<PetItemControl>().mJson);
        Destroy(mControlList[id].gameObject);
        mControlList.Remove(id);

        return true;
    }
    public bool petFight(long id) {
        if (mPetCount >= JsonUtils.getIntance().getConfigValueForId(100038))
        {
            return false;
        }
        mPetCount++;
        PetLocalDate date = null;
        foreach (PetLocalDate local in mLocalList)
        {
            if (local.id == -1)
            {
                date = local;
                date.id = id;
                break;
            }
        }
        GameObject newobj = GameObject.Instantiate(
             mPetGj, new Vector3(-10, - 10, (-0.1f* mPetCount)), Quaternion.Euler(0.0f, 0f, 0.0f));
        PetItemControl pet = newobj.GetComponent<PetItemControl>();
        pet.init(date,this);
        mControlList.Add(id, pet);
        Debug.Log("PetItemControl pet=" + pet);
        Debug.Log("PetItemControl mHero=" + mHero);
        Debug.Log("PetItemControl mHero.mSkillManager=" + mHero.mSkillManager);
        mHero.mSkillManager.
            addSkill(pet.mJson, mHero);
        return true;
    }
    public Point getNewMoveTarget(long id) {
        Point point = null;
     /*  int range = Random.Range(0,9);
        PetLocalDate old = null;
        foreach (PetLocalDate date in mLocalList)
        {
            if (date.id == id)
            {
                old = date;
                break;
            }
        }*/
       float x = Random.Range(xMin, xMax);
       float y = Random.Range(ymin, yMax);
       point = new Point(x, y);
        
  /*          while (true) {
               if (range >= mLocalList.Count)
               {
                   range = 0;
               }
               if (mLocalList[range].id == -1)
               {
                  point = new Point(mLocalList[range].x, mLocalList[range].y);
                   mLocalList[range].id = id;                
                   break;
               }
               else
               {
                   range++;
               }
           }    


        old.id = -1;*/
        return point;
    }
    
}
