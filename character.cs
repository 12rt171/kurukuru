using UnityEngine;
using System.Collections;


namespace character {
    public class character  {

  /*  public void movetoleft() {
        wait -= 5;
        icon.transform.localPosition = new Vector2((wait-200f), icon.transform.localPosition.y);
        //Debug.Log(name);         
        //Debug.Log(wait.ToString());               //デバッグ用
    }*/
        public string name;
        public int level;
        public int currentlevel;
        public int attack;
        public int deffence;
        public int agility;
        public int wait;
        public GameObject icon;
        public int force;//敵か味方か

        private bool called = false;

        public void illear()
        {
            if (called != true)
            {
                name = "illear";
                level = 5;
                currentlevel = 5;
                attack = 10;
                deffence = 3;
                agility = 7;
                wait = 100;
                called = true;
            }
        }

        public void lullshare()
        {
            if (called != true)
            {
                name = "lullshare";
                level = 4;
                currentlevel = 4;
                attack = 10;
                deffence = 3;
                agility = 5;
                wait = 150;
                called = true;
            }
        }

        public void azel()
        {
            if (called != true)
            {
                name = "azel";
                level = 7;
                currentlevel = 7;
                attack = 10;
                deffence = 3;
                agility = 1;
                wait = 500;
                called = true;
            }
        }

        public void synn()
        {
            if (called != true)
            {
                name = "synn";
                level = 7;
                currentlevel = 7;
                attack = 10;
                deffence = 3;
                agility = 1;
                wait = 600;
                called = true;
            }
        }
    }
}