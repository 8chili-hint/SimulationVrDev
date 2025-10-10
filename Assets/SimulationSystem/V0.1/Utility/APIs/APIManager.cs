using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
namespace SimulationSystem.V0._1.Utility.APIs
{
    public class APIManager : MonoBehaviour
    {
        public static APIManager Instance;
        public bool testMode;

        public enum Environment
        {
            Staging,
            Production
        };
        public Environment environment;
        public string pin;
        public string simulationID;
        public string _authorizationToken;

        private string _stagingURL = "https://staging.8chili.com/api/v1.0";
        private string _productionURL = "https://hint.8chili.com/api/v1.0";
    
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(this.gameObject);
            
            
            if (environment == Environment.Staging)
                ApiUrlManager.baseURL = _stagingURL;
            else if (environment == Environment.Production)
                ApiUrlManager.baseURL = _productionURL;
            
            if(testMode)
                StartCoroutine(GETRequest(ApiUrlManager.LoginUrl(pin), SetAuthToken, false));
            else
                _authorizationToken = GetAuthTokenPlayerPref();
        }

        private string GetAuthTokenPlayerPref()
        {
            string token = "";
            if(PlayerPrefs.HasKey("AuthToken"))
                token = PlayerPrefs.GetString("AuthToken");
            return token;
        }

        private void SetAuthToken(bool success, string response, long code)
        {
            Assessment.AuthTokenResponse a = JsonUtility.FromJson<Assessment.AuthTokenResponse>(response );
           _authorizationToken = a.auth_token;
        }

        #region GETRequest

        public IEnumerator GETRequest(string uri, Action<bool,string,long> callbackOnFinish, bool isTokenNeeded)
        {


            var webRequest = new UnityWebRequest (uri, "GET");
        
            if(isTokenNeeded)
                webRequest.SetRequestHeader("Authorization", "Bearer " + _authorizationToken);
        
            DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
            webRequest.downloadHandler = dH;
            
            yield return webRequest.SendWebRequest();
        
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                callbackOnFinish?.Invoke(false, dH.text, webRequest.responseCode);
                Debug.Log("______ error in Get ___________");

            }
            else
            {

                callbackOnFinish?.Invoke(true, dH.text, webRequest.responseCode);
            }
        }

        #endregion      

        #region POSTRequest

        public IEnumerator POSTRequest(string POSTjson, string uri, Action<bool,string> callbackOnFinish)
        {
            var www = new UnityWebRequest (uri, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(POSTjson);
            www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + _authorizationToken);
            yield return www.SendWebRequest();
            Debug.Log(POSTjson +" url " + uri);
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log("error " + www.error + " " + www.downloadHandler.text );
                callbackOnFinish?.Invoke(false, www.downloadHandler.text);
            }
            else
            {
                callbackOnFinish?.Invoke(true, www.downloadHandler.text);

            }
            www.Dispose();
        }

        #endregion
    }
}