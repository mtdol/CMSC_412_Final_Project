using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.AI;
           

public class MazeController : MonoBehaviour
{

    public GameObject wall;
    public GameObject ground;
    public GameObject player;
   // public GameObject enemy;
    public GameObject goal;
    public int MazeRow;
    public int MazeCol;
    public System.Random rnd = new System.Random();
    //public NavMeshSurface surface;

    void Start()
    {
        //generate maze and return it as a 2D array 
        int[,] maze = CreateMaze(MazeRow, MazeCol);
      //  surface.BuildNavMesh();
        //respawn player and goal accroding to the maze structure
        RespawnPlayer(maze);
        RespawnGoal(maze);
       // RespawnEnemy(maze);

        

    
    }

    //generate maze as a 2D array , according to given row and columns 
    public int[,] CreateMaze(int r, int c) {
        int[,] maze = new int[r, c];
        
        for (int i = 0; i <= r-1; i++) 
        {
            for (int j = 0; j <= c-1; j++) 
            {
                // outer most walls 
                if (i == r -1  || j == c-1 || i == 0 || j == 0  )
                {
                    maze[i, j] = 1;
                }

                else if (isEven(i) && isEven(j))
                {
                    if (Random.value > 0.1f)
                    {
                        maze[i, j] = 1;

                        float rand = Random.value;

                        // randomly visit neighbor block
                        if (rand < 0.25f )
                        {
                            maze[i,j+1] = 1;
                        } 
                        else if(  rand < 0.5f  )
                        {
                            maze[i,j-1] = 1;
                        }
                        else if ( rand < 0.75f )
                        {
                            maze[i+1,j] = 1;
                        }else{
                            maze[i-1,j] = 1;
                        }
                    }
                }
            }
        }
        
        for (int i = r-1; i >= 0; i--)
        {
            for (int j = 0; j <= c-1; j++)
            {   
                
                // generate ground
                GameObject g = (GameObject)(Instantiate (ground, new Vector3(i,0,j), Quaternion.identity));
                // generate maze wall 
                if (maze[i, j] == 1)
                {
                   GameObject w = (GameObject)(Instantiate (wall, new Vector3(i,1,j), Quaternion.identity));
                }
        }
        }

        return maze;
    }


    public void RespawnPlayer(int[,] maze)
    {
        for (int i = 1; i <= MazeRow-2 ; i++)
        {   
            if(maze[i,1] == 0)
            {
                player.transform.position = new Vector3(i, 0.5f, 1);
                //GameObject p = (GameObject)(Instantiate (player, new Vector3(i,1,1), Quaternion.identity));
                break;
            }
        }

        // for (int i = MazeRow-2 ; i >= 1 ; i--)
        // {   
        //     if(maze[i,1] == 0)
        //     {   
        //         player.transform.position = new Vector3(i,1,MazeCol-2);
        //         //GameObject t = (GameObject)(Instantiate (treasure, new Vector3(i,1,MazeCol-2), Quaternion.identity));
        //         break;
        //     }
        // }

    }

    public void RespawnGoal(int[,] maze)
    {
        for (int i = MazeRow-2 ; i >= 1 ; i--)
        {   
            if(maze[i,1] == 0)
            {   
                goal.transform.position = new Vector3(i,1,MazeCol-2);
                //GameObject t = (GameObject)(Instantiate (treasure, new Vector3(i,1,MazeCol-2), Quaternion.identity));
                break;
            }
        }
    }

    // public void RespawnEnemy(int[,] maze)
    // {
    //     int count = 1; 
    //     //double count = Math.Floor(MazeCol/4.0d);
    //     while(count != 0 ){
            
    //         int row = rnd.Next(2,MazeRow-3);
    //         int col = rnd.Next(2,MazeCol-3);
    //         if(maze[row,col] == 0){
    //             //enemy.transform.position = new Vector3(row,0.5f,col);
    //             GameObject e = (GameObject)(Instantiate (enemy, new Vector3(row,0.5f,col), Quaternion.identity));
    //             count = count - 1;
    //         }
    //     }

       

        
    // }

    private bool isEven(int i)
    { if (i%2 == 0){
        return true;
    }else{
        return false;

    }

    }





    
}