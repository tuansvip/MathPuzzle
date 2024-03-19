using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//using ACEPlayMobileAds;

namespace ACEPlay.Bridge
{
    public enum AdsEvent
    {
        RewardLoad,
        RewardClick,
        RewardShow,
        RewardFail,
        RewardComplete,
        InterFail,
        InterImpression,
        InterLoad,
        InterShow,
        InterClick,
        InterAdmobFail,
        InterAdmobLoad,
        InterAdmobShow,
        InterAdmobClick,
        AoaLoad,
        AoaFail,
        AoaShow
    }
    public class BridgeController : MonoBehaviour
    {
        public static BridgeController instance;
        public System.Action ACTION_SHOW_NATIVE;
        public System.Action ACTION_HIDE_NATIVE;
        public System.Action<int> ACTION_LOAD_NATIVE;
        public string DataConfigGame;

        #region Controll Game
        public bool IsVipComplete //true: VIP, false: not VIP
        {
            get { return PlayerPrefs.GetInt("IsVip", 0) == 1; }
            set { PlayerPrefs.SetInt("IsVip", value ? 1 : 0); }
        }

        public bool CanShowAds //true: show ads, false: not show ads
        {
            get { return PlayerPrefs.GetInt("CanShowAds", 1) == 1; }
            set { PlayerPrefs.SetInt("CanShowAds", value ? 1 : 0); }
        }
        public bool CanShowAdsWithVip //true: show ads VIP, false: not show ads VIP
        {
            get { return PlayerPrefs.GetInt("CanShowAdsWithVip", 1) == 1; }
            set { PlayerPrefs.SetInt("CanShowAdsWithVip", value ? 1 : 0); }
        }
        public bool CanShowAdsIngame //true: show ads ingame, false: not show ads ingame
        {
            get { return PlayerPrefs.GetInt("CanShowAdsIngame", 0) == 1; }
            set { PlayerPrefs.SetInt("CanShowAdsIngame", value ? 1 : 0); }
        }
        public bool IsFreeContinue //true: free continue, false: not free
        {
            get { return PlayerPrefs.GetInt("IsFreeContinue", 0) == 1; }
            set { PlayerPrefs.SetInt("IsFreeContinue", value ? 1 : 0); }
        }

        public bool IsShowPackAtStart //true: show pack menu, false: not show pack menu
        {
            get { return PlayerPrefs.GetInt("IsShowPackAtStart", 0) == 1; }
            set { PlayerPrefs.SetInt("IsShowPackAtStart", value ? 1 : 0); }
        }
        public int RewardedLevelPlayToday // RewardedLevelPlayToday++ khi xem reward thanh cong
        {
            get { return PlayerPrefs.GetInt("RewardedLevelPlayToday", 0); }
            set { PlayerPrefs.SetInt("RewardedLevelPlayToday", value); }
        }
        public float TimeShowAds  // thời gian delay sau mỗi lần hiện quảng cáo inter
        {
            get { return PlayerPrefs.GetFloat("TimeShowAds", 20f); }
            set { PlayerPrefs.SetFloat("TimeShowAds", value); }
        }
        public float TimeShowAdsIngame // thời gian delay mỗi lần show quảng cáo ingame
        {
            get { return PlayerPrefs.GetFloat("TimeShowAdsIngame", 40f); }
            set { PlayerPrefs.SetFloat("TimeShowAdsIngame", value); }
        }
        public float TaichiThreshold // tROAS
        {
            get { return PlayerPrefs.GetFloat("taichi_threshold", 0.02f); }
            set { PlayerPrefs.SetFloat("taichi_threshold", value); }
        }
        public List<string> NonConsumableList = new List<string>();
        public List<string> VipPackageList = new List<string>();
        #endregion

        #region Device Test
        private bool _isDeviceTest = true;
        public void ChangeTestMode()
        {
            _isDeviceTest = !_isDeviceTest;
            if (_isDeviceTest)
            {
                if (TryGetComponent(out FPSDisplay Fps))
                {
                    Fps.enabled = true;
                }
                else
                {
                    gameObject.AddComponent<FPSDisplay>();
                }
            }
            else
            {
                if (TryGetComponent(out FPSDisplay Fps))
                {
                    Fps.enabled = false;
                }
            }
        }
        public bool isDeviceTest()
        {
#if UNITY_EDITOR
            return true;
#else

            return _isDeviceTest;
#endif
        }
        #endregion


        public bool isLoadGameSuccess = false;
        public bool isCheckFirebase;
        [HideInInspector]
        public bool IsBannerShowing;
        [HideInInspector]
        public bool IsMRECsShowing;
        [HideInInspector]
        public bool canShowBanner = true;
        public bool resumeFromAds;
        public int levelCountShowAds = 0;
        [HideInInspector]
        public float lastTimeShowAd = 0;
        [HideInInspector]
        public float lastTimeShowAdIngame = 0;
        [SerializeField] private string[] urls = new string[] { "https://www.google.com", "https://www.wikipedia.com", "https://www.amazon.com", "https://y.qq.com", "https://www.microsoft.com" };


        private void Awake()
        {
            //PlayerPrefs.DeleteAll();
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else Destroy(this.gameObject);
        }

        private IEnumerator Start()
        {
            lastTimeShowAd = Time.realtimeSinceStartup;
            yield return new WaitWhile(() => Input.touchCount == 0);
            int countTop = 0;
            foreach (Touch touch in Input.touches)
            {
                if ((touch.position.y > Screen.height / 2))
                {
                    countTop++;
                }
            }
            if (countTop != 3)
            {
                _isDeviceTest = false;
            }
            else
            {
                _isDeviceTest = false;
                ChangeTestMode();
            }
        }
        public string GetHostNameOrAddress(int index)
        {
            index = index % urls.Length;
            return urls[index].Replace("https://", "");
        }
        public bool isCheckNonConsumableList(string namePack)
        {
            return NonConsumableList.Contains(namePack);
        }

        public void AddNonConsumableLists(string productSku)
        {
            if (!NonConsumableList.Contains(productSku))
            {
                NonConsumableList.Add(productSku);
            }
        }
        public void AddVipPackageLists(string productSku)
        {
            if (!VipPackageList.Contains(productSku))
            {
                VipPackageList.Add(productSku);
            }
        }
        public bool HasInternet() // false: has internet, true: no internet
        {
            return Application.internetReachability == NetworkReachability.NotReachable;
        }
        #region ADS
        public float GetBannerHeightInPixels()
        {
            return 150f;
        }
        public bool IsShowingBanner()
        {
            return IsBannerShowing;
        }
        public void ShowBanner()
        {
            canShowBanner = true;
            Debug.Log("=====Banner Show success!=====");
        }
        public void HideBannerAD()
        {
            canShowBanner = false;
            Debug.Log("=====Banner Hide success!=====");
        }
        public void ShowMRECs()
        {

        }

        public void HideMRECs(bool activeBanner = true)
        {

        }
        public bool IsInterReady()
        {
            return true;
        }
        public bool ShowInterstitial(string placement, UnityEvent onClosed, bool skipCheckCappingTime = false)
        {
            Debug.Log("=====Interstitial Show success!=====");
            if (onClosed != null) onClosed.Invoke();
            return true;
        }
        public bool IsRewardReady()
        {
            return true;
        }
        public bool ShowRewarded(string placement, UnityEvent onRewarded, UnityEvent onFailed = null)
        {
            Debug.Log("=====Rewarded Show success!=====");
            if (onRewarded != null) onRewarded.Invoke();
            return true;

        }
        #endregion

        #region IAP
        public void PurchaseProduct(string productSku, UnityStringEvent onDonePurchaseEvent)
        {
            if (onDonePurchaseEvent != null) onDonePurchaseEvent.Invoke(productSku);
        }

        public void RestorePurchase()
        {
            Debug.Log("=====Restore Purchase success!=====");

        }
        #endregion

        #region Analytics

        public void LogEvent(string eventName)
        {

        }
        public void LogEvent(string eventName, string paramName, string paramValue)
        {

        }
        public void LogEvent(string eventName, Dictionary<string, object> parameters)
        {

        }
        public void LogAdsEvent(AdsEvent eventType, string placement, string errorMsg = "")
        {

        }
        public void SetUserProperty(string name, string value)
        {

        }
        /// <summary>
        /// Retention of user.
        /// </summary>
        /// <param name="value"></param> retend day. Ex: D0 => 0, D7 => 7
        public void SetPropertyRetendDay(string value)
        {
            SetUserProperty("retent_type", value);
        }
        /// <summary>
        /// Number of days the user has played, different from Retention.
        /// </summary>
        /// <param name="value"></param> day played. Ex: the user installs at D0 and plays after 7 days, the retention is D7 and days_played is 2
        public void SetPropertyDayPlayed(string value)
        {
            SetUserProperty("days_playing", value);
        }
        /// <summary>
        /// Level values ​​of events are logged afterwards, updated after level related events are logged.
        /// </summary>
        /// <param name="value"></param> the value is the maximum user level passed plus 1, (initial value when first installing the game and not passing any which level is 1). Ex: when pass level 1 => 2
        public void SetPropertyLevel(string value)
        {
            SetUserProperty("level_reach", value);
        }
        public void SetPropertyAppVersion()
        {
            SetUserProperty("app_version", Application.version);
        }

        public void SetPropertyCoinSpend(string value)
        {
            SetUserProperty("total_spent", value);
        }

        public void SetPropertyCoinEarn(string value)
        {
            SetUserProperty("total_earn", value);
        }
        /// <summary>
        /// Level start event(Log when start level)
        /// </summary>
        /// <param name="level"></param> level start
        /// <param name="currentGold"></param>  amount of money at the start of level
        public void LogLevelStartWithParameter(int level)
        {
            var firebase_evt = new System.Collections.Generic.Dictionary<string, object>
            {
                {"level", level }
            };
            LogEvent("start_level", firebase_evt);

            if (PlayerPrefs.GetInt($"playlevel_{level}", 0) == 0)
            {
                PlayerPrefs.SetInt($"playlevel_{level}", 1);
                if (level < 10)
                {
                    LogEvent("level_start_00" + level);
                }
                else if (level < 100)
                {
                    LogEvent("level_start_0" + level);
                }
                else
                {
                    LogEvent("level_start_" + level);
                }
            }
            else
            {
                if (level < 10)
                {
                    LogEvent("level_retry_00" + level);
                }
                else if (level < 100)
                {
                    LogEvent("level_retry_0" + level);
                }
                else
                {
                    LogEvent("level_retry_" + level);
                }
            }
        }
        /// <summary>
        /// Level Complete Event(Log when end level)
        /// </summary>
        /// <param name="level"></param> level complete
        /// <param name="timePlay"></param> time to complete level
        public void LogLevelCompleteWithParameter(int level)
        {
            var firebase_evt = new System.Collections.Generic.Dictionary<string, object>
            {
                {"level", level }
            };
            LogEvent("win_level", firebase_evt);

            if (PlayerPrefs.GetInt($"winlevel_{level}", 0) == 0)
            {
                PlayerPrefs.SetInt($"winlevel_{level}", 1);
                if (level < 10)
                {
                    LogEvent("level_complete_00" + level);
                }
                else if (level < 100)
                {
                    LogEvent("level_complete_0" + level);
                }
                else
                {
                    LogEvent("level_complete_" + level);
                }
            }
        }
        /// <summary>
        /// Level Fail Event(Log when end level)
        /// </summary>
        /// <param name="level"></param> level fail
        /// <param name="failCount"></param> amount fail on level
        public void LogLevelFailWithParameter(int level)
        {
            var firebase_evt = new System.Collections.Generic.Dictionary<string, object>
            {
                {"level", level }
            };
            LogEvent("lose_level", firebase_evt);
        }
        /// <summary>
        /// Spend Virtual Currency event.
        /// </summary>
        /// <param name="typeCurrency"></param> type currency of earn. Ex: gold, diamond,...
        /// <param name="amountSpend"></param> amount of spend
        /// <param name="itemName"></param> currency to buy something. Ex: skin1, weapon1,...
        public void LogSpendCurrency(string typeCurrency, int amountSpend, string itemName)
        {
            var firebase_evt = new System.Collections.Generic.Dictionary<string, object>
            {
                {"virtual_currency_name", typeCurrency },
                {"value", amountSpend},
                {"item_name", itemName}
            };
            LogEvent("spend_virtual_currency", firebase_evt);
        }
        /// <summary>
        /// Earn Virtual Currency event.
        /// </summary>
        /// <param name="typeCurrency"></param> type currency of earn. Ex: gold, diamond,...
        /// <param name="amountEarn"></param> amount of earn
        /// <param name="source"></param> source of earn. Ex: shop, win game, daily bonus,...
        public void LogEarnCurrency(string typeCurrency, int amountEarn, string source)
        {
            var firebase_evt = new System.Collections.Generic.Dictionary<string, object>
            {
                {"virtual_currency_name", typeCurrency },
                {"value", amountEarn},
                {"source", source}
            };
            LogEvent("earn_virtual_currency", firebase_evt);
        }
        #endregion

        #region Social
        public void RateGame(UnityEvent onRateRewarded = null)
        {
            Debug.Log("=====Rate Success=====");
            if (onRateRewarded != null) onRateRewarded.Invoke();
        }

        public void ShareGame(UnityEvent onShareRewarded)
        {
            Debug.Log("=====Share Success=====");
            if (onShareRewarded != null) onShareRewarded.Invoke();

        }
        public void PublishHighScore(int score)
        {
            Debug.Log("=====Publish Score:" + score + "Success=====");
        }
        //hien thi ban xep hang
        public void ShowLeaderBoard()
        {
            Debug.Log("=====Show Leaderboard success!=====");
        }

        public void ShowFacebook(UnityEvent onOpenFBRewarded)
        {
            Debug.Log("=====Show Facebook Success=====");
            if (onOpenFBRewarded != null) onOpenFBRewarded.Invoke();

        }

        public void ShowInstagram(UnityEvent onInstaRewarded)
        {
            Debug.Log("=====Show Instagram Success=====");
            if (onInstaRewarded != null) onInstaRewarded.Invoke();

        }

        public void SubcribeYoutube(UnityEvent onSubcribedYoutube)
        {
            Debug.Log("=====Subcribe Youtube Success=====");
            if (onSubcribedYoutube != null) onSubcribedYoutube.Invoke();
        }

        public void Moregames()
        {
            Debug.Log("=====Show Moregames success!=====");

        }
        public void ShowWebsite()
        {
            Debug.Log("=====Show Website success!=====");
        }
        public void InstallGame()
        {
            //link store game install
        }
        public bool CheckAppInstallation(string bundleId)
        {
#if UNITY_EDITOR
            return false;
#elif UNITY_ANDROID
            bool installed = false;
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = curActivity.Call<AndroidJavaObject>("getPackageManager");
            AndroidJavaObject launchIntent = null;
            try
            {
                launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
                if (launchIntent == null) installed = false;
                else installed = true;
            }
            catch (System.Exception e)
            {
                installed = false;
            }
            return installed;
#elif UNITY_IOS
            return false;
#endif
            return false;
        }
        #endregion


    }
}

[System.Serializable]
public class UnityStringEvent : UnityEvent<string>
{
}


