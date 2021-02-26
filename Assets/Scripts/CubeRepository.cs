using System.Collections;
using System.Collections.Generic;
public class CubeRepository : IEnumerable<Cube> {
    private Dictionary<string,Cube> cubes;
    private CubeRepository() {
        cubes = new Dictionary<string,Cube>();
    }

    public void Add( Cube cube ) {
        cubes.Add( cube.ToString(), cube );
    }

    public Cube Get( string id ) {
        if ( cubes.ContainsKey( id ) ) {
            return cubes[ id ];
        }
        return null;
    }

    public IEnumerator<Cube> GetEnumerator()
    {
        foreach ( var cube in cubes ) {
            yield return cube.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static CubeRepository instance = null;
    public static CubeRepository Instance
    {
        get
        {
            if ( instance == null ) {
                instance = new CubeRepository();
            }
            return instance;
        }
    }
}