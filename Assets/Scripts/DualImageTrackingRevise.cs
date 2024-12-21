using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class DualImageTrackingRevise : MonoBehaviour {
    public GameObject[] prefabMap; // 여러 개의 Prefab을 저장하는 배열
    public Dictionary<string, GameObject> imageToPrefabMap = new Dictionary<string, GameObject>(); // 이미지 이름에 대응하는 Prefab 저장

    public string[] targetImageNames; // 트래킹할 이미지들의 이름
    private ARTrackedImageManager trackedImageManager;
    // 생성된 GameObject 관리
    private Dictionary<string, GameObject> instantiatedObjects = new Dictionary<string, GameObject>();

    // Variable to store the audio source component
    private AudioSource audioSource;
    // Audio clip that stores the click sound
    public AudioClip clickSound;

    void Start()
    {
        // Get the AudioSource component from the game object this script is attached to.
        audioSource = GetComponent<AudioSource>();
    }


    void Awake() {
        // ARTrackedImageManager 컴포넌트 가져오기
        trackedImageManager = GetComponent<ARTrackedImageManager>();

        // 이미지 이름에 대응하는 Prefab 매핑 설정
        if (targetImageNames.Length == prefabMap.Length) {
            for (int i = 0; i < targetImageNames.Length; i++) {
                imageToPrefabMap[targetImageNames[i]] = prefabMap[i];
            }
        } else {
            Debug.LogError("Image names and prefab array size mismatch!");
        }
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            // About the first touch
            Touch touch = Input.GetTouch(0);

            // The state of the touch is Began (touch started state), and a raycast is fired from the location where the touch occurred.
            if (touch.phase == TouchPhase.Began)
            {
                // Fires a ray from the camera to the touched location.
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Raycast checks for collided objects
                if (Physics.Raycast(ray, out hit))
                {
                    // If the collided object is a created prefab, the object is processed.
                    GameObject touchedObject = hit.collider.gameObject;

                    if (touchedObject.CompareTag("Cat"))
                    {
                        if (clickSound != null)
                        {
                            audioSource.PlayOneShot(clickSound);
                            SceneManager.LoadScene("PreCatStory");
                        }

                    }

                    if (touchedObject.CompareTag("Plant"))
                    {
                        if (clickSound != null)
                        {
                            audioSource.PlayOneShot(clickSound);
                            SceneManager.LoadScene("PrePlantStory");
                        }

                    }

                    if (touchedObject.CompareTag("Cycle"))
                    {
                        if (clickSound != null)
                        {
                            audioSource.PlayOneShot(clickSound);
                            SceneManager.LoadScene("PreCycleStory");
                        }

                    }

                    if (touchedObject.CompareTag("Light"))
                    {
                        if (clickSound != null)
                        {
                            audioSource.PlayOneShot(clickSound);
                            SceneManager.LoadScene("PreGiraffeStory");
                        }

                    }

                }
            }
        }
    }

    void OnEnable() {
        // 트래킹 이벤트 등록
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable() {
        // 트래킹 이벤트 해제
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs) {
        // 새로 인식된 이미지 처리
        foreach (var trackedImage in eventArgs.added) {
            HandleTrackedImage(trackedImage);
        }

        // 업데이트된 이미지 처리
        foreach (var trackedImage in eventArgs.updated) {
            HandleTrackedImage(trackedImage);
        }
    }

    private void HandleTrackedImage(ARTrackedImage trackedImage) {
        // 이미지 이름에 맞는 Prefab을 찾음
        if (imageToPrefabMap.ContainsKey(trackedImage.referenceImage.name)) {
            GameObject prefab = imageToPrefabMap[trackedImage.referenceImage.name];
            GameObject instantiatedObject;

            // 트래킹 상태에 따라 객체 생성/업데이트
            if (trackedImage.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking) {
                if (!instantiatedObjects.ContainsKey(trackedImage.referenceImage.name)) {
                    // 새로운 이미지가 트래킹되었을 때 객체 생성
                    instantiatedObject = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                    instantiatedObject.transform.parent = trackedImage.transform; // 이미지와 함께 움직이도록 설정
                    instantiatedObjects[trackedImage.referenceImage.name] = instantiatedObject;
                } else {
                    // 객체가 이미 존재한다면 위치와 회전 업데이트
                    instantiatedObject = instantiatedObjects[trackedImage.referenceImage.name];
                    instantiatedObject.transform.position = trackedImage.transform.position;
                    instantiatedObject.transform.rotation = trackedImage.transform.rotation;
                }
            } else {
                // 트래킹 상태가 손실되었을 경우 객체 숨기기
                if (instantiatedObjects.ContainsKey(trackedImage.referenceImage.name)) {
                    instantiatedObjects[trackedImage.referenceImage.name].SetActive(false);
                }
            }
        }
    }
}