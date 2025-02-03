using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picture : MonoBehaviour
{
    private PictureManager PictureManagerScript;
    public GameObject PictureManager;
    private Material _firstMaterial;
    private Material _secondMaterial;
    float time = 0;
    float max = 1.5f;
    private Quaternion _currentrotation;

    // Start is called before the first frame update
    void Start()
    {
        PictureManager = GameObject.Find("[PictureManager]");
        PictureManagerScript = PictureManager.GetComponent<PictureManager>();
        _currentrotation = gameObject.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (PictureManagerScript.openPics.Contains(gameObject))
            if (PictureManagerScript.openPics.Count == 2)
            {
                if (time >= max)
                {
                    time = 0;
                    if (PictureManagerScript.openPics[0] != gameObject)
                    {
                        PictureManagerScript.tries++;
                        if (PictureManagerScript.openPics[0].GetComponent<Picture>()._secondMaterial == _secondMaterial)
                        {
                            PictureManagerScript.disabledPics.Add(PictureManagerScript.openPics[0]);
                            PictureManagerScript.disabledPics.Add(PictureManagerScript.openPics[1]);
                            PictureManagerScript.openPics[0].SetActive(false);
                            gameObject.SetActive(false);
                        }
                        else
                        {
                            //if they are not the same then do something
                            applyFirstMaterial();
                            PictureManagerScript.openPics[0].GetComponent<Picture>().applyFirstMaterial();
                        }
                        PictureManagerScript.openPics.Clear();
                    }
                }
                else
                {
                    time += Time.deltaTime;
                }
            
            }
    }

    void OnMouseDown()
    {
        StartCoroutine(loopRotation(45f, false));
    }

    IEnumerator loopRotation (float angle, bool firstMat)
    {
        var rot = 0f; 
        const float dir = 1f;
        const float speed = 100f;
        const float speed1 = 90f;
        var startAngle = angle;
        var assigned = false;

        if (firstMat)
        {
            while (rot< angle ) 
            {
                var step = Time.deltaTime * speed1;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0,2,0) * step * dir);
                if (rot >= (startAngle - 2) && assigned == false)
                {
                    applyFirstMaterial();
                    assigned = true;
                }
                rot += (1 * step * dir);
                yield return null;

            }
        }
        else
        {
            while(angle > 0)
            {
                float step = Time.deltaTime * speed;
                gameObject.GetComponent<Transform>().Rotate(new Vector3(0,2,0)* step*dir );
                angle -= 1*step*dir;
                yield return null;
            }
        }
        gameObject.GetComponent<Transform>().rotation = _currentrotation;

        if (!firstMat)
        {
            applySecondMaterial();
        }

    }

    public void setFirstMaterial(Material mat, string texturePath)
    {
        _firstMaterial = mat;
        _firstMaterial.mainTexture  = Resources.Load(texturePath , typeof(Texture2D)) as Texture2D;
    }
    public void setSecondMaterial(Material mat, string texturePath)
    {
        _secondMaterial = mat;
        _secondMaterial.mainTexture = Resources.Load(texturePath, typeof(Texture2D)) as Texture2D;
    }
    public void applyFirstMaterial()
    {
        gameObject.GetComponent<Renderer>().material = _firstMaterial;
    }
    public void applySecondMaterial()
    {
        if (PictureManagerScript.openPics.Count == 0)
        {
            PictureManagerScript.openPics.Add(gameObject);
            gameObject.GetComponent<Renderer>().material = _secondMaterial;
        }
        else if(PictureManagerScript.openPics.Count == 1)
        {
            if (PictureManagerScript.openPics[0] != gameObject)
            {
                PictureManagerScript.openPics.Add(gameObject);
                gameObject.GetComponent<Renderer>().material = _secondMaterial;
            }
        }
    }

}
