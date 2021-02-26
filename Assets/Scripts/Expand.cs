using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Expand : MonoBehaviour
{

    public GameObject cube;

    public float distance = 1.5f;

    private bool initialized = false;

    private int steps = 0;

    private CubeRepository cubes;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        cubes = CubeRepository.Instance;

        InputField field = GameObject.Find( "Input" ).GetComponent<InputField>();
        field.text = "..#..#..\n.###..#.\n#..##.#.\n#.#.#.#.\n.#..###.\n.....#..\n#...####\n##....#.";
        Button button = GameObject.Find( "Start" ).GetComponent<Button>();
        button.onClick.AddListener( Initialize );
    }

    void Initialize() {
        InputField field = GameObject.Find( "Input" ).GetComponent<InputField>();
        Canvas canvas = (Canvas) field.transform.parent.GetComponent<Canvas>();
        string txt_input = field.text;
        canvas.enabled = false;

        string[] lines = txt_input.Split( '\n' ).Select( str => str.Trim() ).ToArray();

        int height = lines.Length;
        int width = lines[ 0 ].Length;
        // Start top left.
        offset = new Vector3( - ( width * distance ) / 2, ( height * distance ) / 2, 0 );

        for ( int row = 0; row < lines.Length; row++ ) {
            for ( int column = 0; column < lines[ row ].Length; column++ ) {
                SpawnCube( new Vector3Int( column, row, 0 ), lines[ row ][ column ] == '#' );
            }
        }

        Button button = GameObject.Find( "NextStepButton" ).GetComponent<Button>();
        button.transform.parent.GetComponent<Canvas>().enabled = true;
        button.onClick.AddListener( Step );

        initialized = true;
    }

    public void Step() {
        GenerateInactiveCubes();
        DetermineNextState();
        ApplyNextState();
        steps++;
        
        GameObject.Find( "Step" ).GetComponent<Text>().text = "Step: " + steps;
        GameObject.Find( "ActiveCubes" ).GetComponent<Text>().text = "Active cubes: " + GameObject.FindGameObjectsWithTag( "activeCube" ).Length;
    }

    void GenerateInactiveCubes() {
        GameObject[] activeCubes = GameObject.FindGameObjectsWithTag( "activeCube" );
        foreach ( GameObject activeCube in activeCubes ) {
            Cube c = activeCube.GetComponent<CubeBehaviour>().data;
            foreach ( Vector3Int empty in c.noNeighbours.Values.ToArray() ) {
                if ( cubes.Get( Cube.ConvertVectorToId( empty ) ) == null ) {
                    SpawnCube( empty );
                }
            }
        }
    }

    void DetermineNextState() {
        foreach ( Cube cube in cubes ) {
            if ( cube.IsActive() && ! ( cube.activeNeighours == 2 || cube.activeNeighours == 3 ) ) {
                cube.SetNextActiveState( false );
            }
            if ( ! cube.IsActive() && cube.activeNeighours == 3 ) {
                cube.SetNextActiveState( true );
            }
        }
    }

    void ApplyNextState() {
        foreach ( Cube cube in cubes ) {
            cube.ApplyNextState();
        }
    }

    void SpawnCube( Vector3Int gridPosition, bool active = false ) {
        Vector3 cubePosition = offset + new Vector3( gridPosition.x * distance, gridPosition.y * -distance, gridPosition.z * distance );
        Cube cube = new Cube( gridPosition, cubePosition, this.cube );
        cube.SetActive( active );
    }
}
