
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private List<IATank> IATankList;
    [SerializeField] private List<PlayerTank> PlayerTankList;
    [SerializeField] private List<CaptureZone> ZoneCaptureList;
    [SerializeField] private List<CollectableItem> CollectableItemList;
    [SerializeField] private GameObject IATeam;
    [SerializeField] private GameObject PlayerTeam;
    [SerializeField] private int MaxTimeGame;


    private List<Vector3> SpawnIA;
    private List<Vector3> SpawnPlayer;

  
    private int NbTotalIATank;
    private int NbTotalPlayerTank;
    private int NbCaptureZone;

    private float IARespawnCooldown;
    private float PlayerRespawnCooldown;

    [SerializeField] private float IARespawnRate;
    [SerializeField] private float PlayerRespawnRate;

    private Stopwatch _GameTimer = new Stopwatch();
    
    private int rnd;
    private enum StateLevel
    {
        InGame,
        Victory,
        Defeat,
        OutofTime
    }
    private enum StateRespawn
    {
        Full,
        Checking,
        WaitingForRespawn
    }
    private StateLevel _StateLevel;
    private StateRespawn _StatePlayerTeam;
    private StateRespawn _StateIATeam;
    public PlayerTank TankPlayers;
    public IATank TankIAs;
    

    // Start is called before the first frame update
    void Start()
    {   
        SpawnIA = new List<Vector3>();
        SpawnPlayer = new List<Vector3>();
        foreach (IATank tank in IATankList)
        {
            SpawnIA.Add(tank.gameObject.transform.position);
        }        
        foreach (PlayerTank tank in PlayerTankList)
        {
            SpawnPlayer.Add(tank.gameObject.transform.position);
        }
        NbTotalIATank = IATankList.Count();
        NbTotalPlayerTank = PlayerTankList.Count();
        _StateLevel = StateLevel.InGame;
        _StatePlayerTeam = StateRespawn.Full;
        _StateIATeam = StateRespawn.Full;
        _GameTimer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        EndGameVerification();
        CheckLists();
    }
    public void CheckLists()
    {
        foreach (PlayerTank tank in PlayerTankList)
        {
            if (tank.gameObject.active == false)
            {
                PlayerTankList.Remove(tank);
                _StatePlayerTeam = StateRespawn.WaitingForRespawn;
                StartCoroutine(ManagePlayerTankRespawn());
            }
        }
        foreach (IATank tank in IATankList)
        {
            if (tank.gameObject.active == false)
            {
                IATankList.Remove(tank);
                _StateIATeam = StateRespawn.WaitingForRespawn;
                StartCoroutine(ManageIATankRespawn());
            }
        }
    }


    public IEnumerator ManagePlayerTankRespawn()
    {

        while (_StatePlayerTeam == StateRespawn.WaitingForRespawn )
        {
            if (PlayerRespawnRate <= PlayerRespawnCooldown)
            {

                rnd = Random.Range(0, NbTotalPlayerTank);
                PlayerTank NewPlayerTank = Instantiate(TankPlayers, SpawnPlayer[rnd], Quaternion.Euler(0, 0, 0));
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

        while (_StateIATeam == StateRespawn.WaitingForRespawn)
        {
            if (IARespawnRate <= IARespawnCooldown)
            {

                rnd = Random.Range(0, NbTotalIATank);
                IATank NewIATank = Instantiate(TankIAs, SpawnIA[rnd], Quaternion.Euler(0, 0, 0));
                NewIATank.transform.parent = PlayerTeam.transform;
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
        foreach(CaptureZone captureZone in ZoneCaptureList)
        {         
            if(captureZone._ZoneState == CaptureZone.ZoneState.CapturedPlayer)
            {
                _StateLevel = StateLevel.Victory;
            }
            else
            {
                _StateLevel = StateLevel.InGame;
            }
            if(captureZone._ZoneState == CaptureZone.ZoneState.CapturedAI) 
            {
                _StateLevel = StateLevel.Defeat;
            }
            else
            {
                _StateLevel = StateLevel.InGame;
            }
        }
        
        if(_GameTimer.Elapsed.TotalSeconds > MaxTimeGame)
        {
            _StateLevel = StateLevel.OutofTime;
        }      
    }
}