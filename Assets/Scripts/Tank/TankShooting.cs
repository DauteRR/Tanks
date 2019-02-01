using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int playerIdentifier = 1;       
    public Rigidbody shell;            
    public Transform fireTransform;    
    public Slider aimSlider;           
    public AudioSource shootingAudio;  
    public AudioClip chargingClip;     
    public AudioClip fireClip;         
    public float minimumLaunchForce = 15f; 
    public float maximumLaunchForce = 30f; 
    public float maximumChargeTime = 0.75f;

    private string fireButton;         
    private float currentLaunchForce;  
    private float chargeSpeed;         
    private bool fired;                

    private void OnEnable()
    {
        currentLaunchForce = minimumLaunchForce;
        aimSlider.value = minimumLaunchForce;
    }

    private void Start()
    {
        fireButton = "Fire" + playerIdentifier;

        chargeSpeed = (maximumLaunchForce - minimumLaunchForce) / maximumChargeTime;
    }

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        aimSlider.value = minimumLaunchForce;

        if (currentLaunchForce >= maximumLaunchForce && !fired)
        {
            currentLaunchForce = maximumLaunchForce;
            Fire();
        }
        else if (Input.GetButtonDown(fireButton))
        {
            fired = false;
            currentLaunchForce = minimumLaunchForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        else if (Input.GetButton(fireButton) && !fired)
        {
            currentLaunchForce += chargeSpeed * Time.deltaTime;

            aimSlider.value = currentLaunchForce;
        }
        else if (Input.GetButtonUp(fireButton) && !fired)
        {
            Fire();
        }
    }

    private void Fire()
    {
        // Instantiate and launch the shell.
        fired = true;

        Rigidbody shellInstance = Instantiate(shell, fireTransform.position, fireTransform.rotation) as Rigidbody;

        shellInstance.velocity = fireTransform.forward * currentLaunchForce;

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentLaunchForce = minimumLaunchForce;
    }
}