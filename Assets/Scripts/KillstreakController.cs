using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class KillstreakController : MonoBehaviour
{
    public Camera killstreakCamera;
    public Transform POV;

    public Animator animator;

    public Vector3 offset;

    public bool loaded = false;

    public FirearmController gun;

    [SerializeField]
    private float time = 15f;

    [SerializeField]
    private Light killstreakLight;

    [SerializeField]
    private PlayerWeaponController playerWeaponController;

    [SerializeField]
    private Camera mainCamera;
    public delegate void KillstreakEndHandler();
    public event KillstreakEndHandler KillstreakEnd;

    public Image progress;

    void OnEnable()
    {
        playerWeaponController.OnActivateKillstreak += ActivateKillstreak;
    }

    void OnDisable()
    {
        playerWeaponController.OnActivateKillstreak -= ActivateKillstreak;
    }

    private void ActivateKillstreak()
    {
        LoadState();
        // Start the coroutine to wait for 1.5 seconds
        StartCoroutine(LoadCoroutine());
        StartCoroutine(Rack());
    }

    // Start is called before the first frame update
    void Start()
    {
        playerWeaponController = FindObjectOfType<PlayerWeaponController>();
        
    }
    private IEnumerator Rack()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<AudioSource>().Play();
    }

    private IEnumerator LowerProgress()
    {
        float timeElapsed = 0f;
        while (timeElapsed < time)
        {
            progress.fillAmount = Mathf.Lerp(1, 0, timeElapsed / time);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }


    private void LoadState()
    {
        mainCamera.enabled = false;
        killstreakCamera.gameObject.SetActive(true);
        killstreakLight.enabled = true;
        loaded = false;
        gun.enabled = false;
        animator.Play("Waiting");
        animator.SetTrigger("killstreak");

    }

    private IEnumerator ExitState(float time)
    {
        StartCoroutine(LowerProgress());
        yield return new WaitForSeconds(time);
        killstreakLight.enabled = false;
        loaded = false;
        gun.enabled = false;
        mainCamera.enabled = true;
        KillstreakEnd?.Invoke();
    }

    

    private IEnumerator LoadCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        gun.enabled = true;
        loaded = true;
        StartCoroutine(ExitState(time));
    }

    // Update is called once per frame
    void Update()
    {
        // Check if loaded is true before processing input
        if (!loaded)
            return;

        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y") * -1;

        // Rotate the gun transform based on mouse input
        transform.Rotate(Vector3.up, mouseX);

        // Update the pitch rotation
        float pitch = transform.localEulerAngles.x - mouseY;

        // Normalize the pitch to be within -180 to 180 degrees
        if (pitch > 180) pitch -= 360;
        if (pitch < -180) pitch += 360;

        // Clamp the pitch
        pitch = Mathf.Clamp(pitch, -70f, 15f);

        // Update the yaw rotation
        float yaw = transform.localEulerAngles.y + mouseX;

        // Normalize the yaw to be within -180 to 180 degrees
        if (yaw > 180) yaw -= 360;
        if (yaw < -180) yaw += 360;

        // Clamp the yaw
        yaw = Mathf.Clamp(yaw, -60f, 60f);

        // Apply the rotations
        transform.localEulerAngles = new Vector3(pitch, yaw, 0f);

        // Set the camera's position relative to the gun
        killstreakCamera.transform.position = POV.position + POV.TransformDirection(offset);

        // Match the gun's forward direction but keep a fixed orientation
        killstreakCamera.transform.rotation = Quaternion.LookRotation(POV.forward, Vector3.up);

        // Reset the mount's position and rotation
    }
}
