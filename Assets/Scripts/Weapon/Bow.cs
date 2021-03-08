using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [System.Serializable]
    public class BowSettings
    {
        [Header("Arrow Settings")]
        public float arrowCount;
        public Rigidbody arrowPrefab;
        public Transform arrowPos;
        public Transform arrowEquipParent;
        public float arrowForce = 3;

        [Header("Bow Equip & UnEquip Settings")]
        public Transform EquipPos;
        public Transform UnEquipPos;

        public Transform UnEquipParent;
        public Transform EquipParent;

        [Header("Bow String Settings")]
        public Transform bowString;
        public Transform stringInitialPos;
        public Transform stringHandPullPos;
        public Transform stringInitialParent;

        [Header("Bow Audio Settings")]
        public AudioClip pullStringAudio;
        public AudioClip releaseStringAudio;
        public AudioClip drawArrowAudio;
    }
    [SerializeField]
    public BowSettings bowSettings;

    [Header("Crosshair Settings")]
    public GameObject crossHairPrefab;
    GameObject currentCrossHair;

    Rigidbody currentArrow;

    bool canPullString = false;
    bool canFireArrow = false;

    AudioSource bowAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        bowAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickArrow()
    {
        bowAudio.PlayOneShot(bowSettings.drawArrowAudio);
        bowSettings.arrowPos.gameObject.SetActive(true);
    }

    public void DisableArrow()
    {
        bowSettings.arrowPos.gameObject.SetActive(false);
    }

    public void PullString()
    {
        bowSettings.bowString.transform.position = bowSettings.stringHandPullPos.position;
        bowSettings.bowString.transform.parent = bowSettings.stringHandPullPos;
    }

    public void ReleaseString()
    {
        bowSettings.bowString.transform.position = bowSettings.stringInitialPos.position;
        bowSettings.bowString.transform.parent = bowSettings.stringInitialParent;
    }

    public void EquipBow()
    {
        this.transform.position = bowSettings.EquipPos.position;
        this.transform.rotation = bowSettings.EquipPos.rotation;
        this.transform.parent = bowSettings.EquipParent;
    }

    public void UnEquipBow()
    {
        this.transform.position = bowSettings.UnEquipPos.position;
        this.transform.rotation = bowSettings.UnEquipPos.rotation;
        this.transform.parent = bowSettings.UnEquipParent;
    }

    public void ShowCrosshair(Vector3 crosshairPos)
    {
        if (!currentCrossHair)
            currentCrossHair = Instantiate(crossHairPrefab) as GameObject;

        currentCrossHair.transform.position = crosshairPos;
        currentCrossHair.transform.LookAt(Camera.main.transform);
    }

    public void RemoveCrosshair()
    {
        if (currentCrossHair)
            Destroy(currentCrossHair);
    }

    public void PullAudio()
    {
        bowAudio.PlayOneShot(bowSettings.pullStringAudio);
    }

    public void Fire(Vector3 hitPoint)
    {
        if (bowSettings.arrowCount < 1)
            return;

        bowAudio.PlayOneShot(bowSettings.releaseStringAudio);
        Vector3 dir = hitPoint - bowSettings.arrowPos.position;
        currentArrow = Instantiate(bowSettings.arrowPrefab, bowSettings.arrowPos.position, bowSettings.arrowPos.rotation) as Rigidbody;

        currentArrow.AddForce(dir * bowSettings.arrowForce, ForceMode.Force);

        bowSettings.arrowCount -= 1;
    }
}
