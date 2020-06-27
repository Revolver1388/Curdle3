using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Sonar : MonoBehaviour
{
    private bool sonarIsReady;
    public int sonarCooldownTimer;
    [SerializeField] GameObject sonarCharge;
    [SerializeField] GameObject sonarSphere;
    
    
    // Start is called before the first frame update
    void Start()
    {
        sonarIsReady = true;
        sonarSphere.SetActive(false);
        StartCoroutine(SonarCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("left ctrl"))
        {
            if (sonarIsReady)
            {
                sonarIsReady = false;
                sonarSphere.SetActive(true);

                StartCoroutine(SonarCountdown());
            }
            else
            {
                Debug.Log("Sonar not ready yet");
            }
        }
    }

    private IEnumerator SonarCountdown()
    {
        sonarCharge.GetComponent<Animation>().Play();
        yield return new WaitForSeconds(sonarCooldownTimer);
        sonarIsReady = true;
        sonarSphere.SetActive(false);
    }

}
