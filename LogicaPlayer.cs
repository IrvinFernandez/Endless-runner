using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicaPlayer : MonoBehaviour
{
    //Este codigo es para detectar el deslizamiento del dedod por el celular para mover al personaje en 3 carriles, muy usado en juegos Endless runner como Subway surfers



    //Variables para el movimiento
    public float speed;

    [Range(-1,1)]public int posicion;
   
    public Vector3 finalPosicion;


    public bool IrDerecha;
    public bool IrIzq;
    public float VolverFalso=0.3f;
    

    private Vector2 fp; // first finger position
    private Vector2 lp; // last finger position


    void Start()
    {
        murio = false;
        acertoRespuesta = false;
        posicion = 0;
        
    }
    // Update is called once per frame

    void Update()
    {

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
            }
            if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if ((fp.x - lp.x) > 15) // left swipe
                {
                    IrDerecha = false;
                    IrIzq = true;
                    CancelInvoke();
                    Invoke("TodoFalso", VolverFalso);
                }
                else if ((fp.x - lp.x) < -15) // right swipe
                {
                    IrDerecha = true;
                    IrIzq = false;
                    CancelInvoke();
                    Invoke("TodoFalso", VolverFalso);
                }
                
            }
        }
      


        if (transform.position == finalPosicion)
        {
            if (IrDerecha == true && posicion < 1)
            {
                finalPosicion = new Vector3(transform.position.x + 1.3f, transform.position.y, transform.position.z);
                posicion++;
            }
            if (IrIzq == true && posicion > -1)
            {
                finalPosicion = new Vector3(transform.position.x - 1.3f, transform.position.y, transform.position.z);
                posicion--;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, finalPosicion, speed * Time.deltaTime);

    }
    
    public void TodoFalso()
    {
            IrIzq = false;
            IrDerecha = false;
    }


    

}
