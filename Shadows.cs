using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadows : MonoBehaviour
{
    //Este script es para crear sombras atras del personajes para dar la sensacion de velocidad


    public static Shadows me;
    public GameObject Sombra;
    public List<GameObject> pool = new List<GameObject>();
    private float cronometro;
    public float speed;
    public Color _color;
    // Start is called before the first frame update
    private void Awake()
    {
        me = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject GetShadows()
    {
        for(int i= 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                pool[i].SetActive(true);
                pool[i].transform.position = transform.position;
                pool[i].transform.rotation = transform.rotation;
                pool[i].GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                pool[i].GetComponent<Solid>()._color = _color;
                return pool[i];
            }
        }
        GameObject obj = Instantiate(Sombra, transform.position, transform.rotation) as GameObject;
        obj.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        obj.GetComponent<Solid>()._color = _color;
        pool.Add(obj);
        return obj;
    }

    public void Sombras_skill()
    {
        cronometro += speed * Time.deltaTime;
        if(cronometro > 1)
        {
            GetShadows();
            cronometro = 0;
        }
    }
}
