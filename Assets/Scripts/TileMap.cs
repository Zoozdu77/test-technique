using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    #region tiles
    public Transform[,] tileMap;
    [SerializeField] private Vector2 mapSize;
    [SerializeField] private TileTypes[] tileTypes;
    [SerializeField] private GameObject tilesPrefab;
    //baseOffset s'assure que les tiles soient séparées a la bonne distance, et bonus Offset ajoute un offset une ligne sur 2 a cause de la forme hexagonale
    [SerializeField] private float tileBaseOffset;
    [SerializeField] private float tileBonusOffset;
    #endregion

    #region otherSpawn
    [SerializeField] private GameObject hutFullPrefab;
    [SerializeField] private GameObject hutEmptyPrefab;
    [SerializeField] private Transform Player;
    #endregion

    #region classes
    [System.Serializable]
    public class TileTypes
    {
        //les types disponibles
        public string name;
        public float speedInMult;
        public float speedOutMult;
        public bool canWalk;
        public bool slippery;
        public Material mat;
    }
    #endregion

    #region functions
    private void Start()
    {
        tileMap = new Transform[(int)mapSize.x, (int)mapSize.y];
        //tileOffestter permet de choisir si l'offset est ajouté ou non, une ligne sur 2
        bool tileOffsetter = false;
        for (int y = 0; y < mapSize.y; y++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                //instancier la tile, en ajoutant les offset
                tileMap[x, y] = Instantiate(tilesPrefab, new((x * tileBaseOffset - mapSize.x / 2) + tileBonusOffset * System.Convert.ToInt32(tileOffsetter), 0, -y + mapSize.y / 2), Quaternion.identity, transform).transform;

                //choisir le type de tile
                Tile newTile = tileMap[x,y].GetComponent<Tile>();
                //appliquer les effets du type de tile
                newTile.typeId = Random.Range(0, tileTypes.Length);
                newTile.speedOutMult = tileTypes[newTile.typeId].speedOutMult;
                newTile.speedInMult = tileTypes[newTile.typeId].speedInMult;
                newTile.canWalk = tileTypes[newTile.typeId].canWalk;
                newTile.slippery = tileTypes[newTile.typeId].slippery;
                newTile.GetComponent<MeshRenderer>().material = tileTypes[newTile.typeId].mat;
                
            }
            tileOffsetter = !tileOffsetter;
        }

        Player.position = FindSpawnTile() + new Vector3(0, 1, 0);

        Instantiate(hutFullPrefab, FindSpawnTile() + new Vector3(0, 0.7f, 0), Quaternion.identity, null);
        Instantiate(hutEmptyPrefab, FindSpawnTile() + new Vector3(0, 0.7f, 0), Quaternion.identity, null);
    }

    private Vector3 FindSpawnTile()
    {
        Tile SpawnTile = tileMap[Random.Range(0, (int)mapSize.x), Random.Range(0, (int)mapSize.y)].GetComponent<Tile>();
        while (!SpawnTile.canWalk && !SpawnTile.ContainSmtg)
        {
            SpawnTile = tileMap[Random.Range(0, (int)mapSize.x), Random.Range(0, (int)mapSize.y)].GetComponent<Tile>();
        }
        SpawnTile.ContainSmtg = true;
        return SpawnTile.transform.position;
    }
    #endregion
}
