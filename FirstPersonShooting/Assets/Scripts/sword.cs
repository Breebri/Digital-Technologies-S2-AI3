using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    private CharacterInput controls;
    public float damage = 10f;
    public float range = 10f;
    public Camera fpsCam;

    private void Awake()
    {
        controls = new CharacterInput();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        if (controls.Player.Shoot.triggered)
        {
            slash();
        }
    }

    void slash()
    {

        AudioSource swordAS = GetComponent<AudioSource>();
        swordAS.Play();

        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target enemy = hit.transform.GetComponent<Target>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                
            }
        }
    }
  
}
