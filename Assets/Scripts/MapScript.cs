using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapScript : MonoBehaviour
{
        [SerializeField]
    private Tilemap map; // attach
        [SerializeField]
    private Tilemap pathTilemap; // attach
        [SerializeField]
    private TileBase movable; //attach
        [SerializeField]
    private TileBase block; //attach
        [SerializeField]
    private Vector3 worldPosition;
        [SerializeField]
    private Vector3Int cellPosition;
        [SerializeField]
    private TileBase tile; //attach
        [SerializeField]
    private TileBase addTileGuide; //attach
        [SerializeField]
    private List<GameObject> EnvironmentObjects;
        [SerializeField]
    private List<GameObject> BuildingObjects;
        [SerializeField]
    public List<MoveOnTilemap.TileAndMovementCost> tileAndMovementCost;
    
    ObjectsListData objsListData;
    void Start()
    {
        objsListData = GameObject.Find("ObjectsList").GetComponent<ObjectsListData>();
        createHexaPlotTerrain(new Vector3Int(0, 0, 0), tile);
        createObjectInHexaTile(BuildingObjects[0], new Vector3Int(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Camera mainCamera = Camera.main;
            Vector3 mousePos = Input.mousePosition;
            mousePos.y -= 0.24f;
            worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            if( map != null && tile != null) 
            {
                cellPosition = map.WorldToCell(worldPosition);
                if(map.GetTile(cellPosition) == addTileGuide)
                {
                    generateRandomTileContent();
                    createHexaPlotTerrain(cellPosition, tile);
                }
            }
            else
            {
                Debug.Log("Attach Map in MapScript");
            }
        }
    }
    private void addGuideTile(Vector3Int pos)
    {
        if(!map.HasTile(pos)) map.SetTile(pos, addTileGuide);
    }
    private void generateRandomTileContent()
    {   
        int objectGenerateCount = UnityEngine.Random.Range(0, 4);
        while(objectGenerateCount != 0){
            int randomObj = UnityEngine.Random.Range(0, EnvironmentObjects.Count);
            createObjectInHexaTile(EnvironmentObjects[randomObj], cellPosition);
            objectGenerateCount--;
        }
    }
    private void createObjectInHexaTile(GameObject ObjectTemp, Vector3Int cellPos)
    {
        EnvironmentObjectData objEnvironmentData = ObjectTemp.GetComponent<EnvironmentObjectData>();
        Vector3 cellPositionInWorld = map.CellToWorld(cellPos); // use to get the center world pos of cell
        cellPositionInWorld.y += 0.083f; //adjust cell as center of top plain
        float X_AxisUse, Y_AxisUse;
        if(objEnvironmentData.FixedPosition)
        {
            X_AxisUse = objEnvironmentData.OffsetX;
            Y_AxisUse = objEnvironmentData.OffsetY;
        }else{
            float randomX = UnityEngine.Random.Range(0-objEnvironmentData.RandomMaxX, objEnvironmentData.RandomMaxX);
            float randomY = UnityEngine.Random.Range(0-objEnvironmentData.RandomMaxY, objEnvironmentData.RandomMaxY);
            X_AxisUse = randomX;
            Y_AxisUse = randomY;
        }
        Vector3 pos = new Vector3(cellPositionInWorld.x + X_AxisUse, cellPositionInWorld.y + Y_AxisUse);
        GameObject instantiatedObject = Instantiate(ObjectTemp, pos, Quaternion.identity);
        //store info
        instantiatedObject.GetComponent<EnvironmentObjectData>().resX = X_AxisUse;
        instantiatedObject.GetComponent<EnvironmentObjectData>().resY = Y_AxisUse;
        instantiatedObject.GetComponent<EnvironmentObjectData>().cellPos = cellPositionInWorld;
        objsListData.AddObject(instantiatedObject);
    }
    private void createHexaPlotTerrain(Vector3Int cellPos, TileBase plotTile)
    {
        map.SetTile(cellPos, plotTile);
        createHexaPlotSlotTerrain(cellPos);
    }
    private void createHexaPlotSlotTerrain(Vector3Int cellPos)
    {
        for (int x = 1; x >= -1; x--)
        {
            for (int y = 1; y >= -1; y--)
            {
                    //if absolute position of y has no remainder in  then x-1 and y-1 if absolute position y has remainder then use y-1
                // if absolute position y has no remainder or even number then use x=0 y-1 and if odd number then use x+1 and y-1
                // skip x+1 and y+1 if absolute y position is even
                // add x-1 and y+1 only if absolute y is even
                bool isSameNegative1 = x == -1 && y == -1;
                bool isSamePositive1 = x == 1 && y == 1;
                bool isNegativeY = x == 0 && y == -1;
                bool isPositiveXnegativeY = x == 1 && y == -1;
                bool isPositiveYnegativeX = x == -1 && y == 1;
                if(Math.Abs(cellPos.y)%2 != 0 && (isSameNegative1 || isPositiveYnegativeX)) continue;
                if(Math.Abs(cellPos.y)%2 == 0 &&( isPositiveXnegativeY || isSamePositive1)) continue;
                Vector3Int posTemp = new Vector3Int(cellPos.x + x, cellPos.y + y);
                addGuideTile(posTemp);
            }
        }
    }

}
