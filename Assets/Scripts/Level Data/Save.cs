using System;
using System.Text;
using UnityEngine;

// welcome to this absolute mess of a code
public class Save : MonoBehaviour
{
    public GameObject saveText;
    private const string cellKey = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@$%&+=?^/#";
    public GameObject canvas;

    public void Awake()
    {
        saveText.gameObject.SetActive(false);
    }

    private string EncodeInt(int num)
    {

        // a
        // .a
        // ,a
        // 'a
        // !a
        // (a
        // {a
        // )a
        // }a

        if (num < 72)
            return cellKey[num].ToString();

        int varNum = num;
        string output = "";

        while (varNum > 71)
        {
            output += ".";
            varNum -= 72;
        }
        
        output = output.Replace("....", "!");
        output = output.Replace("...", "'");
        output = output.Replace("..", ",");
        output = output.Replace("!!!!!!!!!!", "(");
        output = output.Replace("((((((((((", "{");
        output = output.Replace("{{{{{{{{{{", ")");
        output = output.Replace("))))))))))", "}");

        output += cellKey[varNum].ToString();
        return output;
    }

    public void SaveString() {
        SaveString(new Vector2Int(0, CellFunctions.gridHeight - 1), new Vector2Int(CellFunctions.gridWidth - 1, 0));
    }

    public void SaveString(Vector2Int topLeft, Vector2Int bottomRight)
    {
        StringBuilder output = new StringBuilder();

        switch (PlayerPrefs.GetInt("ExportFormat", 2))
        {
            // Save as V1 code
            case 1:
                output.Append("V1;");
                output.Append(((bottomRight.x + 1) - topLeft.x) + ";" + ((topLeft.y + 1) - bottomRight.y) + ";");
                
                bool debounce = false;
                for (int y = bottomRight.y; y <= topLeft.y; y++)
                {
                    for (int x = topLeft.x; x <= bottomRight.x; x++)
                    {
                        if (GridManager.instance.tilemap.GetTile(new Vector3Int(x, y, 0)) == GridManager.instance.placebleTile)
                        {
                            if (debounce)
                                output.Append(",");
                            debounce = true;
                            output.Append((x - topLeft.x) + "." + (y - bottomRight.y));
                        }
                    }
                }
                output.Append(";");

                debounce = false;
                foreach (Cell cell in CellFunctions.cellList)
                {
                    if (debounce)
                        output.Append(",");
                    debounce = true;
                    output.Append((int)cell.cellType + "." + (int)cell.spawnRotation + "." + (int)(cell.spawnPosition.x - topLeft.x) + "." + (int)(cell.spawnPosition.y - bottomRight.y));
                }
                break;

            // Save as MP1 code
            default:

                output.Append("MP1;");

                output.Append(EncodeInt(((bottomRight.x + 1) - topLeft.x)) + ";" + EncodeInt(((topLeft.y + 1) - bottomRight.y)) + ";");

                int typeLength = Enum.GetNames(typeof(CellType_e)).Length;
                int dirLength = Enum.GetNames(typeof(Direction_e)).Length;

                int[] cellData;
                int dataIndex = 0;

                bool placeable = false;
                
                bool looping = false;
                int repeats = 0;

                int prevCell = -1;

                cellData = new int[((bottomRight.x + 1) - topLeft.x) * ((topLeft.y + 1) - bottomRight.y)];

                for (int y = bottomRight.y; y <= topLeft.y; y++)
                {
                    for (int x = topLeft.x; x <= bottomRight.x; x++)
                    {
                        cellData[(x - topLeft.x) + ((y - bottomRight.y) * (bottomRight.x + 1 - topLeft.x))] = GridManager.instance.tilemap.GetTile(new Vector3Int(x, y, 0))
                        == GridManager.instance.placebleTile ?
                        typeLength * dirLength + 666 : -1;
                    }
                }
                foreach (Cell cell in CellFunctions.cellList)
                {
                    cellData[(int)(cell.spawnPosition.x - topLeft.x) + ((int)(cell.spawnPosition.y - bottomRight.y) * ((bottomRight.x + 1) - topLeft.x))]
                    += ( (int)cell.getDirection() + ( 4 * (int)cell.cellType ) + 1 );
                }

                while (dataIndex < cellData.Length)
                {
                    int thisCell = cellData[dataIndex];

                    if (looping == true)
                    {
                        if (cellData[dataIndex] != prevCell)
                        {
                            looping = false;
                            output.Append(EncodeInt(repeats));
                            repeats = 0;
                        }
                        else
                        {
                            repeats++;
                            goto Next;
                        }
                    }

                    if (looping == false && cellData[dataIndex] == prevCell)
                    {
                        output.Append(":");
                        repeats++;
                        looping = true;
                        goto Next;
                    }

                    if (cellData[dataIndex] > typeLength * dirLength + 665)
                    {
                        if (placeable == false)
                        {
                            output.Append("[");
                            placeable = true;
                        }
                        cellData[dataIndex] -= ( typeLength * dirLength + 667 );
                    }
                    else
                    {
                        if(placeable == true)
                        {
                            output.Append("]");
                            placeable = !placeable;
                        }
                    }

                    if (cellData[dataIndex] == -1)
                    {
                        output.Append("-");
                    }
                    else
                    {
                        output.Append(EncodeInt(cellData[dataIndex]));
                    }
                    
                    prevCell = thisCell;

                    Next:
                        dataIndex += 1;
                }

                if(placeable == true)
                {
                    output.Append("]");
                    placeable = !placeable;
                }

                if (looping == true)
                {
                    looping = false;
                    output.Append(EncodeInt(repeats));
                }

                cellData = null;

                break;
        }
        output.Append(";;");

        GridManager.hasSaved = true;
        GUIUtility.systemCopyBuffer = output.ToString();
        GameObject go = Instantiate(saveText, canvas.GetComponent<Transform>(), true);
        go.SetActive(true);
        Destroy(go, 3);
    }
}
