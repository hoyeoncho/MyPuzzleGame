using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3 : MonoBehaviour
{
    public ArrayLayout boardLayout;

    [Header("UI Elements")]
    public Sprite[] pieces;
    public RectTransform gameBoard;
    
    [Header("Prefabs")]
    public GameObject nodePiece;

    int width = 5;
    int height = 5;
    Node[,] board;

    System.Random random;

    public void OnMouseDown()
    {
        
    }

    void Start()
    {
        StartGame();  
    }

    void StartGame()
    {

        string seed = getRandomSeed();
        //랜덤으로 담은 길이 20의 문자열의 해시코드를 받는다. 
        random = new System.Random(seed.GetHashCode());

        InitializeBoard();
        VerifyBoard();
        InstantiateBoard();
    }

    void InitializeBoard()
    {
        board = new Node[width, height];
        for(int y=0; y< height; y++)
        {
            for(int x= 0; x< width; x++)
            {
                //저거 삼항연산자 rows[y].rows[x] 가 참 거짓이 어케 나오지? 저 부분 이해가 잘 안됨
                board[x, y] = new Node((boardLayout.rows[y].row[x]) ? - 1 : fillPiece(), new Point(x, y));
            }
        }
    }

    //시작하자마자 3이상 매치된 타일들이 생성되는지 확인하는 함수 
    void VerifyBoard()
    {
        List<int> remove;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Point p = new Point(x, y);
                int val = getValueAtPoint(p);
                if (val <= 0) continue;

                remove = new List<int>();
                while(isConnected(p,true).Count>0)
                {
                    val = getValueAtPoint(p);
                    if (!remove.Contains(val))
                        remove.Add(val);
                    setValueAtPoint(p, newValue(ref remove));
                }

            }
        }
    }

    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int val = board[x, y].value;
                if (val <= 0) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                NodePiece node = p.GetComponent<NodePiece>();
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(50 + (100 * x), -50 - (100 * y));
                node.initialize(val, new Point(x, y), pieces[val - 1]);
            }
        }
    }

    List<Point> isConnected(Point p, bool main)
    {
        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(p);
        // 방향 순서별로 넣어주는거 지켜야 됨
        Point[] directions = { Point.up, Point.right, Point.down, Point.left };

        //방향에 2개 혹은 그 이상의 같은타입이 있는지 체크 
        foreach(Point dir in directions)
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for(int i=1; i< 3; i++) // 두번 
            {
                Point check = Point.add(p, Point.mult(dir, i));
                if (getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }

            if (same > 1) // 2이상의 같은 형태가 있는 상황이면 
                AddPoints(ref connected, line);            
        }

        // 두개의 같은 속성 사이에 껴있는 경우도 체크해준다. 
        for(int i=0; i< 2;i++)
        {
            List<Point> line = new List<Point>();
            int same = 0;
            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[i + 2])};
            //밑에 i+2 는 directions 배열 안에 차례 바뀌면 안통한다. 
            
            foreach(Point next in check)
            {
                if (getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                } 
            }
             
            if (same > 1) // 2이상의 같은 형태가 있는 상황이면 
                AddPoints(ref connected, line);
        }

        //이 밑부분은 다시한번 보고 이해하자. 지금이해가 안댐 check for a 2x2
        for(int i =0; i<4; i++)
        {
            List<Point> square = new List<Point>();

            int same = 0;
            int next = i + 1;
            if (next >= 4)
                next -= 4;

            Point[] check = 
            {
                Point.add(p, directions[i]), 
                Point.add(p, directions[next]), 
                Point.add(p, Point.add(directions[i], 
                directions[next])) 
            };

            foreach (Point pnt in check)
            {
                if (getValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }

            if (same > 2)
                AddPoints(ref connected, square);
        }

        if(main) // checks for other  matches along  current match
        {
            for(int i =0; i<connected.Count; i++)
            {
                AddPoints(ref connected, isConnected(connected[i], false));
            }
        }

        if (connected.Count > 0)
            connected.Add(p);

        return connected;
    }

    void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach(Point p in add)
        {
            bool doAdd = true;
            for(int i= 0; i< points.Count;i++)
            {
                if(points[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }
            }

            if (doAdd) points.Add(p);
        }
    }

    //보드의 해당 포인트의 속성(무슨색인지 벨류) 가져오기 
    int getValueAtPoint(Point p)
    {
        if (p.x < 0 || p.x >= width || p.y < 0 || p.y >= height) return -1;
        return board[p.x, p.y].value;
    }

    void setValueAtPoint(Point p, int v)
    {
        board[p.x, p.y].value = v;
    }


    //0 만들어서 리턴할건데 왜 굳이 위에 바로안쓰고 여기에 함수로 만들었는지 이해 안됨 
    int fillPiece()
    {
        int val = 1;
        //random.Next 0~99 중에 숫자 하나 나오는건데 range랑 다를거없음
        //아래공식으로 val은 무조건 스프라이트갯수 값중에 하나 나옴 
        val = (random.Next(0, 100) / (100 / pieces.Length)) + 1;
        return val;
    }

    int newValue(ref List<int> remove)
    {
        List<int> avaliable = new List<int>();
        for(int i=0; i<pieces.Length; i++)
            avaliable.Add(i + 1);
        foreach (int i in remove)
            avaliable.Remove(i);

        if (avaliable.Count <= 0) return 0;
        return avaliable[random.Next(0, avaliable.Count)]; 

    }

    void Update()
    {
        
    }

    //처음 제네레이트 할때 해쉬코드 만들어서 사용할거임
    string getRandomSeed()
    {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstovwxyz1234567890!@#$%^&*()";
        for(int i=0;i<20;i++)
        {
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
        }
        return seed;
    }
}

[System.Serializable]
public class Node
{
    public int value; // 0 = 빈노드, 1 = 파랑, 2 = 빨강, 3 = 보라, 4 = 초록, 5 = 노랑, -1 = 구멍
    public Point index;

    public Node(int v, Point i)
    {
        value = v;
        index = i; 
    }
}