using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;
using Cinemachine;

public class KartController : MonoBehaviour
{
    #region Field Setting
    [Header("Kart")]
    public Transform kartNormal;
    private Transform kartModel, frontWheels, backWheels, steeringWheel;
    private Transform taxi, lfWheel, rfWheel, lbWheel, rbWheel;
    private Transform driver, colleagues;
    public Rigidbody sphere, t_sphere;
    public Transform wheelParticles, flashParticles;
    public Transform t_wheelParticles, t_flashParticles;
    public Color[] turboColors;

    [Header("References")]
    private PostProcessVolume postVolume;
    private PostProcessProfile postProfile;

    [Header("Settings")]
    public LayerMask layerMask;
    private float maxSpeed = 50f;
    private float acceleration = 30f;
    private float deceleration = 3f;
    private float steering = 10f;
    private float gravity = 25f;
    private float boostDuration = 0.3f;
    private float dizzyDuration = 1.5f;
    private float firstDriftLimit = 50f;
    private float secondDriftLimit = 100f;
    private float thirdDriftLimit = 150f;

    [Header("States")]
    private List<ParticleSystem> primaryParticles = new List<ParticleSystem>();
    private List<ParticleSystem> secondaryParticles = new List<ParticleSystem>();

    private float input;
    private float currentSpeed;
    private float rotate, currentRotate;
    private int driftDirection;
    private float driftPower;
    private int driftMode = 0;
    private bool drifting;
    private bool isDizzy = false;
    private bool isTaxi = false;
    private bool first, second, third;
    private Color c;

    private bool isOnGround = false;

    private PlayerData playerData;
    private int shieldCount = 0;
    #endregion

    #region Initialize - Awake & Start
    void Awake()
    {
        driver = kartNormal.Find("Driver");
        colleagues = kartNormal.Find("CharacterModels");
        kartModel = kartNormal.Find("KartModel");
        frontWheels = kartModel.Find("f_wheel");
        backWheels = kartModel.Find("b_wheel");
        steeringWheel = kartModel.Find("head");

        taxi = kartNormal.Find("Taxi");
    }

    void Start()
    {
        transform.parent.gameObject.GetComponentInChildren<CollisionManager>().KCRegister(this);

        postVolume = Camera.main.GetComponent<PostProcessVolume>();
        postProfile = postVolume.profile;

        CacheParticles();

        playerData = new PlayerData(SaveNickname.LoadNickname());
        StartCoroutine(Per10SecondsUpdate());

        taxi.gameObject.SetActive(false);
    }

    private void CacheParticles()
    {
        for (int i = 0; i < wheelParticles.GetChild(0).childCount; i++)
        {
            primaryParticles.Add(wheelParticles.GetChild(0).GetChild(i).GetComponent<ParticleSystem>());
        }

        for (int i = 0; i < wheelParticles.GetChild(1).childCount; i++)
        {
            primaryParticles.Add(wheelParticles.GetChild(1).GetChild(i).GetComponent<ParticleSystem>());
        }

        foreach(ParticleSystem p in flashParticles.GetComponentsInChildren<ParticleSystem>())
        {
            secondaryParticles.Add(p);
        }
    }

    private IEnumerator Per10SecondsUpdate()
    {
        while (true)
        {
            playerData.Insert(transform.position);
            yield return new WaitForSeconds(10f);
        }
    }
    #endregion

    #region Callbacks - Update
    void Update()
    {
        // Follow Collider
        transform.position = sphere.transform.position - new Vector3(0, 0.4f, 0);

        if (isDizzy) return;
        input = Input.GetAxis("Horizontal");

        // Accelerate
        if (Input.GetAxis("Vertical") > 0) currentSpeed += acceleration * Time.deltaTime;
        else currentSpeed = Mathf.Lerp(currentSpeed, 0, deceleration * Time.deltaTime);
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);

        // Steer
        if (input != 0)
        {
            Steer((int)(Mathf.Sign(input)), Mathf.Abs(input));

            // Begin Drift
            if (!drifting && Input.GetKeyDown(KeyCode.LeftShift))
            {
                drifting = true;
                driftDirection = input > 0 ? 1 : -1;
                foreach (ParticleSystem p in primaryParticles)
                {
                    var pmain = p.main;
                    pmain.startColor = Color.clear;
                    p.Play();
                }
                kartModel.parent.DOComplete();
                kartModel.parent.DOPunchPosition(transform.up * .2f, .3f, 5, 1);
            }
        }

        // Drifting
        if (drifting)
        {
            float control = (driftDirection == 1) ? ExtensionMethods.Remap(input, -1, 1, 0, 2) : ExtensionMethods.Remap(input, -1, 1, 2, 0);
            float powerControl = (driftDirection == 1) ? ExtensionMethods.Remap(input, -1, 1, .2f, 1) : ExtensionMethods.Remap(input, -1, 1, 1, .2f);
            Steer(driftDirection, control);
            driftPower += powerControl * Time.deltaTime * 120;
            UpdateDriftEffects();

            // End Drift
            if (Input.GetKeyUp(KeyCode.LeftShift)) Boost();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            sphere.velocity = new Vector3(sphere.velocity.x, 30f, sphere.velocity.z);
            isOnGround = false;
        }

        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        rotate = 0f;

        //Animations

        //a) Kart
        if (!drifting)
        {
            kartModel.localEulerAngles = Vector3.Lerp(kartModel.localEulerAngles, new Vector3(0, 90 + (input * 15), kartModel.localEulerAngles.z), .2f);
        }
        else
        {
            float control = (driftDirection == 1) ? ExtensionMethods.Remap(input, -1, 1, .5f, 2) : ExtensionMethods.Remap(input, -1, 1, 2, .5f);
            kartModel.parent.localRotation = Quaternion.Euler(0, Mathf.LerpAngle(kartModel.parent.localEulerAngles.y,(control * 10) * driftDirection, .2f), 0);
        }

        //b) Wheels
        frontWheels.localEulerAngles = new Vector3(0, (input * 15), frontWheels.localEulerAngles.z);
        frontWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude/2);
        backWheels.localEulerAngles += new Vector3(0, 0, sphere.velocity.magnitude/2);

        //c) Steering Wheel
        steeringWheel.localEulerAngles = new Vector3(0, (input * 1), 77.4f);

        //d) character
        driver.localEulerAngles = new Vector3(0, (input * 7), 0);
        Vector3 kartEuler = kartModel.localEulerAngles;
        colleagues.localEulerAngles = new Vector3(kartEuler.x, kartEuler.y - 90f, kartEuler.z);
    }

    public void Steer(int direction, float amount) => rotate = (steering * direction) * amount;
    public void Boost()
    {
        drifting = false;

        if (driftMode > 0)
        {
            DOVirtual.Float(currentSpeed * 3, currentSpeed, boostDuration * driftMode, Speed);
            DOVirtual.Float(0, 1, .5f, ChromaticAmount).OnComplete(() => DOVirtual.Float(1, 0, .5f, ChromaticAmount));
            if (isTaxi) {
                taxi.Find("Tube001").GetComponentInChildren<ParticleSystem>().Play();
                taxi.Find("Tube002").GetComponentInChildren<ParticleSystem>().Play();
            }
            else {
                kartModel.Find("Tube001").GetComponentInChildren<ParticleSystem>().Play();
            }
        }

        driftPower = 0;
        driftMode = 0;
        first = false; second = false; third = false;

        foreach (ParticleSystem p in primaryParticles)
        {
            var pmain = p.main;
            pmain.startColor = Color.clear;
            p.Stop();
        }
        kartModel.parent.DOLocalRotate(Vector3.zero, .5f).SetEase(Ease.OutBack);
    }

    public void UpdateDriftEffects()
    {
        if (!first)
            c = Color.clear;

        if (driftPower > firstDriftLimit && driftPower < secondDriftLimit && !first)
        {
            first = true;
            c = turboColors[0];
            driftMode = 1;

            PlayFlashParticle(c);
        }

        if (driftPower >= secondDriftLimit && driftPower < thirdDriftLimit && !second)
        {
            second = true;
            c = turboColors[1];
            driftMode = 2;

            PlayFlashParticle(c);
        }

        if (driftPower >= thirdDriftLimit && !third)
        {
            third = true;
            c = turboColors[2];
            driftMode = 3;

            PlayFlashParticle(c);
        }

        foreach (ParticleSystem p in primaryParticles)
        {
            var pmain = p.main;
            pmain.startColor = c;
        }

        foreach(ParticleSystem p in secondaryParticles)
        {
            var pmain = p.main;
            pmain.startColor = c;
        }
    }

    void PlayFlashParticle(Color c)
    {
        GameObject.Find("CM vcam1").GetComponent<CinemachineImpulseSource>().GenerateImpulse();

        foreach (ParticleSystem p in secondaryParticles)
        {
            var pmain = p.main;
            pmain.startColor = c;
            p.Play();
        }
    }

    private void Speed(float x) => currentSpeed = x;
    private void ChromaticAmount(float x) => postProfile.GetSetting<ChromaticAberration>().intensity.value = x;

    #endregion

    #region Callbacks - FixedUpdate
    private void FixedUpdate()
    {
        //Forward Acceleration
        if(!drifting)
            sphere.AddForce(-kartModel.transform.right * currentSpeed, ForceMode.Acceleration);
        else
            sphere.AddForce(transform.forward * currentSpeed, ForceMode.Acceleration);

        //Gravity
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        RaycastHit hitOn;
        RaycastHit hitNear;

        Physics.Raycast(transform.position + (transform.up*.1f), Vector3.down, out hitOn, 1.1f, layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out hitNear, 2.0f, layerMask);

        //Normal Rotation
        kartNormal.up = Vector3.Lerp(kartNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
        kartNormal.Rotate(0, transform.eulerAngles.y, 0);
    }
    #endregion

    #region Getter/Setter
    public PlayerData GetPlayerData()
    {
        return this.playerData;
    }

    public float GetMaxSpeed() {
        return maxSpeed;
    }

    public void SetMaxSpeed(float x) {
        maxSpeed = x;
    }

    public float GetAccel() {
        return acceleration;
    }

    public void SetAccel(float x) {
        acceleration = x;
    }

    public float GetBoostDuration() {
        return boostDuration;
    }

    public void SetBoostDuration(float x) {
        boostDuration = x;
    }

    public void GiveShields(int count) {
        shieldCount += count;
    }

    public ShieldResult UseShield() {
        Debug.Log($"쉴드 사용 시도 (현재 {shieldCount}개 남음)");
        if (shieldCount == 0) return ShieldResult.Failed;
        shieldCount--;
        return ShieldResult.Succeed;
    }

    public int GetRemainingShields() {
        return shieldCount;
    }

    public void IncrementDizzyTime(float x) {
        dizzyDuration += x;
    }

    public void GetDizzy() {
        CancelInvoke(nameof(NotDizzy));
        isDizzy = true;
        sphere.velocity = Vector3.zero;
        currentSpeed = 0;
        Debug.Log("Dizzy");
        Invoke(nameof(NotDizzy), dizzyDuration);
    }

    private void NotDizzy() {
        isDizzy = false;
        Debug.Log("Not Dizzy");
    }

    public void ChangeBikeToTaxi() {
        driver.gameObject.SetActive(false);
        colleagues.gameObject.SetActive(false);
        kartModel.gameObject.SetActive(false);
        frontWheels.gameObject.SetActive(false);
        backWheels.gameObject.SetActive(false);
        steeringWheel.gameObject.SetActive(false);
        taxi.gameObject.SetActive(true);
        isTaxi = true;
        sphere = t_sphere;
        wheelParticles = t_wheelParticles;
        flashParticles = t_flashParticles;
        CacheParticles();
    }


    #endregion

    public void SetAsOnGround() {
        isOnGround = true;
    }
}

public enum ShieldResult {
  Succeed,
  Failed
}
