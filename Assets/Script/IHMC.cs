using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class IHMC : MonoBehaviour
{
    private Logic game;
    private Network net;
    public Sprite bomb0;
    public Sprite bomb1;
    public Sprite bomb2;
    public Sprite bomb3;
    public Sprite bomb4;
    public Sprite bomb5;
    public Sprite bomb6;

    [SerializeField]
    private GameObject defShop;

    [SerializeField]
    private GameObject synShop;

    [SerializeField]
    private GameObject boom;

    [SerializeField]
    private AudioClip bombClip1;

    [SerializeField]
    private AudioClip bombClip2;

    [SerializeField]
    private AudioClip bombClip3;

    [SerializeField]
    private AudioClip bombClip4;

    [SerializeField]
    private AudioClip bombClip5;

    [SerializeField]
    private AudioClip bombClip6;

    [SerializeField]
    private AudioClip boomClip;

    [SerializeField]
    private AudioSource bombAudio;

    [SerializeField]
    private GameObject FireBreath1;

    [SerializeField]
    private GameObject FireBreath2;

    [SerializeField]
    private GameObject FireBreath3;

    [SerializeField]
    private GameObject FireBreath4;

    [SerializeField]
    private GameObject FireBreath5;

    [SerializeField]
    private GameObject FireBreath6;

    [SerializeField]
    private GameObject bomb;

    [SerializeField]
    private Canvas loadingC;
    
    [SerializeField]
    private TextMeshProUGUI showLetters;

    [SerializeField]
    private TextMeshProUGUI infoMotDef;

    [SerializeField]
    public TextMeshProUGUI infoDef;

    [SerializeField]
    private TextMeshProUGUI definition;

    [SerializeField]
    public TextMeshProUGUI infoSyn;

    [SerializeField]
    private GameObject fireWork1;

    [SerializeField]
    private GameObject fireWork2;

    [SerializeField]
    private GameObject skull;

    [SerializeField]
    private TextMeshProUGUI info;

    [SerializeField]
    private TextMeshProUGUI shopCoin;

    [SerializeField]
    private TextMeshProUGUI badletters;

    [SerializeField]
    private TextMeshProUGUI badwords;

    [SerializeField]
    private TextMeshProUGUI errorMessage;

    [SerializeField]
    private TextMeshProUGUI userName;

    [SerializeField]
    private TextMeshProUGUI userName1;

    [SerializeField]
    private TextMeshProUGUI IGUserName;

    [SerializeField]
    private Canvas winC;

    [SerializeField]
    private TextMeshProUGUI winMot;

    [SerializeField]
    private TextMeshProUGUI winScore;

    [SerializeField]
    private TextMeshProUGUI score;

    [SerializeField]
    private TextMeshProUGUI looseMot;

    [SerializeField]
    private TextMeshProUGUI looseScore;

    [SerializeField]
    private Canvas LooseC;

    [SerializeField]
    private Canvas Settings;

    [SerializeField]
    private Canvas infoC;
    // Start is called before the first frame update
    void Start()
    {
        game = GetComponent<Logic>();
        net = GetComponent<Network>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator waitError()//efface le message d'erreur après un certain temps 
    {
        yield return new WaitForSeconds(2f);
        errorMessage.text = "";
    }
    public void showletters()//affiche les lettres trouve par le joueur 
    {
        game = GetComponent<Logic>();
        showLetters.text = game.motComplet;
        Debug.Log(game.motComplet);
    }
    public void infomot()//affiche le nombre de lettres present
    {   
        game = GetComponent<Logic>();

        if(game.modeJeu == 1)
        {
            info.text = "Il y a ? lettres dans ce mot";
        }
        else 
        {
            info.text = "Il y a " + game.motChoisiString.Length + " lettres dans ce mot";
        }
    }
    public void badLetters()//affiche lettres tapées mais pas presente dans le mot 
    {
        game.badLetter = string.Concat(game.mauvaisesLettres);

        badletters.text = "Lettres déjà tapées : " + game.badLetter;
    }
    public void error(string message)//message d'erreur
    {
        errorMessage.text = message;
        StartCoroutine(waitError());
    }
    public void DisableCanvas() //active ou desactive le canvas de win ou de loose 
    {
        if(game.isWin)
        {
            winC.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            winC.GetComponent<Canvas>().enabled = false;
        }
        if(game.essaisRestants == 0)
        {
            LooseC.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            LooseC.GetComponent<Canvas>().enabled = false;
        }
    }
    public void infoScore()//affiche le score de joueur 
    {
        score.text = game.score.ToString();
        shopCoin.text = game.score.ToString();
    }
    public void endGame()//affiche le score du joueur et le mot de la partie a la fin de la partie
    {
        if(game.isWin == true)
        {
            winMot.text = game.motChoisiString;
            winScore.text = game.score.ToString();
        }
        else if(game.essaisRestants == 0)
        {
            looseMot.text = game.motChoisiString;
            looseScore.text = game.score.ToString();
        }
    }
    public void userNameInput()//affiche l'username du joueur 
    {
        game = GetComponent<Logic>();
        userName.text = game.playerName;
        userName1.text = game.playerName;
        IGUserName.text = game.playerName;
    }
    public void fireWorksStystem()//systeme de particules de fin de partie 
    {
        if(game.isWin == true)
        {
            fireWork1.SetActive(true);
            fireWork2.SetActive(true);
        }
        if(game.isWin == false)
        {
            fireWork1.SetActive(false);
            fireWork2.SetActive(false);
        }
        if(game.essaisRestants == 0)
        {
            skull.SetActive(true);
        }
        if(game.essaisRestants != 0)
        {
            skull.SetActive(false);
        }
    }
    public void showBomb()//met a jour la bombe en fonction du nombre d'essais restant
    {
        if(game.essaisRestants == 7)
        {
            bomb.SetActive(true);
            boom.SetActive(false);
            bomb.GetComponent<Image>().sprite = bomb0;
            FireBreath1.SetActive(false);
            FireBreath2.SetActive(false);
            FireBreath3.SetActive(false);
            FireBreath4.SetActive(false);
            FireBreath5.SetActive(false);
            FireBreath6.SetActive(false);
            bombAudio.loop = true;
            bombAudio.clip = bombClip1;
            bombAudio.Stop();
        }
        if(game.essaisRestants == 6)
        {
            game.bombAnim.gameObject.GetComponent<Animator>().speed = 1;
            bomb.GetComponent<Image>().sprite = bomb1;
            FireBreath1.SetActive(true);
            bombAudio.Play();
        }
        if(game.essaisRestants == 5)
        {
            game.bombAnim.gameObject.GetComponent<Animator>().speed = 2;
            bomb.GetComponent<Image>().sprite = bomb2;
            FireBreath1.SetActive(false);
            FireBreath2.SetActive(true);
            bombAudio.clip = bombClip2;
            bombAudio.Play();
        }
        if(game.essaisRestants == 4)
        {
            game.bombAnim.gameObject.GetComponent<Animator>().speed = 3;
            bomb.GetComponent<Image>().sprite = bomb3;
            FireBreath2.SetActive(false);
            FireBreath3.SetActive(true);
            bombAudio.clip = bombClip3;
            bombAudio.Play();
        }
        if(game.essaisRestants == 3)
        {
            game.bombAnim.gameObject.GetComponent<Animator>().speed = 4;
            bomb.GetComponent<Image>().sprite = bomb4;
            FireBreath3.SetActive(false);
            FireBreath4.SetActive(true);
            bombAudio.clip = bombClip4;
            bombAudio.Play();
        }
        if(game.essaisRestants == 2)
        {
            game.bombAnim.gameObject.GetComponent<Animator>().speed = 5;
            bomb.GetComponent<Image>().sprite = bomb5;
            FireBreath4.SetActive(false);
            FireBreath5.SetActive(true);
            bombAudio.clip = bombClip5;
            bombAudio.Play();
        }
        if(game.essaisRestants == 1)
        {
            game.bombAnim.gameObject.GetComponent<Animator>().speed = 6;
            bomb.GetComponent<Image>().sprite = bomb6;
            FireBreath5.SetActive(false);
            FireBreath6.SetActive(true);
            bombAudio.clip = bombClip6;
            bombAudio.Play();
        }
        if(game.essaisRestants == 0)
        {
            bomb.SetActive(false);
            FireBreath6.SetActive(false);
            bombAudio.clip = boomClip;
            bombAudio.loop = false;
            bombAudio.Play();
            boom.SetActive(true);
        }
    }
    public void showDef()//affiche la definition
    {
        infoDef.text = game.defChoisiString;
    }
    public void showSyn()//affiche le synonyme
    {
        infoSyn.text = game.synChoisiString;
    }
    public void loadingCanvas()//canvas de loading en debut de partie et au restart
    {
        if(game.motChoisiString.Length == 0 && game.defChoisiString.Length == 0 && game.synChoisiString.Length == 0)
        {
            loadingC.GetComponent<Canvas>().enabled = true;
        }
        else if(game.motChoisiString.Length != 0 && game.defChoisiString.Length != 0 && game.synChoisiString.Length != 0)
        {
            loadingC.GetComponent<Canvas>().enabled = false;
        }
    }
    public void def()//affiche la definition dans le mode de jeu 1
    {
        definition.text = game.defChoisiString;
        infoMotDef.text = game.motComplet;
    }
    public void triedWords()//affiche les mots deja tapés dans le mot de jeu 1
    {
        game.badWords = string.Concat(game.mauvaisMot);

        badwords.text = "Mots déjà tapés : " + game.badWords;
    }
    public void shopItems()//affiche soit la definition soit le synonyme dans le shop en fonction du mode de jeu 
    {
        if(game.modeJeu == 0)
        {
            defShop.SetActive(true);
            synShop.SetActive(false);
        }
        if(game.modeJeu == 1)
        {
            defShop.SetActive(false);
            synShop.SetActive(true);
        }
    }
}