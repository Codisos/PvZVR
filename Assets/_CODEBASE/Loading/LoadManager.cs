using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadManager : MonoBehaviour
{
    [Header("Settings")]
    //public variables
    [SerializeField] private bool useLoadingScene = true;

    [Tooltip("Can have different settings for normal,slower,corupted, etc. load scene")]
    [SerializeField] LoaderSettings loadSettings;

    //Instance
    public static LoadManager Instance; //POUZIVAT/VOLAT METODY POUZE PRES TUHLE INSTANCI!

    //EVENTS
    [Header("Events")]
    [Tooltip("Which event to raise when loading is ended")]
    public GameEvent loadEnded;

    [Tooltip("Which event to raise when loading started")]
    public GameEvent loadStarted;

    //CONTROL VARIABLES
    
    string sceneToLoad;

    //set by loader settings
    private FloatReference forcedWaitTime;
    private FloatReference secondsToFadeCamera;

    private void Awake()        //rozhodnuti jestli uz je ve scene load manager, pozdeji pridat do general scene manageru i vytvoreni load manager objektu pokud neni ve scene
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevel(string sceneName)     //public method for begining the load, has to have scene name, is called from the load manager reference
    {
        loadStarted.Raise();
        sceneToLoad = sceneName;
        StartCoroutine(WaitForFade());
    }

    private void LevelIsLoaded(Scene scene, LoadSceneMode mode)
    {
        loadEnded.Raise();
    }


    IEnumerator WaitForFade()
    {    
        yield return new WaitForSeconds(loadSettings.secondsToFadeCamera.Value);
        StartLoadingSequence();
    }

    IEnumerator WaitForFadeFromLoad(AsyncOperation s)
    {
        loadStarted.Raise();
        yield return new WaitForSeconds(loadSettings.secondsToFadeCamera.Value);

        s.allowSceneActivation = true;      //pak pokud je scena nactena ji aktivovat

        SceneManager.sceneLoaded += LevelIsLoaded;
    }

    private async void StartLoadingSequence()
    {
        if (useLoadingScene)
        {
            //hned po fade se presunout do LOAD SCENE
            var scene = SceneManager.LoadSceneAsync(loadSettings.loadSceneName);

            //az bude scena nactena, posli event
            SceneManager.sceneLoaded += LevelIsLoaded;

            //zacit nacitani sceny v pozadi load sceny
            scene = SceneManager.LoadSceneAsync(sceneToLoad);
            scene.allowSceneActivation = false;     //vypnout aktivaci sceny hned po jejim nacteni

            await Task.Delay((int)loadSettings.forcedWaitTime.Value * 1000); //pockat minimalne pocet sekund v loadTime

            StartCoroutine(WaitForFadeFromLoad(scene));  //coroutine pro fade z load sceny, tam se pak dokonci load  
        }
        else
        {
            //zacit nacitani sceny v pozadi load sceny
            var scene = SceneManager.LoadSceneAsync(sceneToLoad);
            scene.allowSceneActivation = false;     //vypnout aktivaci sceny hned po jejim nacteni

            await Task.Delay((int)loadSettings.forcedWaitTime.Value * 1000); //pockat minimalne pocet sekund v loadTime
            scene.allowSceneActivation = true;      //pak pokud je scena nactena ji aktivovat

            SceneManager.sceneLoaded += LevelIsLoaded;
        }
        
    }

}
