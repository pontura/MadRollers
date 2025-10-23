using UnityEngine;
using UnityEngine.Purchasing;

public class LegacyIAPManager : MonoBehaviour, IStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

    private const string PRODUCT_FREE = "free"; // ID exacto del producto en Play Console

    void Start()
    {
        if (storeController == null)
            InitializePurchasing();
        Events.BuyIAP += BuyIAP;
        DontDestroyOnLoad(this.gameObject);
    }
    void OnDestroy()
    {
        Events.BuyIAP -= BuyIAP;
    }
    System.Action<bool> OnDone;
    void BuyIAP(System.Action<bool> OnDone)
    {
        this.OnDone = OnDone;
        BuyFree();
    }
    public void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(PRODUCT_FREE, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyFree()
    {
        print("IAP Buy Free " + storeController);
        if (storeController != null)
            storeController.InitiatePurchase(PRODUCT_FREE);
        else
            Debug.LogError("IAP no inicializado todavía.");
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
        Debug.Log("IAP Inicializado correctamente.");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnDone(false);
        Debug.LogError("IAP FALLÓ AL INICIALIZAR: " + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (args.purchasedProduct.definition.id == PRODUCT_FREE)
        {
            Debug.Log("Compra exitosa de FREE.");
            PlayerPrefs.SetInt("free_unlocked", 1);
        }
        OnDone(true);
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        OnDone(false);
        Debug.LogError("COMPRA FALLÓ: " + failureReason);
    }

    public bool IsFreeUnlocked()
    {
        return PlayerPrefs.GetInt("free_unlocked", 0) == 1;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message = null)
    {
        throw new System.NotImplementedException();
    }
}
