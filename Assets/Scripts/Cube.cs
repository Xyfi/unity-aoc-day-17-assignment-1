using System.Collections.Generic;
using System;
using UnityEngine;

public class Cube {
    public Vector3Int gridPosition { get; private set; }
    public Vector3 worldPosition { get; private set; }

    private GameObject obj;
    private CubeBehaviour script;

    private string id;
    public List<Cube> neighbours { get; private set; }
    public int activeNeighours { get; private set; } = 0;

    public Dictionary<string,Vector3Int> noNeighbours = new Dictionary<string,Vector3Int>();

    private CubeRepository cubeRepository;
    private GameObject spawner;

    private bool? nextActiveState = null;

    public Cube(
        Vector3Int gridPosition,
        Vector3 worldPosition,
        GameObject prefab
    ) {
        this.spawner = GameObject.Find( "Spawner" );
        this.gridPosition = gridPosition;
        this.worldPosition = worldPosition;
        this.obj = GameObject.Instantiate( prefab, worldPosition, prefab.transform.rotation, spawner.transform );
        this.script = obj.GetComponent<CubeBehaviour>();
        this.script.data = this;
        id = ConvertVectorToId( gridPosition );
        cubeRepository = CubeRepository.Instance;
        cubeRepository.Add( this );
        neighbours = new List<Cube>();
        activeNeighours = 0;
        RegisterWithNeighbours();
    }

    private void RegisterWithNeighbours() {
        foreach ( var dir in Directions.GetAll() ) {
            var neighbourPosition = gridPosition + dir;
            var neightbourId = ConvertVectorToId( neighbourPosition );
            var possibleNeighbour = cubeRepository.Get( neightbourId );
            if ( possibleNeighbour == null ) {
                noNeighbours.Add( neightbourId, neighbourPosition );
                continue;
            }
            neighbours.Add( possibleNeighbour );
            if ( possibleNeighbour.IsActive() ) {
                activeNeighours++;
            }
            possibleNeighbour.Register( this );
        }
    }

    private void OnNeighbourActiveStateChanged( bool active ) {
        if ( active ) {
            activeNeighours++;
        } else {
            activeNeighours--;
        }
    }

    public void Register( Cube other ) {
        noNeighbours.Remove( other.id );
        neighbours.Add( other );
        if ( other.IsActive() ) {
            activeNeighours++;
        }
    }

    public void SetActive( bool active ) {
        bool stateChanged = active != this.script.IsActive();
        this.script.SetActive( active );
        if ( stateChanged ) {
            foreach ( Cube neighbour in neighbours ) {
                neighbour.OnNeighbourActiveStateChanged( active );
            }
        }
    }

    public void SetNextActiveState( bool active ) {
        this.nextActiveState = active;
    }

    public void ApplyNextState() {
        if ( nextActiveState != null ) {
            this.SetActive( (bool) nextActiveState );
            nextActiveState = null;
        }
    }

    public bool IsActive() {
        return this.script.IsActive();
    }

    public override string ToString()
    {
        return id;
    }

    public static string ConvertVectorToId( Vector3Int v ) {
        return  $"{ v.x }|{ v.y }|{ v.z }";
    }
}