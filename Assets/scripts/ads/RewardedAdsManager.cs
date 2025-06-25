using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
    [SerializeField] string androidAdUnitId = "Rewarded_Android";
    [SerializeField] string iosAdUnitId = "Rewarded_iOS";
    [SerializeField] bool testMode = true;
    string adUnitId;
    string gameID; 

    void Start()
    {
#if UNITY_IOS
        adUnitId = iosAdUnitId;
        gameID = "5885018";
#else
        adUnitId = androidAdUnitId;
        gameID = "5885019";
#endif
        Advertisement.Initialize(gameID, testMode, this);
        Advertisement.Load(adUnitId, this);
    }
    System.Action<bool> OnAdReady;
    public void ShowAd(System.Action<bool> OnAdReady)
    {
        this.OnAdReady = OnAdReady;
        if (Advertisement.isInitialized)
        {
            Advertisement.Show(adUnitId, this);
        }
        else
        {
            Debug.LogWarning("Anuncio aún no está listo");
            OnAdReady(false);
        }
    }

    // ✅ Llamado cuando se carga correctamente el ad
    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == adUnitId)
        {
            Debug.Log("Anuncio cargado exitosamente.");
        }
    }

    // ✅ Llamado cuando el ad se muestra completamente
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Jugador vio el anuncio completo. Recompensa otorgada.");
            OnReward();
            Advertisement.Load(adUnitId, this); // Volvemos a cargar para la próxima vez
        }
    }

    void OnReward()
    {
        OnAdReady(true);
        Debug.Log("🎁 ¡Jugador recompensado!");
    }

    // ⚠️ Manejo de errores
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Error al cargar ad: {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Error al mostrar ad: {message}");
        OnAdReady(false);
    }

    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }

    public void OnInitializationComplete()
    {
       // throw new System.NotImplementedException();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
      //  throw new System.NotImplementedException();
    }
}