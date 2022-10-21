using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class villagerMovement : MonoBehaviour
{
    [SerializeField] float speed=2;

    public Animator anim;
    public GameObject workplace;
    public Vector3 housePosition;

    public float lastTime;
    public int steps=0;
    bool isRunning = false;
    timeSystem timeSystem;
    bool isDay=false;
    bool ReturnOpen = false;
    [SerializeField] float rotationSpeed = 90f;
    Cell[,] grid;
    int size=150;
    List<int2> path=new List<int2>();
    int2 target;
    public bool onPath = false;
    public bool arrived = false;
    // Start is called before the first frame update
    bool onWork = false;
    private void Start()
    {
        grid = new Cell[size, size];
        Cell[,] gridOrg = GameObject.FindObjectOfType<Grid>().grid;
        for(int y=0;y<size;y++)
            for (int x = 0; x < size; x++)
            {
                Cell cell = new Cell(gridOrg[x,y]);
                grid[x, y] = cell;
            }

        anim = GetComponent<Animator>();
        timeSystem = Canvas.FindObjectOfType<timeSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        isRunning = false;
        if (workplace == null)
        {
                MoveTo(housePosition);
        }
        else
        {
            ApplyMove();
            if (ReturnOpen) Return();
            if (onWork) Work();
        }
        anim.SetBool("run", isRunning);
        if (Input.GetKeyDown(KeyCode.F))
            TEST();
    }
    private void ApplyMove()
    {
        if (isDay != timeSystem.isDay)
        {
            isDay = timeSystem.isDay;
            onWork = false;
            ReturnOpen = true;
        }
    }
    void Return()
    {
        if (isDay) ReturnWork();
        else ReturnHouse();
    }
    void ReturnHouse()
    {
        MoveTo(housePosition);
        if (arrived) ReturnOpen = false;
    }
    void ReturnWork()
    {
        MoveTo(workplace.transform.position);
        if (arrived)
        {
            ReturnOpen = false;
            onWork = true;
        }

    }
    void Work()
    {
        if (workplace.name.Contains("FarmMill"))
            GetComponent<Farmer>().Work();
        else if (workplace.name.Contains("Builder"))
            GetComponent<Builder>().Work();
        else if (workplace.name.Contains("Flag"))
            GetComponent<Collecter>().Work();
        else if (workplace.name.Contains("LumberMill"))
            GetComponent<Lumber>().Work();
    }

    public void MoveTo(Vector3 targetPosition)
    {
        arrived = false;
        if (!onPath)
        {
            path = GetPathFindingClass().getPath(transform.position, targetPosition);

            if(path.Count<1)   //Could Not Find Any Path
            {
                arrived = true;
                onPath = false;
                return;
            }
            target = path[path.Count - 1];
            path.RemoveAt(path.Count - 1);
            onPath = true;
        }
        else
        {
            if(isArrive(new Vector3(target.x,0,target.y),.1f))
            {
                if (path.Count < 1)
                {
                    arrived = true;
                    onPath = false;
                    return;
                }
                target = path[path.Count-1];
                path.RemoveAt(path.Count - 1);
            }
            else
            {
                Vector3 pos = new Vector3(target.x - (transform.position.x), 0, target.y - (transform.position.z));
                if (Mathf.Abs(pos.x) < .1f) pos.x = 0;
                else pos.x = Mathf.Sign(pos.x);
                if (Mathf.Abs(pos.z) < .1f) pos.z = 0;
                else pos.z = Mathf.Sign(pos.z);
                print(pos);
                move1step(pos);
            }
        }
    }
    //void moveDirect(Vector3 targetPosition)
    //{
    //    Vector3 difference = targetPosition - transform.position;
    //    float horizontal = Mathf.Sign(difference.x);
    //    float vertical = Mathf.Sign(difference.z);

    //    bool anyLinecast = false, isLargeEnough = false, _isWater = false;

    //    anyLinecast = Physics.Linecast(transform.position, new Vector3(transform.position.x + horizontal, 0.5f, transform.position.z));
    //    isLargeEnough = Mathf.Abs(difference.x) > 0.1f;
    //    _isWater = isWater(transform.position.x + horizontal, transform.position.z);

    //    if (!isLargeEnough || _isWater) horizontal = 0;


    //    anyLinecast = Physics.Linecast(transform.position, new Vector3(transform.position.x, 0.5f, transform.position.z + vertical));
    //    isLargeEnough = Mathf.Abs(difference.z) > 0.1f;
    //    _isWater = isWater(transform.position.x, transform.position.z + vertical);

    //    if (!isLargeEnough || _isWater) vertical = 0;

    //    Vector3 Movement = new Vector3(horizontal, 0, vertical);

    //    if (Movement != Vector3.zero)
    //        move1step(Movement);
    //    else if(!isArrive(targetPosition,1))
    //        InitCoastelMovement(targetPosition);
    //}
    
    //Vector3 StartPointOriginal;
    //Vector3 EndPoint;
    //Vector2 firstIndexes = Vector2.one*-1;
    //Vector2 secondIndexes = Vector2.one * -1;
    //Vector2 targetIndex;
    //int c=1;
    //List<Cell> passedCells = new List<Cell>();
    //void InitCoastelMovement(Vector3 targetPosition)
    //{
    //    coastalMovement = true;

    //    StartPointOriginal = transform.position;
    //    EndPoint = targetPosition;
    //    firstIndexes = Vector2.one * -1;
    //    secondIndexes = Vector2.one * -1;

    //    int x = (int)transform.position.x + 75;
    //    int y = (int)transform.position.z + 75;
        
    //    FindStartPointsOf2Dimensions(x - 1, y - 1);
    //    FindStartPointsOf2Dimensions(x - 1, y);
    //    FindStartPointsOf2Dimensions(x - 1, y + 1);
    //    FindStartPointsOf2Dimensions(x, y - 1);
    //    FindStartPointsOf2Dimensions(x, y + 1);
    //    FindStartPointsOf2Dimensions(x + 1, y - 1);
    //    FindStartPointsOf2Dimensions(x + 1, y);
    //    FindStartPointsOf2Dimensions(x + 1, y + 1);

    //    setPassed(x, y);

    //    int a = algorithm(((int)firstIndexes.x), ((int)firstIndexes.y), 0);
    //    int b = algorithm(((int)secondIndexes.x), ((int)secondIndexes.y), 0);

    //    print(" a : " + a + " b : " + b);

    //    if (a < b)
    //    {
    //        targetIndex = firstIndexes;
    //    }
    //    else targetIndex = secondIndexes;

    //    resetCells();

    //    setPassed(x, y);
    //}
    //void MoveCoastel()
    //{
    //    if(isArrive(new Vector3(targetIndex.x-75,0,targetIndex.y-75),.15f))
    //    {
    //        if(c++>100)
    //        {
    //            print("ERROR : COULDN'T FIND ANY VALID PATH");  return;
    //        }
    //        targetIndex = algorithm1step(((int)targetIndex.x), ((int)targetIndex.y), c);
    //        if(targetIndex.x ==-1 || targetIndex.x == -2)
    //        {
    //            coastalMovement = false;
    //            targetIndex = new Vector2();
    //            c = 1;
    //            resetCells();
    //        }
    //    }
    //    else
    //    {
    //        Vector3 pos = new Vector3(targetIndex.x - 75 - (transform.position.x), 0, targetIndex.y - 75- (transform.position.z));
    //        pos.y = 0;
    //        if (Mathf.Abs(pos.x) < .1f) pos.x = 0;
    //        else pos.x = Mathf.Sign(pos.x);
    //        if (Mathf.Abs(pos.z) < .1f) pos.z = 0;
    //        else pos.z = Mathf.Sign(pos.z);
    //        move1step(pos);
    //    }
    //}
    private void move1step(Vector3 pos)
    {
        isRunning = true;
        transform.Translate(pos*Time.deltaTime*speed,Space.World);
        rotate(pos.x, pos.z);
    }
    public bool isArrive(Vector3 target,float range=.2f)
    {
        float x = (target - transform.position).x;
        float y = (target - transform.position).z;
        return Mathf.Abs(x) < range && Mathf.Abs(y) < range;
    }
    void rotate(float horizontal,float vertical)
    {
        Quaternion rotate = Quaternion.LookRotation(new Vector3(horizontal, 0, vertical),Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation,rotate,rotationSpeed*Time.deltaTime);
    }
    public bool setWorkplace(GameObject work)
    {
        if(!work.name.Contains("Flag"))
        {
            Collecter cl;
            bool b=TryGetComponent<Collecter>(out cl);
            if (b) Destroy(cl);
        }
        bool flag = workplace == work;
        workplace = work;
        return flag;
    }

    
    //void FindStartPointsOf2Dimensions(int x, int y)
    //{
    //    if (isCoast(x,y))
    //    {
    //        if(firstIndexes.x==-1)
    //        {
    //            firstIndexes = new Vector2(x, y);
    //        }
    //        else if(secondIndexes.x==-1)
    //        {
    //            secondIndexes = new Vector2(x, y);
    //        }
    //    }
    //}
    //int algorithm(int x, int y, int count)
    //{
    //    if (count > 100)
    //    {
    //        return 999;
    //    }
    //    else if (isArriveToLine(x, y) && count > 2)
    //    {
    //        return 1;
    //    }
    //    setPassed(x, y);
        
    //    if (isCoast(x - 1, y - 1))
    //        return 1 + algorithm(x - 1, y - 1, count + 1);
    //    else if (isCoast(x - 1, y + 1))
    //        return 1 + algorithm(x - 1, y + 1, count + 1);
    //    else if (isCoast(x + 1, y - 1))
    //        return 1 + algorithm(x + 1, y - 1, count + 1);
    //    else if (isCoast(x + 1, y + 1))
    //        return 1 + algorithm(x + 1, y + 1, count + 1);
    //    else if (isCoast(x, y - 1))
    //        return 1 + algorithm(x, y - 1, count + 1);
    //    else if (isCoast(x, y + 1))
    //        return 1 + algorithm(x, y + 1, count + 1);
    //    else if (isCoast(x - 1, y))
    //        return 1 + algorithm(x - 1, y, count + 1);
    //    else if (isCoast(x + 1, y))
    //        return 1 + algorithm(x + 1, y, count + 1);
    //    else return 0;
    //}

    //public Vector2 algorithm1step(int x, int y, int count)
    //{
    //    if (isArriveToLine(x, y) && count > 2)
    //    {
    //        return -1 * Vector2.one;
    //    }
    //    setPassed(x, y);
    //    Vector2 result;
        
    //    if (isCoast(x - 1, y - 1))
    //        result = new Vector2(x - 1, y - 1);
    //    else if (isCoast(x - 1, y + 1))
    //        result = new Vector2(x - 1, y + 1);
    //    else if (isCoast(x + 1, y - 1))
    //        result = new Vector2(x + 1, y - 1);
    //    else if (isCoast(x + 1, y + 1))
    //        result = new Vector2(x + 1, y + 1);
    //    else if (isCoast(x, y - 1))
    //        result = new Vector2(x, y - 1);
    //    else if (isCoast(x, y + 1))
    //        result = new Vector2(x, y + 1);
    //    else if (isCoast(x - 1, y))
    //        result = new Vector2(x - 1, y);
    //    else if (isCoast(x + 1, y))
    //        result = new Vector2(x + 1, y);
    //    else result = Vector2.one * -2;

    //    return result;
    //}

    //bool isArriveToLine(int xIndex, int yIndex)
    //{
    //    xIndex -= 75; yIndex -= 75; // INDEX TO WORLD POSITION
    //    float distance = UnityEditor.HandleUtility.DistancePointLine(new Vector3(xIndex, 0, yIndex), StartPointOriginal, EndPoint);
    //    return distance < 1f;
    //}
    //bool isCoast(int x, int y)
    //{
    //    return !grid[x, y].isPassed && grid[x, y].isCoast;
    //}
    //void setPassed(float x,float y)
    //{
    //    try
    //    {
    //        grid[((int)x), ((int)y)].isPassed = true;
    //    }
    //    catch (MissingComponentException) { }
    //    passedCells.Add(grid[((int)x), ((int)y)]);
    //}
    //void resetCells()
    //{
    //    try
    //    {
    //        foreach (var cell in passedCells)
    //            cell.isPassed = false;
    //        passedCells.Clear();
    //    }catch(MissingComponentException) { }
    //}
    public void TEST()
    {
        int count = 0;
        for (int y = 0; y < 150; y++)
        {
            for (int x = 0; x < 150; x++)
            {
                if (grid[x, y].isPassed) count++;
            }
        }
        print("TEST 0 RESULT :: COUNT = " + count);
    }


 
    public bool isWater(float x, float y)
    {
        int index1 = (int)(x + size / 2);
        int index2 = (int)(y + size / 2);

        return grid[index1, index2].isWater;
    }
    
    PathFinding GetPathFindingClass()
    {
        PathFinding pathf;
        if (TryGetComponent<PathFinding>(out pathf))
            return pathf;
        else gameObject.AddComponent<PathFinding>();
        if (TryGetComponent<PathFinding>(out pathf))
            return pathf;
        else return null;
        
    }
}
