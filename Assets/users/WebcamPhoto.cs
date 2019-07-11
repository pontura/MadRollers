using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WebcamPhoto : MonoBehaviour
{

    public string deviceName;
    WebCamTexture webcam;
    WebCamDevice[] devices;
    AspectRatioFitter fit;
    RawImage image;
    bool camAvailable;
    Texture defaultBackground;

    void OnDisable()
    {
        OnUserRegisterCanceled();
    }
    void OnUserRegisterCanceled()
    {
        if(devices == null || devices.Length==0)
            return;
        
            StopCamera();
    }
    void StopCamera()
    {
        if (webcam == null)
            return;

        webcam.Stop();
        camAvailable = false;
    }
    public void InitWebcam(RawImage image, AspectRatioFitter fit)
    {
       
        devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            camAvailable = false;
            return;
        }
        this.image = image;
        this.fit = fit;

        defaultBackground = image.texture;
        
        for (int a = 0; a<devices.Length; a++)
        {
            if (devices[a].isFrontFacing)
                webcam = new WebCamTexture(devices[a].name, Screen.width, Screen.height);
        }
        if (webcam == null)
            return;

        webcam.Play();
        image.texture = webcam;

        camAvailable = true;

    }
    void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)webcam.width / (float)webcam.height;
        fit.aspectRatio = ratio;

        float scaleY = webcam.videoVerticallyMirrored ? -1 : 1f;
        image.rectTransform.localScale = new Vector3(1, scaleY, 1);
        int orient = -webcam.videoRotationAngle;
        image.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }
    public Camera cam;
    System.Action OnDone;
    public void TakeSnapshot(System.Action OnDone)
    {
        this.OnDone = OnDone;

#if UNITY_EDITOR
        string fileName = UserData.Instance.path + UserData.Instance.userID + ".png";
#else
        string fileName = UserData.Instance.userID + ".png";
#endif
        print("____________ GUARDA SCREENSHOT en : " + UserData.Instance.path + UserData.Instance.userID + ".png");

        ScreenCapture.CaptureScreenshot(fileName);
       
        Invoke("Delayed", 0.5f);
    }
    void Delayed()
    {
        UserData.Instance.sprite = null;
        StopCamera();
        UserData.Instance.LoopUntilPhotoIsLoaded(OnDone);
    }
}