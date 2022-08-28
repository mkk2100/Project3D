using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {

    public bool isAnimated = false;

    public bool isRotating = false;
    public bool isFloating = false;
    public bool isScaling = false;

    public Vector3 rotationAngle;
    public float rotationSpeed;

    public float floatSpeed;
    private bool goingUp = true;
    public float floatRate;
    private float floatTimer;
   
    public Vector3 startScale;
    public Vector3 endScale;

    private bool scalingUp = true;
    public float scaleSpeed;
    public float scaleRate;
    private float scaleTimer;

	void Start () 
    {
	
	}
	
	void Update () 
    {
        if(!isAnimated) return;
        if(isRotating) transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
        if(isFloating)
        {
            floatTimer += Time.deltaTime;
            Vector3 moveDir = new Vector3(0.0f, 0.0f, floatSpeed);
            transform.Translate(moveDir);

            if (goingUp && floatTimer >= floatRate)
            {
                goingUp = false;
                floatTimer = 0;
                floatSpeed = -floatSpeed;
            }

            else if(!goingUp && floatTimer >= floatRate)
            {
                goingUp = true;
                floatTimer = 0;
                floatSpeed = +floatSpeed;
            }
        }
        if(isScaling)
        {
            scaleTimer += Time.deltaTime;

            if (scalingUp)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
            }
            else if (!scalingUp)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
            }

            if(scaleTimer >= scaleRate)
            {
                if (scalingUp) { scalingUp = false; }
                else if (!scalingUp) { scalingUp = true; }
                scaleTimer = 0;
            }
        }
	}

    public void StartGameButton()
    {
        StartCoroutine(Effect1());
    }

    IEnumerator Effect1()
    {
        while(rotationSpeed < 50)
        {
            rotationSpeed += Time.deltaTime * 8;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Effect2());
    }

    IEnumerator Effect2()
    {
        Vector3 tmp = new Vector3(0, 1, 0);
        while(this.transform.position.z > -10)
        {
            tmp.z -= 0.5f;
            this.transform.position = tmp;
            yield return new WaitForEndOfFrame();
        }
        TransitionManager.instance.LoadNextZone(TransitionManager.NextZone.Zone1);
    }
}
