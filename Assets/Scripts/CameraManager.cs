using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine.UI;
using System.IO;

public class CameraManager : MonoBehaviour,IInputClickHandler {

    public GameObject ImageCube;

    int camWidth_px = 1280;
    int camHeight_px = 720;
    WebCamTexture webCamTexture;

    string filePath = "";

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (webCamTexture.isPlaying)
        {
            SavePhoto();
        }else
        {
            webCamTexture.Play();
        }
    }

    // Use this for initialization
    void Start () {
        InputManager.Instance.AddGlobalListener(gameObject);
        var devices = WebCamTexture.devices;

        if (devices.Length > 0)
        {
            webCamTexture = new WebCamTexture(camWidth_px, camHeight_px);

        }else
        {
            return;
        }
	}

    public void SavePhoto()
    {
        webCamTexture.Play();
        string filename = string.Format(@"CaptureImage{0}_n.jpg", Time.time);
        filePath = Path.Combine(Application.persistentDataPath, filename);

        Texture2D snap = new Texture2D(camWidth_px, camHeight_px);
        snap.SetPixels(webCamTexture.GetPixels());
        snap.Apply();

        var bytes = snap.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);

        ImageCube.GetComponent<Renderer>().material.mainTexture = snap;

        webCamTexture.Stop();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
