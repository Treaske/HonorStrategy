using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ArmyConfig : MonoBehaviour
{
    public HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();
    [SerializeField] GameObject characterPlayer;

    public CharManager charManager;

    public void GenerateArmy(int gWidht, int gHeight, int nSoldier, Tilemap campoPropio)
    {
        int placedSoldiers = 0;

        while (placedSoldiers < nSoldier)
        {
            int randomX = Random.Range(0, gWidht);
            int randomY = Random.Range(0, gHeight);
            Vector3Int tilePosition = new Vector3Int(randomX, randomY, 0);

            if (!occupiedPositions.Contains(tilePosition) && campoPropio.HasTile(tilePosition))
            {
                Vector3 worldPosition = campoPropio.GetCellCenterWorld(tilePosition);
                GameObject characterObj = Instantiate(characterPlayer, transform);
                CharManager playerChar = characterObj.GetComponent<CharManager>();
                SpriteRenderer spriteRenderer = characterObj.GetComponent<SpriteRenderer>();

                // Instanciar primero sin definir posición
                //CharManager newSoldier = Instantiate(charManager);
                //charManager.Start(); // Llamar a Start() manualmente para aplicar el sprite

                // Luego asignar la posición
                playerChar.transform.position = worldPosition;

                //calculando el sortingOrder de los sprites para que no den erorres visuales en el isometrico
                int order = ((gHeight - tilePosition.y) * 10) + (gWidht - tilePosition.x);                    
                spriteRenderer.sortingOrder = order;
                playerChar.positionInt = tilePosition;

                //datos del charcter, a cambiar cuando haya mas tipos
                playerChar.positionInt = tilePosition;
                playerChar.colorInt = Random.Range(0, 3);
                playerChar.damage = 1;
                playerChar.health = 1;
                playerChar.modo = 2;
                playerChar.status = 1;
            

                occupiedPositions.Add(tilePosition);
                placedSoldiers++;
            }
        }
    }
}
