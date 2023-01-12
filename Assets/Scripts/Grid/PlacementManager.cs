using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlacementManager : MonoBehaviour
{
    Direction_e oldDir;
    Direction_e dir;

    public static PlacementManager i;

    float animationTime = 0;
    readonly float animationDuration = .1f;

    bool backgroundTileToggle = false;
    bool backgroundTileDebounce = false;

    public Transform[] Buttons;

    public EditorPage[] pages;
    private int page = 1;

    // Short for Category Name lol
    public Text catName;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        if (GridManager.mode != Mode_e.EDITOR)
        {
            this.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        animationTime += Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.Z) && page > 1)
        {   
            pages[page-1].buttons.SetActive(false);
            page -= 1;
            pages[page-1].buttons.SetActive(true);
            Color tmp = catName.color;
            tmp.a = 1;
            catName.color = tmp;
            catName.text = pages[page-1].category;

        }
        if (Input.GetKeyDown(KeyCode.X) && page < pages.Length)
        {   
            pages[page-1].buttons.SetActive(false);
            page += 1;
            pages[page-1].buttons.SetActive(true);
            Color tmp = catName.color;
            tmp.a = 1;
            catName.color = tmp;
            catName.text = pages[page-1].category;
        }

        #region Animation and rotating
        if (Input.GetKeyDown(KeyCode.Q) && GridManager.tool != Tool_e.SELECT) {
            animationTime = 0;
            if ((int)dir == 0)
            {
                dir = (Direction_e)3;
            }
            else {
                dir = (Direction_e)((int)dir - 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && GridManager.tool != Tool_e.SELECT) {
            animationTime = 0;
            if ((int)dir == 3)
            {
                dir = (Direction_e)0;
            }
            else
            {
                dir = (Direction_e)((int)dir + 1);
            }
        }
        

        foreach (Transform transform in Buttons) {
            if(transform.GetComponent<EditorButtons>().Animate)
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, (int)oldDir * -90), Quaternion.Euler(0, 0, (int)dir * -90), animationTime / animationDuration);
        }

        if (animationTime > animationDuration) {
            animationTime = 0;
            oldDir = dir;
        }
        #endregion

        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        int x = Mathf.FloorToInt(worldPoint.x + .5f);
        int y = Mathf.FloorToInt(worldPoint.y + .5f);

        if (Input.GetMouseButton(0)) {

            if (!GridManager.clean)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (x < 0 || y < 0)
                return;

            if (x >= CellFunctions.gridWidth || y >= CellFunctions.gridHeight)
                return;

            if (GridManager.tool == Tool_e.DRAG || GridManager.tool == Tool_e.SELECT || GridManager.tool == Tool_e.FOLLOW)
                return;

            if (GridManager.tool == Tool_e.PLACEMENT)
            {
                if (!backgroundTileDebounce)
                    backgroundTileToggle = GridManager.instance.tilemap.GetTile(new Vector3Int(x,y,0)) == GridManager.instance.placebleTile;
                backgroundTileDebounce = true;
                GridManager.instance.tilemap.SetTile(new Vector3Int(x,y,0),
                    backgroundTileToggle ? GridManager.instance.backgroundTile : GridManager.instance.placebleTile
                    );
                return;
            }

            if (CellFunctions.cellGrid[x, y] != null)
            {
                if (CellFunctions.cellGrid[x, y].cellType != (CellType_e)GridManager.tool || CellFunctions.cellGrid[x, y].getDirection() != (Direction_e)dir) {
                    if (CellFunctions.cellGrid[x, y].cellType == CellType_e.FALL && (CellType_e)GridManager.tool == CellType_e.FALL) return;
                    if (CellFunctions.cellGrid[x, y].cellType == CellType_e.PLAYER && (CellType_e)GridManager.tool == CellType_e.PLAYER) return;
                    CellFunctions.cellGrid[x, y].Delete(true);
                }
                else return;
            }
            
            AudioManager.i.PlaySound(GameAssets.i.place);
            Cell cell = GridManager.instance.SpawnCell((CellType_e)GridManager.tool, new Vector2(x,y), dir, false);
            GridManager.hasSaved = false;
        }

        if (Input.GetMouseButtonUp(0))
        {   
            if (!GridManager.clean)
                return;
                
            if (x < 0 || y < 0)
                return;

            if (x >= CellFunctions.gridWidth || y >= CellFunctions.gridHeight)
                return;

            if (CellFunctions.cellGrid[x, y] == null)
                return;

            if (GridManager.tool == Tool_e.FOLLOW)
            {
                if (CellFunctions.cellGrid[x, y].cellType == CellType_e.VOID) return;

                for (int xx = 0; xx < CellFunctions.gridWidth; xx++)
                {
                    for (int yy = 0; yy < CellFunctions.gridHeight; yy++)
                    {
                        if (CellFunctions.cellGrid[xx, yy] != null)
                        {
                            if (xx != x || yy != y) CellFunctions.cellGrid[xx, yy].camera = false;
                        }
                    }
                }
                CellFunctions.cellGrid[x, y].camera = !CellFunctions.cellGrid[x, y].camera;
                return;
            }
        }

        if (Input.GetMouseButton(1) && GridManager.tool != Tool_e.SELECT)
        {
            if (!GridManager.clean)
                return;
            if (x < 0 || y < 0)
                return;
            if (x >= CellFunctions.gridWidth || y >= CellFunctions.gridHeight)
                return;

            if (CellFunctions.cellGrid[x, y] != null)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    CellType_e delete = CellFunctions.cellGrid[x, y].cellType;
                    List<Cell> allCells = new List<Cell>();

                    for (int gridX = 0; gridX < CellFunctions.gridWidth; gridX++)
                    {
                        for (int gridY = 0; gridY < CellFunctions.gridHeight; gridY++)
                        {
                            if (CellFunctions.cellGrid[gridX, gridY] != null)
                            {
                                allCells.Add(CellFunctions.cellGrid[gridX, gridY]);
                            }
                        }
                    }

                    foreach (Cell cell in allCells)
                    {
                        if (cell.cellType == delete)
                        {
                            cell.Delete(true);
                        }
                    }
                }
                else
                {
                    CellFunctions.cellGrid[x, y].Delete(true);
                }
                
                AudioManager.i.PlaySound(GameAssets.i.destroy);
                GridManager.hasSaved = false;
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (!GridManager.clean)
                return;
            if (x < 0 || y < 0)
                return;
            if (x >= CellFunctions.gridWidth || y >= CellFunctions.gridHeight)
                return;

            if (CellFunctions.cellGrid[x, y] != null)
            {
                GameObject selectedPage = null;
                GameObject selectedCell = null;
                
                foreach (EditorPage p in pages)
                {
                    p.buttons.active = true;
                }

                foreach (EditorButtons e in FindObjectsOfType(typeof(EditorButtons)))
                {
                    if (CellFunctions.cellGrid[x, y].cellType.ToString() == e.tool.ToString())
                    {
                        e.switchTool();
                        selectedCell = e.gameObject;
                    }
                }

                foreach (EditorPage p in pages)
                {
                    foreach(Transform child in p.buttons.transform)
                    {
                        if (child.gameObject == selectedCell)
                        {
                            selectedPage = p.buttons;
                            while (pages[page-1].buttons != p.buttons)
                            {
                                page = Random.Range(0, pages.Length) + 1;
                            }
                            catName.text = pages[page-1].category;
                        }
                    }

                    if (selectedPage != p.buttons)
                    {
                        p.buttons.SetActive(false);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            backgroundTileDebounce = false;

        }
    }
}
