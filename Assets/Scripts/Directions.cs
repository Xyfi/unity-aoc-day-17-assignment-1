using System;
using UnityEngine;

public class Directions
{    
    private static int[] axis = new int[] { -1, 0, 1 };

    private static Vector3Int[] directions = null;
    public static Vector3Int[] GetAll() {
        if ( directions != null ) {
            return directions;
        }

        directions = new Vector3Int[ (int) Mathf.Pow( 3, 3 ) - 1 ];
        int index = 0;

        foreach ( int x in axis ) {
            foreach ( int z in axis ) {
                foreach ( int y in axis ) {
                    if ( x == 0 && z == 0 && y == 0 ) {
                        continue;
                    }
                    directions[ index ] = new Vector3Int( x, y, z );
                    index++;
                }
            }
        }

        return directions;
    }
}
