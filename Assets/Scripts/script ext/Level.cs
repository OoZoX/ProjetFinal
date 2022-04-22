
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<GameObject> IATankList;
    [SerializeField] private List<GameObject> PlayerTankList;
    [SerializeField] public List<CaptureZone> ZoneCaptureList;
    [SerializeField] private List<CollectableItem> CollectableItemList;
    [SerializeField] private GameObject IATeam;
    [SerializeField] private GameObject PlayerTeam;
    [SerializeField] private int MaxTimeGame;


    private List<Vector3> SpawnIA;
    private List<Vector3> SpawnPlayer;

  
    private int NbTotalIATank;
    private int NbTotalPlayerTank;
    public int NbCaptureZone;
    public int score;
    private float IARespawnCooldown;
    private float PlayerRespawnCooldown;

    [SerializeField] private float IARespawnRate;
    [SerializeField] private float PlayerRespawnRate;
    public static Level Instance;
    public  Stopwatch _GameTimer = new Stopwatch();
    
    private int rnd;
    public enum StateLevel
    {
        InGame,
        Victory,
        Defeat,
    }
    private enum StateRespawn
    {
        Full,
        Checking,
        WaitingForRespawn
    }
    public StateLevel _StateLevel;
    private StateRespawn _StatePlayerTeam;
    private StateRespawn _StateIATeam;
    public GameObject TankPlayers;
    public GameObject TankIAs;


    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        score = 0;
        SpawnIA = new List<Vector3>();
        SpawnPlayer = new List<Vector3>();
        foreach (GameObject tank in IATankList)
        {
            SpawnIA.Add(tank.gameObject.transform.position);
        }        
        foreach (GameObject tank in PlayerTankList)
        {
            SpawnPlayer.Add(tank.gameObject.transform.position);
        }
        NbTotalIATank = IATankList.Count();
        NbTotalPlayerTank = PlayerTankList.Count();
        NbCaptureZone = ZoneCaptureList.Count();
        _StateLevel = StateLevel.InGame;
        _StatePlayerTeam = StateRespawn.Full;
        _StateIATeam = StateRespawn.Full;
        _GameTimer.Start();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        EndGameVerification();
        CheckLists();
    }
    public void CheckLists()
    {
        foreach (GameObject tank in PlayerTankList)
        {
            if (tank.GetComponentInChildren<Tank>()._UITank.gameObject.active == false)
            {
                UnityEngine.Debug.Log("score " + score);
                score = score - 1;
                PlayerTankList.Remove(tank);
                _StatePlayerTeam = StateRespawn.WaitingForRespawn;
                StartCoroutine(ManagePlayerTankRespawn());
            }
        }
        foreach (GameObject tank in IATankList)
        {
            if (tank.GetComponentInChildren<Tank>()._UITank.gameObject.active == false)
            {
                UnityEngine.Debug.Log("score " + score);
                score = score + 1;
                IATankList.Remove(tank);
                _StateIATeam = StateRespawn.WaitingForRespawn;
                StartCoroutine(ManageIATankRespawn());
            }
        }
    }


    public IEnumerator ManagePlayerTankRespawn()
    {

        while (PlayerTankList.Count() < NbTotalPlayerTank)
        {
            if (PlayerRespawnRate <= PlayerRespawnCooldown)
            {

                rnd = Random.Range(0, NbTotalPlayerTank);
                GameObject NewPlayerTank = Instantiate(TankPlayers, SpawnPlayer[rnd], Quaternion.Euler(0, 0, 0));
                NewPlayerTank.transform.parent = PlayerTeam.transform;
                NewPlayerTank.transform.position = SpawnPlayer[rnd];
                PlayerTankList.Add(NewPlayerTank);
                _StatePlayerTeam = StateRespawn.Checking;
                PlayerRespawnCooldown = 0;
            }
            else
            {
                PlayerRespawnCooldown = PlayerRespawnCooldown + Time.deltaTime;
            }
            yield return null;
        }
    }

    public IEnumerator ManageIATankRespawn()
    {

        while (IATankList.Count() < NbTotalIATank)
        {
            if (IARespawnRate <= IARespawnCooldown)
            {

                rnd = Random.Range(0, NbTotalIATank);
                GameObject NewIATank = Instantiate(TankIAs, SpawnIA[rnd], Quaternion.Euler(0, 0, 0));
                NewIATank.transform.parent = IATeam.transform;
                NewIATank.transform.position = SpawnIA[rnd];
                IATankList.Add(NewIATank);
                _StateIATeam = StateRespawn.Checking;
                IARespawnCooldown = 0;
            }
            else
            {
                IARespawnCooldown = IARespawnCooldown + Time.deltaTime;
            }
            yield return null;
        }
    }
    public void EndGameVerification()
    {
        int nbZone = 0;
        foreach(CaptureZone captureZone in ZoneCaptureList)
        {         
            if(captureZone._ZoneState == CaptureZone.ZoneState.CapturedPlayer)
            {
                nbZone++;
            }
            else
            {
                _StateLevel = StateLevel.InGame;
            }
            if(captureZone._ZoneState == CaptureZone.ZoneState.CapturedAI) 
            {
                nbZone--;
            }
            else
            {
                _StateLevel = StateLevel.InGame;
            }
        }
        if(nbZone == 3)
        {
            _StateLevel = StateLevel.Victory;
        }
        else if(nbZone == -3)
        {
            _StateLevel = StateLevel.Defeat;
        }
        
        if(_GameTimer.Elapsed.TotalSeconds > MaxTimeGame)
        {
            _StateLevel = StateLevel.Defeat;
        }
        if (-5 > score)
        {
            _StateLevel = StateLevel.Defeat;
        }
        //UnityEngine.Debug.Log("_StateLevel " + _StateLevel);
    }
}
