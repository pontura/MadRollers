using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class WebCamPhotoCamera : MonoBehaviour
{

  //  Texture2D lastPhotoTexture;
  //  WebCamTexture webCamTexture;

    public MeshRenderer rawImage;

    private bool photoTaken;
    private bool ready;

    void Start()
    {
//		if (WebCamTexture.devices.Length == 0 || Data.Instance.webcamOff) {
//			Data.Instance.LoadLevel("MainMenu");
//			return;
//		}
//        lastPhotoTexture = null;
//        webCamTexture = new WebCamTexture(WebCamTexture.devices[Data.Instance.WebcamID].name, 800, 600, 12);
//
//        print("CAntidad de camaras: " + WebCamTexture.devices.Length);
//
//        if (webCamTexture.isPlaying)
//            webCamTexture.Stop();
//        else
//            webCamTexture.Play();
//
//        Vector3 scale = rawImage.transform.localScale;
//
//        rawImage.transform.localScale = scale;
//        rawImage.material.mainTexture = webCamTexture;
    }
//    void Update()
//    {
//
//    }
    void Ready()
    {
        ready = true;
    }
    void OnDestroy()
    {
//		if(webCamTexture != null)
//       		 webCamTexture.Stop();
    }
    public void TakePhoto(int score)
    {
//		if (WebCamTexture.devices.Length == 0) {
//			Data.Instance.LoadLevel("MainMenu");
//			return;
//		}
//        photoTaken = true;
//        lastPhotoTexture = new Texture2D(webCamTexture.width, webCamTexture.height);
//        lastPhotoTexture.SetPixels(webCamTexture.GetPixels());
//        lastPhotoTexture.Apply();
//        webCamTexture.Stop();
//        Data.Instance.GetComponent<PhotosManager>().SavePhoto(lastPhotoTexture, score);
//        lastPhotoTexture = null;
    }
}