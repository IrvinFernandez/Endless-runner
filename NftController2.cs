using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class NftController2 : MonoBehaviour
{
    //Este es un Script para recibir los datos de la web y mostrarlos en formato de Ui tipo NFT en Unity, es automaticos, detecta la cantidad de los nfts que tienes en la web y los crea en unity
    //Este Script lo cree yo desde cero, excepto la parte de Photon, si se manejarlo, pero en este proyecto no me encargaba de eso.



    static string URI_NFTS = "https://api-metaverse.minimetaverse.tk/public/api/nfts";

    static string URI_TRADE = "https://api-metaverse.minimetaverse.tk/public/api/nfts/trade";

    static string URI_TODOSnft = "https://api-metaverse.minimetaverse.tk/public/api/nfts/all";

    static string URI_TradeMarketplace = "https://marketplace.minimetaverse.tk/";
    //UI IRVIN
    public GameObject CargandoDesactivar;
    public GameObject[] CargandoDes;

    public GameObject Usuario1;
    public string IdDelElegido;
    // string para photon
    public static string IdDelElegidoStatic;

    public string IDdelNFTaCambiar;
    public  TextMeshProUGUI NickName;
    [SerializeField] VariablesIntercambio variables;
   
    bool buscar = false;

    public Image[] ImagenesURL;
    public int NumeroImagenURL =0;
    public int NumeroSumando =0;


    GameObject LineaUIprefap;

    public string url1;
    //Automatico
    public GameObject PadreLinea;
    public GameObject LineaPrefap;
    public GameObject ImagenNuevaLinea;
    public Image ImageNuevo;
    public int tamañoDeLista;
    //titulo
    public GameObject objetoTextTitulo;
    public TextMeshProUGUI Titulo;
    public TextMeshProUGUI[] TextTitulos;
    //id
    public GameObject objetoTextID;
    public TextMeshProUGUI id;
    public TextMeshProUGUI[] TextID;


    [System.Serializable]
    public struct Nft
    {
        public int id;
        public string title;
        public string   image;
        public int inventory_id;
        public string created_at;
        public string updated_at;
    }
    
    [System.Serializable]
    public struct Response
    {
        public List<Nft> nfts;
        public string message;
    }

    public Response response;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject.Find("PostButton").GetComponent<Button>().onClick.AddListener(addNfts);
        //GameObject.Find("GetButton").GetComponent<Button>().onClick.AddListener(getNfts);
        StartCoroutine(GetDataApiNfts());
        NickName.text = LoginController.NickNameJ;
        
        
        Debug.Log("este es el nombre " +LoginController.NickNameJ);
    }
    private void Update()
    {
        if (DropSlot.colocoEnElSlot == true)
        {
            MandarIdNftElegido();
            DropSlot.colocoEnElSlot = false;
        }
    }
    public void addNfts()
    {
        StartCoroutine(PostDataApiNfts());
    }


    [ContextMenu("getNfyts")]
    public void getNfts()
    {
        StartCoroutine(GetDataApiNfts());
    }


    public void Hacertrade()
    {
        StartCoroutine(PostDataApiNfts());
    }
    
    IEnumerator PostDataApiNfts()//Esta funcion hace el intercambio
    {
        WWWForm form = new WWWForm();

        form.AddField("other_user_id", 3);
        form.AddField("my_nft_id", IdDelElegido);
        form.AddField("other_nft_id", variables.IdNftList[1]);
        using (UnityWebRequest request = UnityWebRequest.Post(URI_TRADE, form))
        {
            request.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + SessionWeb.user.access_token);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                //outputArea.text = request.error;
                Debug.LogWarning(request.downloadHandler.text+"Si funciono el trade");
                //session = JsonUtility.FromJson<SesionObject>(web.downloadHandler.text);

                
               
            }
            else
            {
                //luego del intercambio pasara....
                //
                //
                //outputArea.text = request.downloadHandler.text;
                Debug.Log(request.downloadHandler.text);
                Debug.Log("Si funciono el trade");
                StartCoroutine(LuegoDelTrade());
            }
        }

    }
    public IEnumerator LuegoDelTrade()
    {
        yield return 0;
        NumeroSumando = 0;
        NumeroImagenURL = 0;
        //eliminar los nft actuales
        Destroy(LineaUIprefap);
        Destroy(PadreLinea.transform.GetChild(1).gameObject);
        Destroy(PadreLinea.transform.GetChild(2).gameObject);
        //Denuevo descargar los NFT de la web
        StartCoroutine(GetDataApiNfts());
    }


    IEnumerator GetDataApiNfts()//descargar Nfts de la web.
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URI_NFTS))
        {
            request.SetRequestHeader("Authorization", "Bearer " + SessionWeb.user.access_token);
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                //outputArea.text = request.error;
            }
            else
            {
                //outputArea.text = request.downloadHandler.text;
                Debug.Log(request.downloadHandler.text);
                response = JsonUtility.FromJson<Response>(request.downloadHandler.text);

                Debug.Log(response.message);

                foreach (var nft in response.nfts )
                {
                    Debug.Log(nft.title);
                }


               //crear nuevas lineas en el  UI
                tamañoDeLista = response.nfts.Count;
                for (int i = 0; i < response.nfts.Count; i++)
                {
                    //Crear la nueva linea
                    GameObject nuevaLinea = Instantiate(LineaPrefap.gameObject, PadreLinea.transform);

                    //Asinar al array las Imagenes
                    ImagenNuevaLinea = nuevaLinea.transform.GetChild(0).gameObject;
                    ImageNuevo = ImagenNuevaLinea.GetComponent<Image>();
                    ImagenesURL[i] = ImageNuevo;
                    //Asinar al array los Textos Titulo
                    objetoTextTitulo = ImagenNuevaLinea.transform.GetChild(0).gameObject;
                    Titulo = objetoTextTitulo.GetComponent<TextMeshProUGUI>();
                    TextTitulos[i] = Titulo;
                    //Asignar al array de id los textosID
                    objetoTextID = ImagenNuevaLinea.transform.GetChild(1).gameObject;
                    id = objetoTextID.GetComponent<TextMeshProUGUI>();
                    TextID[i] = id;
                    //Asinar al array los textos cargando
                    CargandoDesactivar = nuevaLinea.transform.GetChild(1).gameObject;
                    CargandoDes[i] = CargandoDesactivar;
                    



                }
                CargarDatos();
            }
            
        }

    }
    
    private void CargarDatos()
    {
           // create WWW object pointing to the url
            // start loading whatever in that url ( delay happens here )

       //Asignar Url
        url1 = response.nfts[NumeroSumando].image;
        NumeroImagenURL = NumeroSumando;
        StartCoroutine(LoadFromLikeCoroutine());
        
    }
   
    private IEnumerator LoadFromLikeCoroutine()//Esta funcion busca las url de las imagenes y las pone en los sprites.
    {
        Debug.Log("Loading ....");
        WWW wwwLoader = new WWW(url1);   // create WWW object pointing to the url
        yield return wwwLoader;         // start loading whatever in that url ( delay happens here )

        Debug.Log("Loaded");

        //Colocando el Sprite de web en la cada Imagen
        ImagenesURL[NumeroImagenURL].sprite = Sprite.Create(wwwLoader.texture, new Rect(0, 0, wwwLoader.texture.width, wwwLoader.texture.height), new Vector2(0, 0));  // set loaded image
       
        

        //Colocando el Texto de la web en cada Titulo
        TextTitulos[NumeroSumando].text = response.nfts[NumeroSumando].title;
        //colocando el texto id a cada texto
        TextID[NumeroSumando].text = response.nfts[NumeroSumando].id+"";
        //Desactivar el texto Cargando
        CargandoDes[NumeroSumando].SetActive(false);
        NumeroSumando++;
        

        CargarDatos();
    }
    public void IDdelNftElegido()
    {
        
        GameObject ImageDeLineaUI;
        GameObject TextoIDdelNFT;
        LineaUIprefap = Usuario1.transform.GetChild(2).gameObject;
        ImageDeLineaUI = LineaUIprefap.transform.GetChild(0).gameObject;
        TextoIDdelNFT = ImageDeLineaUI.transform.GetChild(1).gameObject;

        IdDelElegido = TextoIDdelNFT.GetComponent<TextMeshProUGUI>().text;
        //para photon 
        IdDelElegidoStatic = IdDelElegido;
        Debug.Log(IdDelElegido);
        StartCoroutine(PostDataApiNfts());
    }
    public void MandarIdNftElegido()
    {
        GameObject ImageDeLineaUI;
        GameObject TextoIDdelNFT;
        LineaUIprefap = Usuario1.transform.GetChild(2).gameObject;
        ImageDeLineaUI = LineaUIprefap.transform.GetChild(0).gameObject;
        TextoIDdelNFT = ImageDeLineaUI.transform.GetChild(1).gameObject;

        IdDelElegido = TextoIDdelNFT.GetComponent<TextMeshProUGUI>().text;
        IdDelElegidoStatic = IdDelElegido;
        Debug.Log("nft "+ IdDelElegido);
    }
}
