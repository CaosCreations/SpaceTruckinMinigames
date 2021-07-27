using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryRchargegame : MonoBehaviour
{


    [SerializeField] Transform rightPivot;
    [SerializeField] Transform leftPivot;
    [SerializeField] Transform powCore;


//capsule thingg
     [SerializeField] Transform BateryCapsule;
     float capsulePosition;
     [SerializeField] float capsuleSize=0.1f;


     //the bar fiiling stuff
     [SerializeField] float capsPower=1f;

       [SerializeField] float badcapsPower=0.5f;


     float capsProgress;
      float capsRegress;
     float pullSpeed;
     [SerializeField] float pullPower=0.1f;
     [SerializeField] float capsGravity=0.0005f;

     [SerializeField] float progressDecay=0.1f;



    float powPosition;
    float powDest;

    float powTimer;
    [SerializeField] float powMultiplicator=3f;
    float powSpeed;
     [SerializeField] float smothMotion=1f;

     [SerializeField] SpriteRenderer capsRenders;
     [SerializeField] Transform progressbarContainer;
     [SerializeField] Transform badbarContainer;
     float badBarProgress;
     bool pause;
     [SerializeField] float failTimer=10f;







     void Start()
      {

          Resize();
         
     }





    // Update is called once per frame
    void Update()
    {
        if(pause){ return;}

        BatteryMeter();
        CapsuleControler();
        ProgressCheck();



    }

         private void Resize()
     {
         Bounds b = capsRenders.bounds;
         float xSize= b.size.x;
         Vector3 ls= BateryCapsule.localScale;
         float distance = Vector3.Distance(rightPivot.position,leftPivot.position);
         ls.x=(distance/xSize * capsuleSize);
         BateryCapsule.localScale=ls;


     }

    void ProgressCheck()
    {
        Vector3 ls= progressbarContainer.localScale;

        //overchargeBar
        Vector3 lRs= badbarContainer.localScale;


        ls.y =capsProgress;
        lRs.y=capsRegress;
        
        progressbarContainer.localScale=ls;
        badbarContainer.localScale=lRs;

        float min = capsulePosition - capsuleSize /2;
        float max = capsulePosition + capsuleSize/2;

//inside the bar
        if(min < powPosition && powPosition <max)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
            capsProgress+= capsPower*Time.deltaTime;
            capsRegress+=0;
            ComboManager.instance.SetCombo();
            }

        }
        else{
            

            if(Input.GetKeyDown(KeyCode.Q))
            {
            capsRegress+= capsPower*Time.deltaTime;
            badBarProgress+= badcapsPower*Time.deltaTime;
            ComboManager.instance.ResetCombo();
           
            
            }

            capsProgress-= progressDecay * Time.deltaTime;

            //fail timer stuff maybe another bar later
            failTimer-=Time.deltaTime;
            if(failTimer <-5f)
            {
                Lose();
            }

         



        }

        if(capsProgress>=1f)
        {
            Win();
        }

        
        if(badBarProgress>=1f)
        {
            Lose();
        }



        capsProgress = Mathf.Clamp(capsProgress,0f,1f);
        capsRegress = Mathf.Clamp(capsRegress,0f,1f);



    }



    void CapsuleControler()
    {


        if(Input.GetKey(KeyCode.E))
        {
            pullSpeed+=pullPower * Time.deltaTime;

        }
        pullSpeed-=capsGravity * Time.deltaTime;
        capsulePosition+=pullSpeed;



        if(capsulePosition - capsuleSize/2 <= 0f && pullSpeed<0f)
        {
            pullSpeed=0f;


        }
        if(capsulePosition + capsuleSize/2>= 1f && pullSpeed>0f)
        {
            pullSpeed=0f;


        }




        capsulePosition= Mathf.Clamp(capsulePosition,capsuleSize/2,1-capsuleSize/2);
        BateryCapsule.position= Vector3.Lerp(leftPivot.position, rightPivot.position,capsulePosition);








    }



    void BatteryMeter()
    {
        
        powTimer-=Time.deltaTime;
        if(powTimer<0f)
        {
            powTimer= Random.value * powMultiplicator;

            powDest=Random.value;

        }

        powPosition=Mathf.SmoothDamp(powPosition,powDest,ref powSpeed,smothMotion);
        powCore.position =Vector3.Lerp(leftPivot.position, rightPivot.position,powPosition);
        

    }



    
    private void Win()
    {
        pause=true;
        Debug.Log("Flawless boyyy");

    }

    private void Lose()
    {
        pause=true;
        Debug.Log("Looose boyyy");
    }







}
