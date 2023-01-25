using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class Network : MonoBehaviour
{
    private Logic game;
    private IHMC IHM;
    public MotR[] motr = null;
    public MotR1[] motr1 = null;
    
    int defTry = 0;
    int synTry = 0;

   void Start()
    {
        game = GetComponent<Logic>();
        IHM = GetComponent<IHMC>();
        
        newGame();
    }
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))//web request pour mot,definition,synonyme
        {
            // On envoie la requête et on attend la réponse
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                motr = JsonConvert.DeserializeObject<MotR[]>(webRequest.downloadHandler.text);
            }
            
            foreach (MotR rootObject in motr)
            {
                game.motChoisiString = rootObject.mot;
                game.defChoisiString = rootObject.definition;
            }
        }
        game.choixMot();
        Invoke("newGame", 5.0f);
    }
    public void newGame()
    {
        IHM.loadingCanvas();
        if(game.motChoisiString.Length == 0)
        {
            Debug.Log("IN LOAD MOT");
            loadMot();
        }
        else if(game.motChoisiString.Length != 0)
        {
            if(defTry == 0)
            {
                Debug.Log("IN DEF TRY 0");
                defTry = 1;
                loadDef();
            }
            else if(defTry == 1 && game.defChoisiString.Length == 0)
            {
                Debug.Log("IN DEF TRY 1 def null");
                defTry = 0;
                loadMot();
            }
            else if(synTry == 0 && game.defChoisiString.Length != 0)
            {
                Debug.Log("IN DEF TRY 1 def !null LOAD SYN");
                synTry = 1;
                loadSyn();
            }
            else if(synTry == 1 && game.synChoisiString.Length == 0)
            {
                defTry = 0;
                synTry = 0;
                game.motChoisiString = "";
                game.defChoisiString = "";
                game.synChoisiString = "";
                loadMot();
            }
            else if(game.synChoisiString.Length != 0)
            {
                Debug.Log("\\\\ALL LOAD////");
                defTry = 0;
                synTry = 0;
            }
            IHM.def();
            IHM.loadingCanvas();
        }
    }
    public void loadMot()
    {
        StartCoroutine(GetRequest("https://api.dicolink.com/v1/mots/motauhasard?avecdef=true&minlong=5&maxlong=-1&verbeconjugue=false&api_key=rON7EygLDvbBstbmAZISOfwEv14WtVw4"));

        StartCoroutine(GetRequest("https://error.html"));
    }
    public void loadDef()
    {
        StartCoroutine(GetRequest("https://api.dicolink.com/v1/mot/" + game.motChoisiString + "/definitions?limite=200&api_key=rON7EygLDvbBstbmAZISOfwEv14WtVw4"));

        StartCoroutine(GetRequest1("https://error.html"));
    }
    public void loadSyn()
    {
        StartCoroutine(GetRequest1("https://api.dicolink.com/v1/mot/" + game.motChoisiString + "/synonymes?limite=5&api_key=rON7EygLDvbBstbmAZISOfwEv14WtVw4"));

        StartCoroutine(GetRequest1("https://error.html"));
    }
    IEnumerator GetRequest1(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // On envoie la requête et on attend la réponse
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                motr1 = JsonConvert.DeserializeObject<MotR1[]>(webRequest.downloadHandler.text);
            }
            
            foreach (MotR1 rootObject in motr1)
            {
                game.synChoisiString = rootObject.mot;
            }
        }
    }
}
[System.Serializable]
public class MotR
{
    public string error;
    public string mot = "";
    public string definition = "";
    public string synonyme = "";
    public string champlexical;
    public string name;
    public string message;
    public string code;
    public string status;
    public string dicolinkUrl;
    public string id;
    public string nature;
    public string source;
    public string attributionText;
    public string attributionUrl;
}
[System.Serializable]
public class MotR1
{
    public string mot = "";
    public string synonyme = "";
}