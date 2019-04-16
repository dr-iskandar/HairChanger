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

            float scaleY = webcamTexture.videoVerticallyMirrored ? -1f : 1f;
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
        ScreenCapture.CaptureScreenshot(path);
        yield return new WaitForSeconds(1);
        lastScreenshot = LoadSprite(path);
        previewImage.gameObject.SetActive(true);
        previewImage.sprite = lastScreenshot;
        GO.SetActive(true);
        GO.transform.GetChild(0).gameObject.SetActive(false);
        GO.transform.GetChild(1).gameObject.SetActive(true);
    }

    Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;

    }
}
