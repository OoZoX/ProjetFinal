
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
    private StateLevel _StateLevel;
    // Start is called before the first frame update
    void Start()
    {   
        foreach (Tank tank in IATankList)
        {
          // SpawnIA.Add(tank.gameObject.transform.position);
        }        
        foreach (Tank tank in PlayerTankList)
        {
           // SpawnPlayer.Add(tank.gameObject.transform.position);
        }
        NbTotalIATank = IATankList.Count();
        NbTotalPlayerTank = PlayerTankList.Count();
        _GameTimer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        ManageTankRespawn();
        EndGameVerification();

    }

    public void ManageTankRespawn()
    {
        if(IARespawnRate >= IARespawnCooldown)
        {
            if (IATankList.Count < NbTotalIATank)
            {
                IATank NewTank = new IATank();
                NewTank.transform.position = SpawnIA[Mathf.RoundToInt(NbTotalIATank)];
            }
            IARespawnCooldown = 0;
        }
        else
        {
            IARespawnCooldown = IARespawnCooldown + Time.deltaTime;
        }

        if (PlayerRespawnRate >= PlayerRespawnCooldown)
        {
            if (PlayerTankList.Count < NbTotalPlayerTank)
            {
                PlayerTank NewTank = new PlayerTank();
                rnd = Random.Range(0, NbTotalPlayerTank);
                NewTank.transform.position = SpawnPlayer[rnd];
            }
            PlayerRespawnCooldown = 0;
        }
        else
        {
            PlayerRespawnCooldown = PlayerRespawnCooldown + Time.deltaTime;
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
        /*
        if(_GameTimer.Elapsed.TotalSeconds > MaxTimeGame)
        {
            _StateLevel = StateLevel.OutofTime;
        }
        */
    }
}
