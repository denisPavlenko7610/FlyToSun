using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
   
    int score;


    
    float timer;
    float maxTime;


    // Start is called before the first frame update
    void Start()
    {
        

       
        score = 0;
        
        maxTime = 0.15f;



    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;
        if(timer >= maxTime)
        {
            score++;
            
          
            timer = 0;
            

            if (score % 100 == 0)
            {
                
          
                Time.timeScale += 0.05f;

                
                
            }


        }

        
    }

    
}
