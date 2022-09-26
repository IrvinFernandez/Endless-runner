using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class TrainingManagerSecF : MonoBehaviour
{    
    //Este Script es para hacer una serie de pasos en un capacitacion, segun el paso en el que estes se prenden y apagan objetos para indicarte como se hace cada paso.
    //Este Codigo lo escrini yo desde 0


    [SerializeField] AudioClip stepClip;

    [SerializeField] GameObject MsjFinal01;

    [Header("Step 1")]    
    [SerializeField] SelectableTool ObjSeleccionableA01;
    //[SerializeField] SelectableTool ObjSeleccionableB01;

    [Header("Step 2")]
    [SerializeField] SelectableTool ObjSeleccionableA02;
    //[SerializeField] SelectableTool ObjSeleccionableB02;

    [Header("Step 3")]
    [SerializeField] AutomaticLever ObjSeleccionableA03;
    //[SerializeField] SelectableTool ObjSeleccionableA03;
    //[SerializeField] SelectableTool ObjSeleccionableB03;

    [Header("Step 4")]
    [SerializeField] SelectableTool ObjSeleccionableA04;
    //[SerializeField] SelectableTool ObjSeleccionableB04;

    [Header("Step 5")]
    //[SerializeField] SelectableTool ObjSeleccionableA05;
    [SerializeField] SnapZone Abrazadera01;
    [SerializeField] GameObject LiquidoTazon01;
    [SerializeField] GameObject Vasos01;
    
    [Header("Step 6")]
    [SerializeField] AutomaticLever ObjSeleccionableA06;
    //[SerializeField] SelectableTool ObjSeleccionableB06;
    
    int currentStep = 0;   
    int baseStep = 0;
    int offsetStep;    

    private void Start()
    {
        offsetStep = baseStep - currentStep;

        GetComponent<ArrowsCtrl01>().HideAll01();
        DisableAllSteps();
        EnableCurrentStep();
    }

    private void Update()
    {        
        switch (currentStep)
        {
            case 0: // paso 1

                if ((ObjSeleccionableA01.Complete))
                {
                    currentStep = 1;
                    OnFinishStep();
                }
                
                break;

            case 1: // paso 2

                if (ObjSeleccionableA02.Complete)
                {
                    currentStep = 2;
                    OnFinishStep();
                }
                
                break;

            case 2: // paso 3

                if (ObjSeleccionableA03.Open)
                {
                    currentStep = 3;
                    OnFinishStep();
                }
               
                break;
            case 3: // paso 4

                if (ObjSeleccionableA04.Complete)
                {
                    currentStep = 4;
                    OnFinishStep();
                }
               
                break;
            case 4: // paso 5

                if (Abrazadera01.HeldItem != null && LiquidoTazon01.activeSelf==true)
                {
                    currentStep = 5;
                    OnFinishStep();
                }

                break;
            case 5: // paso 6

                if (ObjSeleccionableA06.Open)
                {
                    currentStep = 6;
                    OnFinishStep();
                }

                break;            
            case 6: // etapa final
                currentStep = 7;
                MsjFinal01.gameObject.SetActive(true);
                
                GetComponent<AudioSource>().PlayOneShot(stepClip);
                break;
            default:
                break;
        }
    }
    void DisableAllSteps()
    {
        MsjFinal01.gameObject.SetActive(false);

        //
        ObjSeleccionableA01.gameObject.SetActive(false);

        // 
        ObjSeleccionableA02.gameObject.SetActive(false);
        
        //
        ObjSeleccionableA03.gameObject.SetActive(false);
     
        //
        ObjSeleccionableA04.gameObject.SetActive(false);

        //
        Abrazadera01.gameObject.SetActive(false);
        Vasos01.gameObject.SetActive(false);

        //
        ObjSeleccionableA06.gameObject.SetActive(false);
        
    }
    void EnableCurrentStep()
    {
        switch (currentStep)
        {
            case 0: // paso 1

                ObjSeleccionableA01.gameObject.SetActive(true);
               
                GetComponent<ArrowsCtrl01>().EnabledArrows(0);
                break;

            case 1: // paso 2

                ObjSeleccionableA01.gameObject.SetActive(false);

                ObjSeleccionableA02.gameObject.SetActive(true);                
                
                GetComponent<ArrowsCtrl01>().HideAll02(0);
                GetComponent<ArrowsCtrl01>().EnabledArrows(1);
                break;

            case 2: // paso 3

                ObjSeleccionableA02.gameObject.SetActive(false);

                ObjSeleccionableA03.gameObject.SetActive(true);
                
                GetComponent<ArrowsCtrl01>().HideAll02(1);
                GetComponent<ArrowsCtrl01>().EnabledArrows(2);
                break;

            case 3: // paso 4

                ObjSeleccionableA03.gameObject.SetActive(false);

                ObjSeleccionableA04.gameObject.SetActive(true);

                GetComponent<ArrowsCtrl01>().HideAll02(2);
                GetComponent<ArrowsCtrl01>().EnabledArrows(3);
                break;

            case 4: // paso 5

                ObjSeleccionableA04.gameObject.SetActive(false);

                Abrazadera01.gameObject.SetActive(true);
                Vasos01.gameObject.SetActive(true);

                GetComponent<ArrowsCtrl01>().HideAll02(3);
                GetComponent<ArrowsCtrl01>().EnabledArrows(4);
               
                break;

            case 5: // paso 6

                Vasos01.gameObject.SetActive(false);

                ObjSeleccionableA06.gameObject.SetActive(true);
                
                GetComponent<ArrowsCtrl01>().HideAll02(4);
                GetComponent<ArrowsCtrl01>().EnabledArrows(5);
                
                break;

            case 6: // paso 7                
                ObjSeleccionableA06.gameObject.SetActive(false);
                break;                

            default:
                break;
        }        
    }
    
    void OnFinishStep()
    {
        EnableCurrentStep();
        
        GetComponent<AudioSource>().PlayOneShot(stepClip);
    }
    public void ResetCurrent()
    {
        currentStep = 0;
        baseStep = 0;
        offsetStep = 0;
        Debug.Log(currentStep);
        Debug.Log(baseStep);

        //offsetStep = baseStep - currentStep;

        GetComponent<ArrowsCtrl01>().HideAll01();
        //DisableAllSteps();
        EnableCurrentStep();

        

    }
}
