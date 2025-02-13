using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ArmyConfig : MonoBehaviour
{
    private HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();

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
            
            // Instanciar primero sin definir posición
            CharManager newSoldier = Instantiate(charManager);
             charManager.Start(); // Llamar a Start() manualmente para aplicar el sprite

            // Luego asignar la posición
            newSoldier.transform.position = worldPosition;
            // Asignar colorInt aleatorio y llamar a Start()
           

            occupiedPositions.Add(tilePosition);
            placedSoldiers++;
        }
        }
    }
}
