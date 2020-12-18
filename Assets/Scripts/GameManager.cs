using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text[] texts = null;

    [SerializeField]
    private GamePlayer playerObj = null;

    [SerializeField]
    private Transform playerParant = null;

    [SerializeField]
    private GameObj objectObj = null;

    [SerializeField]
    private Transform objectParant = null;

    [SerializeField]
    private Generation generation = null;

    private List<GamePlayer> players = new List<GamePlayer>();
    private float score = 0;
    private int maxScore = 0;
    private int gen = 0;

    private float gameSpeed = 1;
    private float objectSpeed = -4;
    private float objectTime = 0;
    private bool first = true;


    private void Update()
    {
        int liveSum = 0;
        for (int j = playerParant.childCount - 1; j > -1; j--)
        {
            if(playerParant.GetChild(j).gameObject.activeSelf)
            {
                liveSum++;
            }
        }

        if (liveSum == 0)
        {
            EndGame();
            Initalize();
        }
        else
        {
            gameSpeed = 1 + (score / 150);

            if(gameSpeed > 2)
            {
                gameSpeed = 2;
            }

            UnityEngine.Time.timeScale = gameSpeed;

            score += (Time.deltaTime * 2);
            objectTime -= Time.deltaTime;

            GameObject objTemp = null;
            float minDistance = 90000f;

            if (objectTime < 1)
            {
                CreateObject();
            }

            for (int i = objectParant.childCount-1; i > -1 ; i--)
            {
                Transform temp = objectParant.GetChild(i);

                temp.localPosition += new Vector3(objectSpeed * gameSpeed, 0f, 0f);

                for (int j = playerParant.childCount - 1; j > -1; j--)
                {
                    if (Vector3.Distance(objectParant.GetChild(i).localPosition, playerParant.GetChild(j).localPosition) < 80) 
                    {
                        playerParant.GetChild(j).gameObject.SetActive(false);
                    }
                }

                ////find object
                if(temp.localPosition.x > -464)
                {
                    if( Mathf.Abs(temp.localPosition.x + 464f) < minDistance)
                    {
                        minDistance = Mathf.Abs(temp.localPosition.x + 464f);
                        objTemp = temp.gameObject;
                    }
                }

                if (temp.localPosition.x < -1000f)
                {
                    DestroyImmediate(temp.gameObject);
                }

            }


            if (objTemp != null)
            {
                double[] input = { 0, 0, 0 };
                input[0] = Mathf.Abs(objTemp.transform.localPosition.x + 464f);

                if (objTemp.transform.localPosition.y > -130)
                {
                    input[1] = 0;
                }
                else
                {
                    input[1] = 1;
                }
                input[2] = gameSpeed;

                for (int j = 0; j < playerParant.childCount; j++)
                {
                    if (playerParant.GetChild(j).gameObject.activeSelf)
                    {
                        generation.Genomes[j].Score = (int)score;
                        double answer = generation.Genomes[j].Answer(input);
                        if (answer > 0.5f)
                        {
                            playerParant.GetChild(j).GetComponent<GamePlayer>().JumpPlayer();
                        }
                    }
                }
            }

            /*
            if (Input.GetKeyDown(KeyCode.Space))
            {
                for (int i = playerParant.childCount - 1; i > -1; i--)
                {
                    playerParant.GetChild(i).GetComponent<GamePlayer>().JumpPlayer();
                }
            }
            */


            //Debug.LogFormat("===========");

            texts[0].text = string.Format("Score : {0}",(int)score);
            texts[1].text = string.Format("MaxScore : {0}", maxScore);
            texts[2].text = string.Format("Gen : {0}", gen);
            texts[3].text = string.Format("Live : {0}", liveSum);
        }
    }

    private void Initalize()
    {
        first = true;

        while (objectParant.childCount > 0)
        {
            DestroyImmediate(objectParant.GetChild(0).gameObject);
        }

        while (playerParant.childCount > 0)
        {
            DestroyImmediate(playerParant.GetChild(0).gameObject);
        }

        //var playerTemp = Instantiate(playerObj, playerParant);
        for (int i = 0; i < 50; i++)
        {
            var playerTemp = Instantiate(playerObj, playerParant);
            playerTemp.transform.localPosition = new Vector3(-464f, -210f, 0f);
            players.Add(playerTemp);
        }
    }
    
    private void EndGame()
    {
        generation.SetBestGenomes();
        generation.Mutations();

        if (maxScore < (int)score)
        {
            maxScore = (int)score;
        }
        score = 0;
        gen++;
    }

    private void CreateObject()
    {
        var ObjectTemp = Instantiate(objectObj, objectParant);

        if (Random.Range(0f, 1f) < 0.5f || first)
        {
            first = false;
            ObjectTemp.transform.localPosition = new Vector3(963f + Random.Range(0f, 20f), -210f + Random.Range(0f, 50f), 0f);
        }
        else
        {
            ObjectTemp.transform.localPosition = new Vector3(963f + Random.Range(0f, 20f), -210f + Random.Range(0f, 250f) + 80f, 0f);
        }

        objectTime = 2;
    }
}
