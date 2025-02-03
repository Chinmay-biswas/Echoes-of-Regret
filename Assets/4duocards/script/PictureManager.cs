using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PictureManager : MonoBehaviour
{

    public Picture picturePrefab;
    public Transform picSpawnPos;
    [HideInInspector]public List<Picture> pictureList;
    private List<Material> materialList = new List<Material>();
    private List<string> texturePath = new List<string>();
    private Material _firstMaterial;
    private string firstTexturePath;
    private Vector2 _offset = new Vector2(2f, 1.5f);
    [SerializeField] private Vector2 grid = new Vector2(3, 4);
    public List<GameObject> openPics = new List<GameObject>();
    public List<GameObject> disabledPics;
    [SerializeField] GameObject uiScreen;
    [SerializeField] TMP_Text triesUI;
    public int tries = 0;

    // Start is called before the first frame update
    void Start()
    {
        LoadMaterials();
        spawnPictureGrid((int)grid.x, (int)grid.y, picSpawnPos.position, _offset, false);
        movePicture((int)grid.x, (int)grid.y, picSpawnPos.position, _offset);
        // implement a feature to automatically set the offsets and the grid size depending on the difficulty
    }

    private void LoadMaterials()
    {
        var materialFilePath = "Materials/";
        var textureFilePath = "Images/Fruits/";
        var pairNumber = (grid.x * grid.y/2); // Find pair number 
        const string matBaseName = "Pic";
        var _firstMaterialName = "Back";
        for (var index = 1; index <= pairNumber; index++)
        {
            var currentFilePath = materialFilePath + matBaseName + index;
            Material mat = Resources.Load(currentFilePath, typeof(Material)) as Material; 
            materialList.Add(mat);

            var currentTextureFilePath = textureFilePath + matBaseName + index;
            texturePath.Add(currentTextureFilePath);
        }
        firstTexturePath = textureFilePath + _firstMaterialName;
        _firstMaterial = Resources.Load(materialFilePath + _firstMaterialName , typeof(Material))as Material;
    }

    // Update is called once per frame
    void Update()
    {
        Input.GetKeyDown(KeyCode.Q);
        if (disabledPics.Count == grid.x*grid.y)
        {
            uiScreen.SetActive(true);
        }
        triesUI.SetText("Number of tries : "+tries);
    }
    void spawnPictureGrid(int rows, int column, Vector2 pos, Vector2 offset, bool scaleDown)
    {
        for(int col =0; col < column; col++)
        {
            for (int row = 0; row< rows; row++)
            {
                var tempPicture = (Picture)Instantiate(picturePrefab, picSpawnPos.position, picturePrefab.transform.rotation);
                tempPicture.name = "Card"+row + "_" + col;
                pictureList.Add(tempPicture); 
            }
        }
        applyTextures();
    }
    public void applyTextures()
    {
        var rndMaterial = UnityEngine.Random.Range(0, materialList.Count);
        var AppliedTimes = new int[materialList.Count];

        for (int i = 0; i< materialList.Count; i++)
        {
            AppliedTimes[i] = 0;
        }
        foreach (var mat in pictureList)
        {
            var randPrev = rndMaterial;
            var counter = 0;
            var forceMat = false;
            while (AppliedTimes[rndMaterial] >= 2 || ((randPrev == rndMaterial) && !forceMat))
            {
                rndMaterial = UnityEngine.Random.Range(0, materialList.Count);
                counter++;
                if (counter == 100)
                {
                    for (int j = 0; j < materialList.Count; j++)
                    {
                        if (AppliedTimes[j] < 2)
                        {
                            rndMaterial = j;
                            forceMat = true;
                        }
                    }
                    if (!forceMat)
                        return;
                }
            }
            mat.setFirstMaterial(_firstMaterial, firstTexturePath);
            mat.applyFirstMaterial();
            mat.setSecondMaterial(materialList[rndMaterial], texturePath[rndMaterial]);
            AppliedTimes[rndMaterial]++;
            forceMat = false;
        }

    }
    private void movePicture(int rows, int column, Vector2 pos, Vector2 offset)
    {
        var index = 0;
        for (var col = 0; col< column;col ++)
        {
            for (int row = 0; row < rows; row ++)
            {
                var targetPosition = new Vector3((pos.x + (offset.x * (row-rows/2))), (pos.y - (offset.y * col)), 0.0f);
                StartCoroutine(moveToPosition(targetPosition, pictureList[index]));
                index++;
            }
        }
    }
    private IEnumerator moveToPosition(Vector3 target, Picture obj)
    {
        var randomDist = 7;
        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, randomDist * Time.deltaTime);
            yield return 0;
        }
    }



}
