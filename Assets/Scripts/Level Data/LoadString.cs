using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

namespace load
{
    public static class LoadString
    {
        private const string cellKey = "0123456789abcdefgh⒜⒝⒞⒟⒠⒡⒢⒣⒤⒥⒦⒧⒨⒩⒪⒫⒬⒭⒮⒯⒰⒱⒲⒳⒴⒵<>[]ijklmnopqrstuvwxyz⓪①②③④⑤⑥⑦⑧⑨⑩⑪⑫⑬⑭⑮⑯⑰⑱⑲⑳㉑㉒㉓㉔㉕㉖㉗㉘㉙ABCDEFGHIJKLMNOPQRⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏ#@~`STUVWXYZ!$%&+-.=?^ⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ*_¯˜{}";
        private const string cellKeyNew = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@$%&+=?^/#";
        private static Dictionary<char, int> decode = new Dictionary<char, int>();
        private static bool debounce;

        private static void initDict()
        {
            for (int i = 0; i < 194; i++)
            {
                decode.Add(cellKey[i], i);
            }
        }

        private static int DecodeString(string str)
        {
            int output = 0;
            foreach (char c in str)
            {
                output *= 194;
                output += decode[c];
            }
            return output;
        }

        private static int DecodeNew(char c)
        {
            int output = 0;
            int dataIndex = 0;

            while (dataIndex < cellKeyNew.Length)
            {
                if (cellKeyNew[dataIndex] == c)
                {
                    output = dataIndex;
                    break;
                }
                dataIndex++;
            }

            return output;
        }


        private static void SetCell(int c, int i)
        {
            //c is celldata index, i is level position index
            if (c % 2 == 1)
                GridManager.instance.tilemap.SetTile(new Vector3Int(i % CellFunctions.gridWidth, i / CellFunctions.gridWidth, 0), GridManager.instance.placebleTile);
            if (c >= 192)
                return;
            GridManager.instance.SpawnCell(
                (CellType_e)((c / 2) % 24),
                new Vector2(i % CellFunctions.gridWidth, i / CellFunctions.gridWidth),
                (Direction_e)(c / 48),
                false);
        }
        
        private static void SetCellNew(int c, int i)
        {
            int cellType = 0;
            int dir = c;
            while (dir > 3)
            {
                dir -= 4;
                cellType += 1;
            }

            GridManager.instance.SpawnCell(
                (CellType_e)(cellType),
                new Vector2(i % CellFunctions.gridWidth, i / CellFunctions.gridWidth),
                (Direction_e)(dir),
                false);
        }

        public static bool Load(string str)
        {
            if (!debounce)
            {
                initDict();
                debounce = true;
            }

            string[] arguments = str.Split(';');

            string levelName = "";
            string tutorialText = "";

            //used in V2/V3/V4
            int length;
            int dataIndex = 0;
            int gridIndex = 0;
            string temp;

            switch (arguments[0])
            {

                case "V1":
                    CellFunctions.gridWidth = int.Parse(arguments[1]);
                    CellFunctions.gridHeight = int.Parse(arguments[2]);
                    GridManager.instance.InitGridSize();

                    string[] placementCellLocationsStr = arguments[3].Split(',');
                    if (placementCellLocationsStr[0] != "")
                        foreach (string String in placementCellLocationsStr)
                        {
                            int x = int.Parse(String.Split('.')[0]);
                            int y = int.Parse(String.Split('.')[1]);
                            GridManager.instance.tilemap.SetTile(new Vector3Int(x, y, 0), GridManager.instance.placebleTile);
                        }

                    string[] cellStr = arguments[4].Split(',');
                    if (cellStr[0] != "")
                        foreach (string String in cellStr)
                        {
                            GridManager.instance.SpawnCell(
                                (CellType_e)int.Parse(String.Split('.')[0]),
                                new Vector2(
                                    int.Parse(String.Split('.')[2]),
                                    int.Parse(String.Split('.')[3])),
                                (Direction_e)int.Parse(String.Split('.')[1]),
                                false);
                        }

                    tutorialText = arguments[5];
                    levelName = arguments[6];
                    break;

                case "V3":
                    CellFunctions.gridWidth = DecodeString(arguments[1]);
                    CellFunctions.gridHeight = DecodeString(arguments[2]);
                    GridManager.instance.InitGridSize();
                    int[] cellDataHistory = new int[CellFunctions.gridWidth * CellFunctions.gridHeight];
                    int offset;

                    while (dataIndex < arguments[3].Length)
                    {
                        if (arguments[3][dataIndex] == ')' || arguments[3][dataIndex] == '(')
                        {
                            if (arguments[3][dataIndex] == ')')
                            {
                                dataIndex += 2;
                                offset = decode[arguments[3][dataIndex - 1]];
                                length = decode[arguments[3][dataIndex]];

                            }
                            else
                            {
                                dataIndex++;
                                temp = "";
                                while (arguments[3][dataIndex] != ')' && arguments[3][dataIndex] != '(')
                                {
                                    temp += arguments[3][dataIndex];
                                    dataIndex++;
                                }
                                offset = DecodeString(temp);
                                if (arguments[3][dataIndex] == ')')
                                {
                                    dataIndex++;
                                    length = decode[arguments[3][dataIndex]];
                                }
                                else
                                {
                                    dataIndex++;
                                    temp = "";
                                    while (arguments[3][dataIndex] != ')')
                                    {
                                        temp += arguments[3][dataIndex];
                                        dataIndex++;
                                    }
                                    length = DecodeString(temp);
                                }
                            }
                            for (int i = 0; i < length; i++)
                            {
                                SetCell(cellDataHistory[gridIndex - offset - 1], gridIndex);
                                cellDataHistory[gridIndex] = cellDataHistory[gridIndex - offset - 1];
                                gridIndex++;
                            }
                        }
                        else
                        {
                            SetCell(decode[arguments[3][dataIndex]], gridIndex);
                            cellDataHistory[gridIndex] = decode[arguments[3][dataIndex]];
                            gridIndex++;
                        }

                        dataIndex++;
                    }

                    tutorialText = arguments[4];
                    levelName = arguments[5];
                    break;

                case "MP1":

                    while (dataIndex < arguments.Length)
                    {
                        string indString = arguments[dataIndex];

                        indString = indString.Replace("}", "))))))))))");
                        indString = indString.Replace(")", "{{{{{{{{{{");
                        indString = indString.Replace("{", "((((((((((");
                        indString = indString.Replace("(", "!!!!!!!!!!");
                        indString = indString.Replace(",", "..");
                        indString = indString.Replace("'", "...");
                        indString = indString.Replace("!", "....");

                        arguments[dataIndex] = indString;

                        dataIndex++;
                    }

                    dataIndex = 0;
                    int dots = 0;

                    bool placeable = false;
                    bool looping = false;
                    bool camera = false;
                    int prevCell = -1;
                    
                    while (dataIndex < arguments[1].Length)
                    {
                        if (arguments[1][dataIndex] == '.')
                        {
                            dots++;
                        }
                        else
                        {
                            CellFunctions.gridWidth = DecodeNew(arguments[1][dataIndex]) + ( dots * 72 );
                        }

                        dataIndex++;
                    }

                    dots = 0;
                    dataIndex = 0;

                    while (dataIndex < arguments[2].Length)
                    {
                        if (arguments[2][dataIndex] == '.')
                        {
                            dots++;
                        }
                        else
                        {
                            CellFunctions.gridHeight = DecodeNew(arguments[2][dataIndex]) + ( dots * 72 );
                        }

                        dataIndex++;
                    }

                    GridManager.instance.InitGridSize();

                    dots = 0;
                    dataIndex = 0;

                    while (dataIndex < arguments[3].Length)
                    {
                        if (arguments[3][dataIndex] == '.')
                        {
                            dots++;
                        }
                        else if (arguments[3][dataIndex] == ':')
                        {
                            looping = true;
                        }
                        else if (arguments[3][dataIndex] == '[')
                        {
                            placeable = true;
                        }
                        else if (arguments[3][dataIndex] == ']')
                        {
                            placeable = false;
                        }
                        else if (arguments[3][dataIndex] == '-')
                        {
                            prevCell = -1;
                            if (placeable == true)
                                GridManager.instance.tilemap.SetTile(new Vector3Int(gridIndex % CellFunctions.gridWidth, gridIndex / CellFunctions.gridWidth, 0), GridManager.instance.placebleTile);
                            gridIndex++;
                        }
                        else
                        {
                            if(looping == false)
                            {
                                int cellId = DecodeNew(arguments[3][dataIndex]) + ( dots * 72 );
                                prevCell = cellId;
                                SetCellNew(cellId, gridIndex);
                                
                                if (placeable == true)
                                    GridManager.instance.tilemap.SetTile(new Vector3Int(gridIndex % CellFunctions.gridWidth, gridIndex / CellFunctions.gridWidth, 0), GridManager.instance.placebleTile);

                                gridIndex++;
                                dots = 0;
                            }
                            else
                            {
                                int loopTimes = DecodeNew(arguments[3][dataIndex]) + ( dots * 72 );

                                while (loopTimes != 0)
                                {
                                    if (prevCell != -1)
                                        SetCellNew(prevCell, gridIndex);
                                
                                    if (placeable == true)
                                        GridManager.instance.tilemap.SetTile(new Vector3Int(gridIndex % CellFunctions.gridWidth, gridIndex / CellFunctions.gridWidth, 0), GridManager.instance.placebleTile);

                                    gridIndex++;
                                    loopTimes -= 1;
                                }

                                dots = 0;
                                loopTimes = 0;
                                looping = false;
                            }
                        }

                        dataIndex += 1;
                    }

                    
                    tutorialText = arguments[4];
                    levelName = arguments[5];
                    break;
            }
            GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>().text = tutorialText;
            return true;
        }
    }
}
