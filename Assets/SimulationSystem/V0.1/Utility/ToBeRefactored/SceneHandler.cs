using System.Collections;
using SimulationSystem.V0._1.Utility.Miscellanous;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SimulationSystem.V0._1.Utility.ToBeRefactored
{
    public class SceneHandler : MonoBehaviour
    {
        public static SceneHandler Instance;
        // public RenderTexture RenderTexCam;
        public UnityEvent OnStereoScene;
        public UnityEvent OnHomeScene;
        public UnityEvent OnPodcastScene;
        public Camera mainCamera;
        bool sceneChanged;
        private bool isSceneChanging;
        private void Awake() 
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
            DontDestroyOnLoad(this);
            OnStereoScene = new UnityEvent();
            OnHomeScene = new UnityEvent();
        
        }

        public void Start()
        {
            SceneManager.sceneLoaded += ChangedActiveScene;
        }

        public IEnumerator ReloadScene(float time)
        {
            yield return new WaitForSeconds(time);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        private void ChangedActiveScene(Scene current, LoadSceneMode mode)
        {
            sceneChanged = true;
            string currentName = current.name;
            if (currentName == "StereoScene")
            {
                OnStereoScene?.Invoke();
            }

            if (currentName == "HomeScene")
            {
                OnHomeScene?.Invoke();
            }
            if (currentName == "PodcastScene")
            {
                OnPodcastScene?.Invoke();
            }
        }

        private void Update() 
        {   
            if(Input.GetKeyDown(KeyCode.H))
            {
                ChangeScene("HospitalScene");
            }
            if(Input.GetKeyDown(KeyCode.M))
            {
                ChangeScene("HomeScene");
            }
        }

        public void ChangeScene(string scene)
        {   
            if(isSceneChanging == false)
                StartCoroutine(FadeSceneChange(scene));
        }


        private IEnumerator FadeSceneChange(string scene)
        {
            isSceneChanging = true;
            ScreenFade fade;
            if (Camera.main != null)
                fade = Camera.main.GetComponent<ScreenFade>();
            else
                fade = mainCamera.GetComponent<ScreenFade>();    
            if(fade)
            {
                fade.fadeTime = 0.5f;
                fade.FadeOut();
            }
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadSceneAsync(scene);
            yield return new WaitUntil(() => sceneChanged == true);
        
            if(scene == "HomeScene") GameObject.Find("LoginCanvas").SetActive(false);
            sceneChanged = false;

            if(fade)
            {
                fade.fadeTime = 0.5f;
                fade.FadeIn();
            }

            isSceneChanging = false;
        }
    }
}
