using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    public RawImage rawImage;

    public Image previewImage;

    public AspectRatioFitter fit;

    private WebCamTexture webcamTexture;
    private Sprite lastScreenshot;
    private WebCamDevice[] devices;
    public bool activated;
    public Text txt;

    public GameObject[] GOs;
    public GameObject OK;
    public GameObject HairContainer;

    // Use this for initialization
    void Start () {

        Activate();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Activate();
        }

        if (activated!=false)
        {
            float ratio = (float)webcamTexture.width / webcamTexture.height;
            fit.aspectRatio = ratio;

            float scaleY = webcamTexture.videoVerticallyMirrored ? 1f : -1f;
            rawImage.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

            int orient = -webcamTexture.videoRotationAngle;
            rawImage.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
        }
    }

    public void Activate()
    {
        Application.targetFrameRate = 60;
        webcamTexture = new WebCamTexture();
        devices = WebCamTexture.devices;
        webcamTexture.deviceName = devices[1].name;

        Renderer renderer = GetComponent<Renderer>();
        Texture cameraTexture = webcamTexture;
        rawImage.texture = cameraTexture;
        webcamTexture.Play();
        activated = true;

        Debug.Log(devices[0].name);
    }

    public void Capture(GameObject GO)
    {
        StartCoroutine(SavePicture(GO));
    }

    IEnumerator SavePicture(GameObject GO)
    {
        string path = Application.persistentDataPath + "/Capture.png";
        GO.SetActive(false);
        //ScreenCapture.CaptureScreenshot(path);
        StartCoroutine(TakeScreenshotAndSave());
        yield return new WaitForSeconds(1);
        //lastScreenshot = LoadSprite(path);
        PickImage(512);
        previewImage.gameObject.SetActive(true);

        GO.SetActive(true);
        GO.transform.GetChild(0).gameObject.SetActive(false);
        GO.transform.GetChild(1).gameObject.SetActive(true);
        HairContainer.SetActive(true);
    }


    private IEnumerator TakeScreenshotAndSave()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // Save the screenshot to Gallery/Photos
        Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, "Hair Changer", "My img.png")); //{0}.png"));

        // To avoid memory leaks
        Destroy(ss);
    }

    private IEnumerator SaveT()
    {
        for (int i = 0; i < GOs.Length; i++)
        {
            GOs[i].SetActive(false);
        }

        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        // Save the screenshot to Gallery/Photos
        Debug.Log("Permission result: " + NativeGallery.SaveImageToGallery(ss, "Hair Changer", "Hair Changer {0}.png")); 

        // To avoid memory leaks
        Destroy(ss);

        yield return new WaitForSeconds(1);
        OK.SetActive(true);
        yield return new WaitForSeconds(2);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    private void PickImage(int maxSize)
    {
        //NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        //{
        string path = "/storage/emulated/0/Hair Changer/My img.png";
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);

                    return;
                }

                // Assign texture to a temporary quad and destroy it after 5 seconds
                previewImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f); ;
            }
        //}, "Select a PNG image", "image/png", maxSize);

        //Debug.Log("Permission result: " + permission);
    }

    public void SaveToGallery()
    {
        StartCoroutine(SaveT());
    }
}
