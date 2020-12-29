using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechRecognitionEngine : MonoBehaviour
{
    //public string[] keywords = new string[] { };//"up", "down", "left", "right", "hello", "who are you" };
    public ConfidenceLevel confidence = ConfidenceLevel.Low;
    public float speed = 1;

    public Text results;
    public GameObject mySelectedObject;
    private Material cubeMaterial;

    protected GrammarRecognizer grammarRecognizer;
    //protected PhraseRecognizer keywordRecognizer;
    //Text that has been recognised by the speechrecognizer -> Stored into 'word' string 
    protected string word = "";
    //Assign the gameobject cube (in the scene) to the variable gameobject cube (in code) -> Through gameobject.find
    //Can
    private void Start()
    {
        //*********************************************
        //if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        //{
        //    Debug.Log("keywordRecognizer already exists");
        //    keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
        //    keywordRecognizer.Stop();
        //    keywordRecognizer.Dispose();
        //}
        //if (keywords != null)
        //{
        //    keywordRecognizer = new KeywordRecognizer(keywords, confidence);
        //    keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        //    keywordRecognizer.Start();
        //}
        //*********************************************
        //41 OnError is a trigger -> Attaching the phrase recognise error function to the OnError Event -> Prints off the error code
        PhraseRecognitionSystem.OnError += PhraseRecognitionSystem_OnError;
        //43 Checks if there is not none and its running, a bit over zealous, -> /49 Memory management tech -> At that time, its disposed of (gone) 
        if (grammarRecognizer != null && grammarRecognizer.IsRunning)
        {
            //47 "-=" detaches the function from the 'OnPhraseRecognized'
            Debug.Log("grammarRecognizer already exists");
            grammarRecognizer.OnPhraseRecognized -= GrammarRecognizer_OnPhraseRecognized;
            grammarRecognizer.Stop();
            grammarRecognizer.Dispose();
        }
        //Don't need to use Application.StreamingAssets 
        //Can use whole path dir
        //'@' Makes the file path backslashes not commands but instead characters
        grammarRecognizer = new GrammarRecognizer(@"C:\Users\Danin\Documents\Unity\BrickLayers\Assets\StreamingAssests\SRGS\GrammarBasic.xml", confidence);
        grammarRecognizer.OnPhraseRecognized += GrammarRecognizer_OnPhraseRecognized;
        //53 Creates /4 Assigning a function name (GrammarRecognizer)
        //Function is called from the event "+=" means it attaches the list of functions to the event 
        //59 Object start meaning the game is activly listening to the player /48/9 is for stopping the listening as we don't want to listen to the player forever
        grammarRecognizer.Start();

        if (grammarRecognizer.IsRunning)
            Debug.Log("Start - grammarRecognizer is running from file: " + grammarRecognizer.GrammarFilePath);
    }

    private void PhraseRecognitionSystem_OnError(SpeechError errorCode)
    {
        Debug.Log("errorCode =" + errorCode.ToString());
    }

    //private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    //{
    //    word = args.text;
    //    results.text = "You said: <b>" + word + "</b>";
    //}
    //77 Once the speech has been recognized -> Sent to the function as args -> multiple values. Args = Argumants /80 Assigning text recognized to Debug.log
    private void GrammarRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        SemanticMeaning[] meanings = args.semanticMeanings;

        Debug.Log("GrammarRecognizer_OnPhraseRecognized: " + word);
        Debug.Log("GrammarRecognizer_OnPhraseRecognized - meanings: " + meanings.Length);

        results.text = "You said: <b>" + word + "</b>";
    }
    //90 'smart' switch statement "Single Keywords" + Grammar Recognizer -> alternate way to creating many if statements
    //Mapping 2 things -> the word recognized, mapping between that string and something to be done /93 Checking whats been recognized 
    //94-6 Can have multiple lines of commands here /109-10 'DEFAULT' If none of the cases aren't equal to the word, do nothing or something in case -> A good way of giving feedback to user (Debug.log)
    private void Update()
    {
        switch (word)
        {
            case "cube":
                mySelectedObject = GameObject.Find("Cube");
                cubeMaterial = mySelectedObject.GetComponent<Renderer>().material;
                break;
            case "sphere":
                mySelectedObject = GameObject.Find("Sphere");
                cubeMaterial = mySelectedObject.GetComponent<Renderer>().material;
                break;
            case "blue":
                cubeMaterial.color = Color.blue;
                break;
            case "red":
                cubeMaterial.color = Color.red;
                break;
            case "green":
                cubeMaterial.color = Color.green;
                break;
            case "yellow":
                cubeMaterial.color = Color.yellow;
                break;
            case "white":
                cubeMaterial.color = Color.white;
                break;
            default:
                break;
        }
    }
    //Get the user to say the object first -> that will be recognized as a single keyword
    //Then recognize the colour -> do it seperately -> Select through code which gameObject you'd like to change the colour of (An array is not required)
    //gameobject.find -> 
    private void OnApplicationQuit()
    {
        //*********************************************
        //if (keywordRecognizer != null && keywordRecognizer.IsRunning)
        //{
        //    keywordRecognizer.OnPhraseRecognized -= KeywordRecognizer_OnPhraseRecognized;
        //    keywordRecognizer.Stop();
        //}
        //*********************************************

        if (grammarRecognizer != null && grammarRecognizer.IsRunning)
        {
            grammarRecognizer.OnPhraseRecognized -= GrammarRecognizer_OnPhraseRecognized;
            grammarRecognizer.Stop();
        }
    }
}
