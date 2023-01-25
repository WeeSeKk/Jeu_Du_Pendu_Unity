using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using TMPro.EditorUtilities;

public class Logic : MonoBehaviour 
{
    private IHMC IHM;
    public Network net;
    public Animator bombAnim;
    [SerializeField]
    private GameObject iF; //Input field 
    public bool isWin = false;
    public int essaisRestants = 7;
    public int modeJeu;
    public string lettreChoisie;
    public string motChoisiString = "";
    public string defChoisiString = "";
    public string synChoisiString = "";
    public string[] foundletters = new string[25];
    public List<string> mauvaisesLettres = new List<string>();
    public List<string> mauvaisMot = new List<string>();
    public string[] accents = new string[] {"é","è","ê","à","â","î","ù","û","ô"};//list d'accents possible dans les mots
    public string motComplet;
    public string badLetter;
    public string badWords;
    public int point = 0;
    public string playerName;
    public float score;
    // Start is called before the first frame update
    void Start()
    {
        IHM = GetComponent<IHMC>();
        net = GetComponent<Network>();
        bombAnim.gameObject.GetComponent<Animator>().speed = 0; //stop l'annimation de la bomb
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void choixMot() 
    {
        Array.Clear(foundletters,0,foundletters.Length); 

        for(int i = 0; i<motChoisiString.Length; i++) //rempli l'array avec " _" pour chaque lettre du mot choisi
        {
            foundletters[i] = "_ " ;
        }

        motComplet = string.Concat(foundletters); //Concat de l'array
        IHM.showletters();
        IHM.infomot();
    }
    public void gameMode(int mode)
    {
        if(mode == 0)//mode de jeu du pendu 
        {
            modeJeu = 0;
            choixMot();
            IHM.infoScore();
            IHM.badLetters();
            IHM.shopItems();
            IHM.showBomb();
        }
        if(mode == 1)//mode de jeu avec definition 
        {
            modeJeu = 1;
            choixMot();
            IHM.infoScore();
            IHM.badLetters();
            IHM.shopItems();
            IHM.showBomb();
            IHM.def();
        }
    }
    public void readInput(string letter)//read input mode de jeu 0
    {
        lettreChoisie = letter;

        if(checkLettre(lettreChoisie))
        {
            if (motComplet.Contains(lettreChoisie))
            {
                IHM.error("lettre déjà trouvée");
            }
            else if (badLetter.Contains(lettreChoisie))
            {
                IHM.error("lettre déjà tapée");
            }
            else
            {
                boucleJeu();
            }
        }
        else
        {
            IHM.error("Ceci n'est pas une lettre");
        }
    }
    public void readInputDef(string mot)//read input mode de jeu 1
    {
        if(mot.Length > 1)
        {
            choixMot();
            if(mot == motChoisiString)//si input = mot choisi alors win 
            {
                isWin = true;
                point += 10;
                winLoose();
                IHM.DisableCanvas();
            }
            else if(mauvaisMot.Contains(mot))//si input deja present dans list de mot deja tapé alors erreur
            {
                IHM.error("mot déjà tapé");
            }
            else//sinon mauvais mot donc essais en moins 
            {
                essaisRestants --;
                mauvaisMot.Add(mot + ",");
                IHM.triedWords();
                winLoose();
                IHM.DisableCanvas();
                IHM.showBomb();
            }
        }
    }
    public bool checkLettre(string lettreChoisie)//pour mode de jeu 0 check si input est bien une lettre 
    {
        if(lettreChoisie.Length == 1)
        {
            bool result = Char.IsLetter(lettreChoisie,0);
            return result;
        }
        else
        {
            bool result = false;
            return result;
        }
    }
    public void checkIfWin()
    {
        int index1 = motChoisiString.IndexOf(lettreChoisie);

        for(int i = 0; i <= motChoisiString.Length; i++)//check si le mot choisi contient l'input
        {
            index1 = motChoisiString.IndexOf(lettreChoisie, i);

            if(index1 > -1)//ajoute la lettre trouvé a la bonne position dans un array
            {
                foundletters[index1] = lettreChoisie;
            }
        }

        motComplet = string.Concat(foundletters);//Concat de l'array des lettre trouvé

        badLetter = string.Concat(mauvaisesLettres);//Concat de l'array des lettres tapées mais pas dans le mot choisi
        
        if(motComplet == motChoisiString)
        {
            isWin = true;
        }
        checkAccents();
        IHM.DisableCanvas();
    }
    public void boucleJeu()
    {           
        checkIfWin();
        IHM.showletters();
        Debug.Log(defChoisiString);//debug 
        Debug.Log(motChoisiString);//debug 

        if (essaisRestants > 0 && !isWin)
        {                    
            if (motChoisiString.Contains(lettreChoisie))//ajout de point en cas de lettre trouvé 
            { 
                point ++;                                         
            } 
            else if (!motChoisiString.Contains(lettreChoisie) && !mauvaisesLettres.Contains(lettreChoisie))//perte de point en cas de lettre pas presente dans le mot 
            {  
                mauvaisesLettres.Add(lettreChoisie + ",");//ajoute mauvaise lettres dans une liste
                IHM.badLetters();
                essaisRestants--;
                point --; 
                IHM.showBomb();
            }   
            clacScore();//calcule le score du joueur
            IHM.infoScore();//affiche le score 
        } 
        winLoose();          
    }
    public void winLoose()
    {
        if (essaisRestants == 0)//partie perdu 0 essais restant 
        {
            Debug.Log("perdu");
            iF.GetComponent<GameObject>();
            iF.SetActive(false); 
            IHM.endGame();
            point --;
            IHM.infoScore();
            IHM.fireWorksStystem();
            IHM.DisableCanvas();
        }
        else if (isWin)//partie gagné isWin = true 
        {
            Debug.Log("gagné");
            iF.GetComponent<GameObject>();
            iF.SetActive(false);
            IHM.endGame();
            point ++;
            IHM.infoScore();
            IHM.fireWorksStystem();
            IHM.DisableCanvas();
        }   
        IHM.showBomb();
    }
    public void restart()//restart les variable en fonction du mode de jeu choisi
    {
        if(modeJeu == 0)//mode de jeu du pendu 
        {
            mauvaisesLettres.Clear();
            essaisRestants = 7;
            isWin = false;
            badLetter = "";
            lettreChoisie = "";
            motComplet = "";
            motChoisiString = "";
            defChoisiString = "";
            synChoisiString = "";
            IHM.infoDef.text = "";
            IHM.infoSyn.text = "";
            IHM.DisableCanvas();
            iF.SetActive(true);
            IHM.fireWorksStystem();
            IHM.badLetters();
            IHM.showBomb();
            IHM.shopItems();
            Start();
            net.newGame();
        }
        if(modeJeu == 1)//mode de jeu definition
        {
            mauvaisMot.Clear();
            essaisRestants = 7;
            isWin = false;
            badWords = "";
            motChoisiString = "";
            defChoisiString = "";
            synChoisiString = "";
            IHM.infoDef.text = "";
            IHM.infoSyn.text = "";
            IHM.DisableCanvas();
            iF.SetActive(true);
            IHM.fireWorksStystem();
            IHM.badLetters();
            IHM.showBomb();
            IHM.shopItems();
            Start();
            net.newGame();
        }
    }
    public void quit()//close Unity
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
    public void shopItems(int numero)//systeme de shop in game 
    {
        
        if(numero == 0)//numero 0 achat d'une lettre aléatoire présente dans le mot 
        {
            bool find = true;
            int buyLetter;
            char truc = 'a';

            while(find)
            {
                System.Random rnd = new System.Random();
                buyLetter = rnd.Next(0, 26);//cherche une lettre aléatoire
                char let = (char)('a' + buyLetter);
                truc = let;

                if(motChoisiString.Contains(truc) && !motComplet.Contains(truc))//continue de chercher une lettre jusqu'a en trouver une presente dans le mot choisi
                {
                    find = false;
                    break;
                }
            }
            if(point >= 10)//retire 10 point au joueur et affiche une lettre aléatoire 
            {
                lettreChoisie = truc.ToString();
                checkIfWin();
                IHM.showletters();
                point -= 10;
                clacScore();
                IHM.infoScore();
                IHM.endGame();
                IHM.def();
                IHM.showBomb();
            }
        }
        if(numero == 1)//numero 1 achat d'un synonyme (charge en debut de partie)
        {
            if(point >= 15)
            {
                point -= 15;
                clacScore();
                IHM.infoScore();
                IHM.showSyn();
            }
        }
        if(numero == 2)//numero 2 achat d'une definition (charge en debut de partie)
        {
            if(modeJeu == 0)
            {
                if(point >= 20)
                {
                    IHM.showDef();
                    point -= 20;
                    clacScore();
                    IHM.infoScore();
                }
            }
        }
    }
    public void clacScore()//empeche le score d'etre a moins de 0
    {
        score = point;

        if(score < 0)
        {
            score = 0;
        }
    }
    public void readInput2(string name)//input field de debutr de partie pour l'username
    {
        playerName = name;
    }
    public void checkAccents()//si input = e,a,i,o,u check si le mot contient des lettres avec accent
    {
        if(lettreChoisie.Contains("e"))//check pour accents sur e
        {   
            for(int a = 0; a <= 2; a++)
            {
                for(int i = 0; i <= motChoisiString.Length; i++)
                {
                    int index = motChoisiString.IndexOf(accents[a], i);

                    if(index > -1)
                    {
                        foundletters[index] = accents[a];
                        motComplet = string.Concat(foundletters);
                        IHM.showletters();
                        IHM.infomot();
                        if(!motChoisiString.Contains("e"))
                        {
                            essaisRestants ++;
                            point ++; 
                            IHM.showBomb();
                        }
                    }
                }
            }
        }
        if(lettreChoisie.Contains("a"))//check pour accents sur a
        {
            for(int a = 3; a <= 4; a++)
            {
                for(int i = 0; i <= motChoisiString.Length; i++)
                {
                    int index = motChoisiString.IndexOf(accents[a], i);

                    if(index > -1)
                    {
                        foundletters[index] = accents[a];
                        motComplet = string.Concat(foundletters);
                        IHM.showletters();
                        IHM.infomot();
                        if(!motChoisiString.Contains("a"))
                        {
                            essaisRestants ++;
                            point ++; 
                            IHM.showBomb();
                        }
                    }
                }
            }
        }
        if(lettreChoisie.Contains("i"))//check pour accents sur i
        {
            for(int i = 0; i <= motChoisiString.Length; i++)
            {
                int index = motChoisiString.IndexOf(accents[5], i);

                if(index > -1)
                {
                    foundletters[index] = accents[5];
                    motComplet = string.Concat(foundletters);
                    IHM.showletters();
                    IHM.infomot();
                    if(!motChoisiString.Contains("i"))
                    {
                        essaisRestants ++;
                        point ++; 
                        IHM.showBomb();
                    }
                }
            }
        }
        if(lettreChoisie.Contains("o"))//check pour accents sur o
        {
            for(int a = 6; a <= 7; a++)
            {
                for(int i = 0; i <= motChoisiString.Length; i++)
                {
                    int index = motChoisiString.IndexOf(accents[a], i);

                    if(index > -1)
                    {
                        foundletters[index] = accents[a];
                        motComplet = string.Concat(foundletters);
                        IHM.showletters();
                        IHM.infomot();
                        if(!motChoisiString.Contains("o"))
                        {
                            essaisRestants ++;
                            point ++; 
                            IHM.showBomb();
                        }
                    }
                }
            }
        }
        if(lettreChoisie.Contains("u"))//check pour accents sur u
        {
            for(int i = 0; i <= motChoisiString.Length; i++)
            {
                int index = motChoisiString.IndexOf(accents[8], i);

                if(index > -1)
                {
                    foundletters[index] = accents[8];
                    motComplet = string.Concat(foundletters);
                    IHM.showletters();
                    IHM.infomot();
                    if(!motChoisiString.Contains("u"))
                    {
                        essaisRestants ++;
                        point ++; 
                        IHM.showBomb();
                    }
                }
            }
        }
    }
}