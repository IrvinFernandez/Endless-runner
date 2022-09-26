using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Ranking : MonoBehaviour
{
    //Este codigo es para descargar, cargar y mostrar las mejores puntaciones de una tabla de puntajes,es un codigo generico, lo hice siguiendo en un tutorial de youtube, pero es totalmente funcional.
    //Aunque actualmente uso los Servicios de Google Play Game para manejar los Logros y Tablas de Puntaje, pero esta es una buena opcion para juegos que no son para celular.


    // Start is called before the first frame update
    [System.Serializable]

    public struct estructuraDatosWeb
    {
        [System.Serializable]
        public struct registro
        {
            public string nombre;
            public int puntaje;
        }
        public List<registro> registros;

    }
    public estructuraDatosWeb datos;

    public Transform Tabla;
    //nuevo record
    public Transform nuevo;

    public GameObject PlantillaRegistros;
    int cantidadderegistros = 5;
   //la variable puntaje
    public int Mipuntaje;
    //donde se pone el nombre
    public TMPro.TMP_InputField Minombre;


   

    //next}
    public GameObject Next;
    [ContextMenu("Leer")]
    public void Leer(System.Action accionAlTerminar)
    {
        StartCoroutine(CorrutinaLeer(accionAlTerminar));
    }
    private IEnumerator CorrutinaLeer(System.Action accionAlTerminar )
    {
        UnityWebRequest web = UnityWebRequest.Get("http://pipasjourney.com/compartido/rankingtenyo.txt");
        yield return web.SendWebRequest();
        //esperamos a que vuelva
        //volvio
        if(!web.isNetworkError && !web.isHttpError)
        {
            //todo ok
            datos = JsonUtility.FromJson<estructuraDatosWeb>(web.downloadHandler.text);
            accionAlTerminar();
        }
        else
        {
            Debug.LogWarning("Hubo un problema al leer archivo");
            SceneManager.LoadScene("Menu");
        }
    }

    [ContextMenu("Escribir")]

    public void Escribir()
    {
        StartCoroutine(CorrutinaEscribir());
    }
    private IEnumerator CorrutinaEscribir()
    {
        WWWForm form = new WWWForm();
        form.AddField("archivo", "rankingtenyo.txt");
        form.AddField("texto", JsonUtility.ToJson(datos));

        UnityWebRequest web = UnityWebRequest.Post("http://pipasjourney.com/compartido/escribir.php", form);

        yield return web.SendWebRequest();
        //esperamos que vuelva
        //volvio
        if(!web.isNetworkError && !web.isHttpError)
        {
            //todo ok
            Debug.Log(web.downloadHandler.text);

        }
        else
        {
            Debug.LogWarning("Hubo un problema al escribir el archivo");
            SceneManager.LoadScene("Menu");
        }
    }
    [ContextMenu("Crear Tabla")]

    void CrearTabla()
    {
        for (int i = 0; i < cantidadderegistros; i++)
        {
             GameObject inst = Instantiate(PlantillaRegistros, Tabla);
            inst.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, i * -90f);
            inst.name = i.ToString();
        }
    }
    [ContextMenu("Pasar Datos a Tabla")]

    void PasarDatosaTabla()
    {
        for (int i = 0; i < cantidadderegistros; i++)
        {
            Tabla.GetChild(i).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = datos.registros[i].nombre;
            Tabla.GetChild(i).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = datos.registros[i].puntaje.ToString();
        }
    }


    [ContextMenu("chequear si corresponde")]
    void ChequearsiCorrespondeNuevoHiscore()
    {
      if(LogicaPlayer.Puntos > datos.registros[cantidadderegistros - 1].puntaje )
        {
            Tabla.gameObject.SetActive(false);
            nuevo.gameObject.SetActive(true);
         
        }
        else
        {
            Tabla.gameObject.SetActive(true);
            nuevo.gameObject.SetActive(false);
            Next.gameObject.SetActive(true);
          
        }
    }

    [ContextMenu("insertar registro")]
    void InsertarNuevoRegistro()
    {
        //saber en que posicion debe va insertar
        for (int i = 0; i < cantidadderegistros; i++)
        {
          if( LogicaPlayer.Puntos > datos.registros[i].puntaje)
            {
                //inserto
                datos.registros.Insert(i, new estructuraDatosWeb.registro()
                {

                    nombre = Minombre.text,
                    puntaje = LogicaPlayer.Puntos
                });

                break; //para que salga del for
            }
        }

    }

    void Start()
    {
        Leer(CrearTablaPasarDatosyChequear);
       
    }
    void CrearTablaPasarDatosyChequear()
    {
        CrearTabla();
        PasarDatosaTabla();
        ChequearsiCorrespondeNuevoHiscore();
    }

    public void InputTermino()
    {
        nuevo.gameObject.SetActive(false);
        Tabla.gameObject.SetActive(true);
        Next.gameObject.SetActive(true);
        Leer(InsertaryEscribir);
    }
    void InsertaryEscribir()
    {
        InsertarNuevoRegistro();
        Escribir();
        PasarDatosaTabla();
    }
   public void PasaraInicio()
    {
        SceneManager.LoadScene("Menu");
    }
}
