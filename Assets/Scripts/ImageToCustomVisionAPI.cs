using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.WebCam;
using System.Linq;

public class ImageToCustomVisionAPI : MonoBehaviour, IInputClickHandler {

    public GameObject ImageFrameobject;
    PhotoCapture photoCaptureObject = null;
    public GameObject parent;

	// Use this for initialization
	void Start () {
        InputManager.Instance.PushFallbackInputHandler(gameObject);		
	}

    void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        photoCaptureObject = captureObject;
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.JPEG;

        captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
        }else
        {

        }
    }

    private void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            List<byte> imageBufferList = new List<byte>();
            photoCaptureFrame.CopyRawImageDataIntoBuffer(imageBufferList);

            DisplayImage(imageBufferList.ToArray());
        }
    }

    private void DisplayImage(byte[] imageData)
    {
        Texture2D imageTxtr = new Texture2D(2, 2);
        imageTxtr.LoadImage(imageData);

        GameObject tmp = Instantiate(ImageFrameobject);
        tmp.GetComponent<Renderer>().material.mainTexture = imageTxtr;

        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (GazeManager.Instance.HitObject.tag != "Interaction")
        {
            PhotoCapture.CreateAsync(true, OnPhotoCaptureCreated);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
