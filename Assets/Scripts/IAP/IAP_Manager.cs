using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

[System.Serializable]
public class Products
{
    public string storeKey;
    public string price;        
}




public class IAP_Manager :Singleton<IAP_Manager>, IStoreListener
{
    public Products product;
    private IStoreController controller;
    private bool isInit, btnSet, buyRemoveAds, buyMoreCoins, buyTheme;
    private IExtensionProvider extensions;

    /*public Text debugText;*/
    

    public void OnInitializeFailed(InitializationFailureReason error)
    {
#if UNITY_EDITOR
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
#endif
        isInit = false;        
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
#if UNITY_EDITOR
        Debug.Log("OnPurchaseReward: Success. Product:" + e.purchasedProduct);
#endif
        OnReward(e.purchasedProduct.definition.id);
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
#if UNITY_EDITOR
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
            i.definition.storeSpecificId, p));
#endif
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
#if UNITY_EDITOR
        Debug.Log("OnInitialized: pass");
#endif
        this.controller = controller;
        this.extensions = extensions;
        isInit = true;            
    }
    private void Init()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());        
        builder.AddProduct(product.storeKey,
                ProductType.Consumable, new IDs()
                {
                    {product.storeKey, GooglePlay.Name},
                    {product.storeKey, AppleAppStore.Name}
                });
        UnityPurchasing.Initialize(this, builder);

    }

    private void OnReward(string id)
    {
        if (!product.storeKey.Equals(id))
        {
            return;
        }        
        // remove ads
        PlayerPrefs.SetInt("BuyRemoveAds", 1);
    }

    private void Start()
    {
        Init();        
    }    
    public void Buy()
    {
        if (product.storeKey == null)
            return;                
        try
        {
            controller.InitiatePurchase(product.storeKey);
        }
        catch (Exception e)
        {
            /*debugText.text = "IPA === logIAP :::" + e;*/
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        throw new NotImplementedException();
    }
}
