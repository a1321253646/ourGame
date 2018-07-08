using UnityEngine;
using System.Collections;

public class UiManager 
{
	public void changeHeroBlood(float scanle){
	
	}

	public void addGasAndCrystal(int gas,int Crystal){
	
	}


	public void showStarBoss(){
	
	}
	public void showLevelUp(){

	}

	private void startBoss(){
		GameManager.getIntance ().startBoss ();
	}
	private void levelUp(){
		GameManager.getIntance ().heroUp ();
	}

}

