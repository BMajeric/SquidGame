using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

public class Teleport : MonoBehaviour
{
    #region Properties

    [Header("Game logics configuration")]
    // Creating the positions field that holds the teleporatation positions for each tile
    private Transform[] positions = new Transform[10];

    public Transform pos1;
    public Transform pos2;
    public Transform pos3;
    public Transform pos4;
    public Transform pos5;
    public Transform pos6;
    public Transform pos7;
    public Transform pos8;
    public Transform pos9;
    public Transform pos10;
    public Transform finish;
    public GameObject player;

    // Creating the tiles field that holds the tile game objects (used for the highlighting option)
    private GameObject[] tiles = new GameObject[10];

    [SerializeField] private GameObject left1;
    [SerializeField] private GameObject right1;
    [SerializeField] private GameObject left2;
    [SerializeField] private GameObject right2;
    [SerializeField] private GameObject left3;
    [SerializeField] private GameObject right3;
    [SerializeField] private GameObject left4;
    [SerializeField] private GameObject right4;
    [SerializeField] private GameObject left5;
    [SerializeField] private GameObject right5;

    // Materials for highlighted and non highlighted tiles
    [SerializeField] private Material BaseMaterial;
    [SerializeField] private Material HighlightMaterial;

    // Variable used for determinining if lives were lost (the player died)
    int old_lives = Globals.lives;

    // Determines the row of glass tiles, used for iteration through fields
    private int step = 0;

    // Which tile is highlighted:
    //      0 -> left
    //      1 -> right
    private int side = 0;

    // Delay imput variables
    private float time = 0f;
    private float timeDelay = 2f;
    private float chooseTime = 0f;
    private float chooseTimeDelay = 0.5f;

    // visage|SDK configurations
#if !UNITY_WEBGL
    [HideInInspector]
#endif
    [Header("Script path settings")]
    public string visageSDK;
#if !UNITY_WEBGL
    [HideInInspector]
#endif
    public string visageAnalysisData;
#if !UNITY_WEBGL
    [HideInInspector]
#endif
    [Header("NeuralNet configuration settings")]
    public string ConfigNeuralNet;

    [Header("Tracker configuration settings")]
    // Tracker configuration file name.
    public string ConfigFileEditor;
    public string ConfigFileStandalone;
    public string ConfigFileIOS;
    public string ConfigFileAndroid;
    public string ConfigFileOSX;
    public string ConfigFileWebGL;


    [Header("Tracking settings")]
#if UNITY_WEBGL
    public const int MAX_FACES = 1;
#else
    public const int MAX_FACES = 2;
#endif

    private bool trackerInited = false;

    // Mesh information
    private const int MaxVertices = 1024;
    private const int MaxTriangles = 2048;

    private int VertexNumber = 0;
    private int TriangleNumber = 0;
    private Vector2[] TexCoords = { };
    private Vector3[][] Vertices = new Vector3[MAX_FACES][];
    private int[] Triangles = { };
    private float[] vertices = new float[MaxVertices * 3];
    private int[] triangles = new int[MaxTriangles * 3];
    private float[] texCoords = new float[MaxVertices * 2];
    private MeshFilter meshFilter;
    private Vector2[] modelTexCoords;

    [Header("Tracker output data info")]
    public Vector3[] Translation = new Vector3[MAX_FACES];
    public Vector3[] Rotation = new Vector3[MAX_FACES];
    private bool isTracking = false;
    public int[] TrackerStatus = new int[MAX_FACES];
    private float[] translation = new float[3];
    private float[] rotation = new float[3];

    [Header("Camera settings")]
    public Material CameraViewMaterial;
    public Shader CameraViewShaderRGBA;
    public Shader CameraViewShaderBGRA;
    public Shader CameraViewShaderUnlit;
    public float CameraFocus;
    public int Orientation = 0;
    private int currentOrientation = 0;
    public int isMirrored = 1;
    private int currentMirrored = 1;
    public int camDeviceId = 0;
    private int AndroidCamDeviceId = 0;
    private int currentCamDeviceId = 0;
    public int defaultCameraWidth = -1;
    public int defaultCameraHeight = -1;
    private bool doSetupMainCamera = true;
    private bool camInited = false;

    [Header("Texture settings")]
    public int ImageWidth = 800;
    public int ImageHeight = 600;
    public int TexWidth = 512;
    public int TexHeight = 512;
#if UNITY_ANDROID
	private TextureFormat TexFormat = TextureFormat.RGB24;
#else
    private TextureFormat TexFormat = TextureFormat.RGBA32;
#endif
    private Texture2D texture = null;
    private Color32[] texturePixels;
    private GCHandle texturePixelsHandle;

    [HideInInspector]
    public bool frameForAnalysis = false;
    public bool frameForRecog = false;
    private bool texCoordsStaticLoaded = false;

#if UNITY_ANDROID
	private AndroidJavaObject androidCameraActivity;
	private bool AppStarted = false;
	AndroidJavaClass unity;
#endif

    #endregion

    #region Native code printing

    private bool enableNativePrinting = true;

    //For printing from native code
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void MyDelegate(string str);

    //Function that will be called from the native wrapper
    static void CallBackFunction(string str)
    {
        Debug.Log("::CallBack : " + str);
    }

    #endregion


    private void Awake()
    {
#if PLATFORM_ANDROID && UNITY_2018_3_OR_NEWER
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
            Permission.RequestUserPermission(Permission.Camera);
#endif

#if UNITY_WEBGL
        VisageTrackerNative._preloadFile(Application.streamingAssetsPath + "/Visage Tracker/" + ConfigFileWebGL);
        VisageTrackerNative._preloadFile(Application.streamingAssetsPath + "/Visage Tracker/" + LicenseString.licenseString);
        VisageTrackerNative._preloadFile(Application.streamingAssetsPath + "/Visage Tracker/" + ConfigNeuralNet);
        VisageTrackerNative._setDataPath(".");

        VisageTrackerNative._preloadExternalJS(visageAnalysisData);
        VisageTrackerNative._preloadExternalJS(visageSDK);
        
#endif

        // Set callback for printing from native code
        if (enableNativePrinting)
        {
            /*  MyDelegate callback_delegate = new MyDelegate(CallBackFunction);
                // Convert callback_delegate into a function pointer that can be
                // used in unmanaged code.
                IntPtr intptr_delegate = Marshal.GetFunctionPointerForDelegate(callback_delegate);
                // Call the API passing along the function pointer.
                VisageTrackerNative.SetDebugFunction(intptr_delegate);*/
        }


#if UNITY_ANDROID
		Unzip();
#endif

        string licenseFilePath = Application.streamingAssetsPath + "/" + "/Visage Tracker/";

        // Set license path depending on platform
        switch (Application.platform)
        {

            case RuntimePlatform.IPhonePlayer:
                licenseFilePath = "Data/Raw/Visage Tracker/";
                break;
            case RuntimePlatform.Android:
                licenseFilePath = Application.persistentDataPath + "/";
                break;
            case RuntimePlatform.OSXPlayer:
                licenseFilePath = Application.dataPath + "/Resources/Data/StreamingAssets/Visage Tracker/";
                break;
            case RuntimePlatform.OSXEditor:
                licenseFilePath = Application.dataPath + "/StreamingAssets/Visage Tracker/";
                break;
            case RuntimePlatform.WebGLPlayer:
                licenseFilePath = "";
                break;
            case RuntimePlatform.WindowsEditor:
                licenseFilePath = Application.streamingAssetsPath + "/Visage Tracker/";
                break;
        }

#if UNITY_STANDALONE_WIN
        //NOTE: licensing for Windows platform expects folder path exclusively
        VisageTrackerNative._initializeLicense(licenseFilePath);
#else
        //NOTE: platforms other than Windows expect absolute or relative path to the license file
        VisageTrackerNative._initializeLicense(licenseFilePath + LicenseString.licenseString);
#endif

    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize positions field
        positions[0] = pos1;
        positions[1] = pos2;
        positions[2] = pos3;
        positions[3] = pos4;
        positions[4] = pos5;
        positions[5] = pos6;
        positions[6] = pos7;
        positions[7] = pos8;
        positions[8] = pos9;
        positions[9] = pos10;

        // Initialize tiles field
        tiles[0] = left1;
        tiles[1] = right1;
        tiles[2] = left2;
        tiles[3] = right2;
        tiles[4] = left3;
        tiles[5] = right3;
        tiles[6] = left4;
        tiles[7] = right4;
        tiles[8] = left5;
        tiles[9] = right5;

        // Set configuration file path and name depending on a platform
        string configFilePath = Application.streamingAssetsPath + "/" + ConfigFileStandalone;

        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
                configFilePath = "Data/Raw/Visage Tracker/" + ConfigFileIOS;
                break;
            case RuntimePlatform.Android:
                configFilePath = Application.persistentDataPath + "/" + ConfigFileAndroid;
                break;
            case RuntimePlatform.OSXPlayer:
                configFilePath = Application.dataPath + "/Resources/Data/StreamingAssets/Visage Tracker/" + ConfigFileOSX;
                break;
            case RuntimePlatform.OSXEditor:
                configFilePath = Application.dataPath + "/StreamingAssets/Visage Tracker/" + ConfigFileOSX;
                break;
            case RuntimePlatform.WebGLPlayer:
                configFilePath = ConfigFileWebGL;
                break;
            case RuntimePlatform.WindowsEditor:
                configFilePath = Application.streamingAssetsPath + "/" + ConfigFileEditor;
                break;
        }

        // Initialize tracker with configuration and MAX_FACES
        trackerInited = InitializeTracker(configFilePath);

        // Get current device orientation
        Orientation = GetDeviceOrientation();

        // Open camera in native code
        camInited = OpenCamera(Orientation, camDeviceId, defaultCameraWidth, defaultCameraHeight, isMirrored);

    }

    /// <summary>
    /// Initialize tracker with maximum number of faces - MAX_FACES.
    /// Additionally, depending on a platform set an appropriate shader.
    /// </summary>
    /// <param name="config">Tracker configuration path and name.</param>
    bool InitializeTracker(string config)
    {
        Debug.Log("Visage Tracker: Initializing tracker with config: '" + config + "'");

#if (UNITY_IPHONE || UNITY_ANDROID) && UNITY_EDITOR
		return false;
#endif

#if UNITY_ANDROID

		Shader shader = Shader.Find("Unlit/Texture");
		CameraViewMaterial.shader = shader;

		// initialize visage vision
		VisageTrackerNative._loadVisageVision();

		unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		this.androidCameraActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
#elif UNITY_STANDALONE_WIN || UNITY_WEBGL
        Shader shader = Shader.Find("Custom/RGBATex");
        CameraViewMaterial.shader = shader;
#else
        Shader shader = Shader.Find("Custom/BGRATex");
        CameraViewMaterial.shader = shader;
#endif

#if UNITY_WEBGL
        // initialize tracker
        VisageTrackerNative._initTracker(config, MAX_FACES, "CallbackInitTracker");
        return trackerInited;
#else
        VisageTrackerNative._initTracker(config, MAX_FACES);
        return true;
#endif
    }

    /// <summary>
    /// Get current device orientation.
    /// </summary>
    /// <returns>Returns an integer:
    /// <list type="bullet">
    /// <item><term>0 : DeviceOrientation.Portrait</term></item>
    /// <item><term>1 : DeviceOrientation.LandscapeRight</term></item>
    /// <item><term>2 : DeviceOrientation.PortraitUpsideDown</term></item>
    /// <item><term>3 : DeviceOrientation.LandscapeLeft</term></item>
    /// </list>
    /// </returns>
    int GetDeviceOrientation()
    {
        int devOrientation;

#if UNITY_ANDROID
		//Device orientation is obtained in AndroidCameraPlugin so we only need information about whether orientation is changed
		int oldWidth = ImageWidth;
		int oldHeight = ImageHeight;

		VisageTrackerNative._getCameraInfo(out CameraFocus, out ImageWidth, out ImageHeight);

		if ((oldWidth!=ImageWidth || oldHeight!=ImageHeight) && ImageWidth != 0 && ImageHeight !=0 && oldWidth != 0 && oldHeight !=0 )
			devOrientation = (Orientation ==1) ? 0:1;
		else
			devOrientation = Orientation;
#else
        if (Input.deviceOrientation == DeviceOrientation.Portrait)
            devOrientation = 0;
        else if (Input.deviceOrientation == DeviceOrientation.PortraitUpsideDown)
            devOrientation = 2;
        else if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft)
            devOrientation = 3;
        else if (Input.deviceOrientation == DeviceOrientation.LandscapeRight)
            devOrientation = 1;
        else if (Input.deviceOrientation == DeviceOrientation.FaceUp)
            devOrientation = Orientation;
        else if (Input.deviceOrientation == DeviceOrientation.Unknown)
            devOrientation = Orientation;
        else
            devOrientation = 0;
#endif

        return devOrientation;
    }

    /// <summary> 
    /// Open camera from native code. 
    /// </summary>
    /// <param name="orientation">Current device orientation:
    /// <list type="bullet">
    /// <item><term>0 : DeviceOrientation.Portrait</term></item>
    /// <item><term>1 : DeviceOrientation.LandscapeRight</term></item>
    /// <item><term>2 : DeviceOrientation.PortraitUpsideDown</term></item>
    /// <item><term>3 : DeviceOrientation.LandscapeLeft</term></item>
    /// </list>
    /// </param>
    /// <param name="camDeviceId">ID of the camera device.</param>
    /// <param name="width">Desired width in pixels (pass -1 for default 800).</param>
    /// <param name="height">Desired width in pixels (pass -1 for default 600).</param>
    /// <param name="isMirrored">true if frame is to be mirrored, false otherwise.</param>
    bool OpenCamera(int orientation, int cameraDeviceId, int width, int height, int isMirrored)
    {
#if UNITY_ANDROID
		if (cameraDeviceId == AndroidCamDeviceId && AppStarted)
			return false;

        AndroidCamDeviceId = cameraDeviceId;
		//camera needs to be opened on main thread
		this.androidCameraActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
			this.androidCameraActivity.Call("closeCamera");
			this.androidCameraActivity.Call("GrabFromCamera", width, height, camDeviceId);
		}));
		AppStarted = true;
		return true;
#elif UNITY_WEBGL
        VisageTrackerNative._openCamera(ImageWidth, ImageHeight, isMirrored, "OnSuccessCallbackCamera", "OnErrorCallbackCamera");
        return false;
#elif UNITY_STANDALONE_WIN
        VisageTrackerNative._openCamera(orientation, cameraDeviceId, width, height);
        return true;
#else
        VisageTrackerNative._openCamera(orientation, cameraDeviceId, width, height, isMirrored);
        return true;
#endif
    }

    bool isTrackerReady()
    {
        if (camInited && trackerInited)
        {
            isTracking = true;
        }
        else
        {
            isTracking = false;
        }
        return isTracking;
    }

    /// <summary>
    /// Update Unity texture with frame data from native camera.
    /// </summary>
    void RefreshImage()
    {
        // Initialize texture
        if (texture == null && isTracking && ImageWidth > 0)
        {
            TexWidth = Convert.ToInt32(Math.Pow(2.0, Math.Ceiling(Math.Log(ImageWidth) / Math.Log(2.0))));
            TexHeight = Convert.ToInt32(Math.Pow(2.0, Math.Ceiling(Math.Log(ImageHeight) / Math.Log(2.0))));
            texture = new Texture2D(TexWidth, TexHeight, TexFormat, false);

            var cols = texture.GetPixels32();
            for (var i = 0; i < cols.Length; i++)
                cols[i] = Color.black;

            texture.SetPixels32(cols);
            texture.Apply(false);

            CameraViewMaterial.SetTexture("_MainTex", texture);

#if UNITY_STANDALONE_WIN
            // "pin" the pixel array in memory, so we can pass direct pointer to it's data to the plugin,
            // without costly marshaling of array of structures.
            texturePixels = ((Texture2D)texture).GetPixels32(0);
            texturePixelsHandle = GCHandle.Alloc(texturePixels, GCHandleType.Pinned);
#endif
        }

        if (texture != null && isTracking && TrackerStatus[0] != (int)TrackStatus.OFF)
        {
#if UNITY_STANDALONE_WIN
            // send memory address of textures' pixel data to VisageTrackerUnityPlugin
            VisageTrackerNative._setFrameData(texturePixelsHandle.AddrOfPinnedObject());
            ((Texture2D)texture).SetPixels32(texturePixels, 0);
            ((Texture2D)texture).Apply();
#elif UNITY_IPHONE || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_ANDROID
			if (SystemInfo.graphicsDeviceVersion.StartsWith ("Metal"))
				VisageTrackerNative._bindTextureMetal (texture.GetNativeTexturePtr ());
			else
				VisageTrackerNative._bindTexture ((int)texture.GetNativeTexturePtr ());
#elif UNITY_WEBGL
            VisageTrackerNative._bindTexture(texture.GetNativeTexturePtr());
#endif
        }
    }

    void OnDestroy()
    {
#if UNITY_ANDROID
		this.androidCameraActivity.Call("closeCamera");
#else
        camInited = !(VisageTrackerNative._closeCamera());
#endif
    }

#if UNITY_IPHONE
	void OnApplicationPause(bool pauseStatus) {
		if(pauseStatus){
			camInited = !(VisageTrackerNative._closeCamera());
			isTracking = false;
		}
		else
		{
			camInited = OpenCamera(Orientation, camDeviceId, defaultCameraWidth, defaultCameraHeight, isMirrored);
			isTracking = true;
		}
	}
#endif




    // Update is called once per frame
    void Update()
    {

        // If the tracker is not ready stop the camera and wait for the tracker to be initialized
        if (!isTrackerReady())
            return;

        if (isTracking)
        {
#if UNITY_ANDROID
			if (VisageTrackerNative._frameChanged())
			{
				texture = null;
				doSetupMainCamera = true;
			}	
#endif
            Orientation = GetDeviceOrientation();

            // Check if orientation or camera device changed
            if (currentOrientation != Orientation || currentCamDeviceId != camDeviceId || currentMirrored != isMirrored)
            {
                currentCamDeviceId = camDeviceId;
                currentOrientation = Orientation;
                currentMirrored = isMirrored;

                // Reopen camera with new parameters 
                OpenCamera(currentOrientation, currentCamDeviceId, defaultCameraWidth, defaultCameraHeight, currentMirrored);
                texture = null;
                doSetupMainCamera = true;

            }

            // grab current frame and start face tracking
            VisageTrackerNative._grabFrame();

            VisageTrackerNative._track();
            VisageTrackerNative._getTrackerStatus(TrackerStatus);

            //After the track has been preformed on the new frame, the flags for the analysis and recognition are set to true
            frameForAnalysis = true;
            frameForRecog = true;

            // Set main camera field of view based on camera information
            if (doSetupMainCamera)
            {
                // Get camera information from native
                VisageTrackerNative._getCameraInfo(out CameraFocus, out ImageWidth, out ImageHeight);
                float aspect = ImageWidth / (float)ImageHeight;
                float yRange = (ImageWidth > ImageHeight) ? 1.0f : 1.0f / aspect;
                Camera.main.fieldOfView = Mathf.Rad2Deg * 2.0f * Mathf.Atan(yRange / CameraFocus);
                doSetupMainCamera = false;
            }
        }

        RefreshImage();

        time = time + 1f * Time.deltaTime;
        chooseTime = chooseTime + 1f * Time.deltaTime;

        for (int i = 0; i < TrackerStatus.Length; i++)
        {
            if (TrackerStatus[i] == (int)TrackStatus.OK)
            {

                if (chooseTime>= chooseTimeDelay && step < 10)
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        side = 0;
                        chooseTime = 0f;
                        Debug.Log(side);

                        // Hihlight the next left tile
                        tiles[step].GetComponent<MeshRenderer>().material = HighlightMaterial;
                        tiles[step+1].GetComponent<MeshRenderer>().material = BaseMaterial;
                    }

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        side = 1;
                        chooseTime = 0f;
                        Debug.Log(side);

                        // Hihlight the next right tile
                        tiles[step + 1].GetComponent<MeshRenderer>().material = HighlightMaterial;
                        tiles[step].GetComponent<MeshRenderer>().material = BaseMaterial;
                    }

                }

                if (time >= timeDelay && Input.GetKeyDown(KeyCode.Space))
                {
                    if (side == 0)
                    {
                        if(step >= 10 ){
                            // Finish line teleport
                            player.transform.position = finish.transform.position;
                            Invoke("loadaj",3 );
                        }else{
                            // Move player to the selected tile and reset the highlighted tile
                            player.transform.position = positions[step].transform.position;
                            tiles[step].GetComponent<MeshRenderer>().material = BaseMaterial;
                            step += 2;
                            time = 0f;
                            //Debug.Log(step);
                            //Debug.Log(player.transform.position);
                        }
                    }
                    else if (side == 1)
                    {
                        if(step >= 10 ){
                            // Finish line teleport
                            player.transform.position = finish.transform.position;
                            Invoke("loadaj",3 );
                        }else{
                            // Move player to the selected tile and reset the highlighted tile
                            player.transform.position = positions[step+1].transform.position;
                            tiles[step + 1].GetComponent<MeshRenderer>().material = BaseMaterial;
                            step += 2;
                            time = 0f;
                            //Debug.Log(step);
                            //Debug.Log(player.transform.position);
                        }
                    }


                }

            }
        }

        // Check if the number of lives has changed (player has died),
        // respawn the player and reset the tile related variables
        if(old_lives != Globals.lives){
            step -= 2;
            //Debug.Log(step);
            tiles[step].GetComponent<MeshRenderer>().material = BaseMaterial;
            tiles[step + 1].GetComponent<MeshRenderer>().material = BaseMaterial;
            step = 0;
            old_lives = Globals.lives;
        }

    }



    private void loadaj(){
        SceneManager.LoadScene("StartMenu");
    }
}
