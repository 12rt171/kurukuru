using UnityEngine;
using System.Collections;
using character;

namespace party
{

    public class partymember : MonoBehaviour
    {

        const int numberofallys = 2;

        public character.character[] ally = new character.character[2];

        // Use this for initialization
        void Start()
        {
            for (int i = 0; i < numberofallys; i++)
            {
                ally[i] = new character.character();
            }
            ally[0].illear();   //現在未使用
        //味方2
            ally[1].lullshare();
 
        }

        void addtoparty()
        {

        }

        void deletefromparty()
        {

        }

        void leaveparty()
        {

        }

    }
}