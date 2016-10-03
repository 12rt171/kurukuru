using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;



public class battle : MonoBehaviour
{
    //敵か味方か判別   現在未使用
    public enum my
    {
        ally = 0,
        enemy = 1
    }

    //キャラクター生成  名前は後で変える
    public character ch1 = new character();
    public character ch2 = new character();
    //キャンバス取得   UIの関係で必要
    GameObject canvas;
    //ボタン作成の為
    GameObject button1;
    GameObject button2;
    GameObject button3;

    // Use this for initialization
    void Start()
    {
        //canvas取得
        canvas = GameObject.Find("Canvas");
        //キャラクターの追加、characterdataをデータベースにしてそこからキャラを読み込む
        //敵味方、表示するアイコン、アイコンの縦の位置はこっちで書かないと駄目
        ch1.illear();
        ch1.force = (int)my.ally;   //現在未使用
        ch1.icon = Instantiate(Resources.Load("Prefabs/Icon"), new Vector2(ch1.wait, -30), Quaternion.identity) as GameObject;    //初期位置
        ch1.icon.transform.SetParent(canvas.transform, false);  //Canvasの子オブジェクトとして生成
        ch1.icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = ch1.currentlevel.ToString(); //初期レベルの表示
        //ch2
        ch2.lullshare();
        ch2.force = (int)my.ally;
        ch2.icon = Instantiate(Resources.Load("Prefabs/Icon2"), new Vector2(ch1.wait, -70), Quaternion.identity) as GameObject;    //初期位置
        ch2.icon.transform.SetParent(canvas.transform, false);  //Canvasの子オブジェクトとして生成
        ch2.icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = ch2.currentlevel.ToString(); //初期レベルの表示
        //本編開始
        StartCoroutine(coroutine());

    }

    // Update is called once per frame
    void Update()
    {
        //使わねぇ
    }

    IEnumerator coroutine()
    {
        while (true)
        {
            //誰も行動可能でないなら左に進める      後々全キャラをチェックするように変更
            /*if (ch1.wait >= 0)
            {
                ch1.movetoleft();
                yield return new WaitForSeconds(0.01f);     //ウェイト　結構適当
            }*/


            yield return StartCoroutine(checkallcharacter(ch1, ch2));     //複数キャラ分
            
            //行動可能な場合
            /*else if (ch1.wait <= 0)             //後々checkallcharacterに統合する
            {
                //行動可能キャラが複数いる場合は？
                //関数をよんで、終わるまで待つってのをやりたい
                yield return StartCoroutine(action(ch1));
                yield return 0;
            }
            if (ch2.wait <= 0)
            {
                //行動可能キャラが複数いる場合は？
                //関数をよんで、終わるまで待つってのをやりたい
                yield return StartCoroutine(action(ch2));
                yield return 0;
            }
             */
            yield return new WaitForSeconds(0.005f); 
            
        }
    }

    //行動
    IEnumerator action(character ch)
    {
        //ボタン生成
        button1 = Instantiate(Resources.Load("Prefabs/attack"), new Vector2(-160, 0), Quaternion.identity) as GameObject;
        //キャンバスの子オブジェクト化
        button1.transform.SetParent(canvas.transform, false);
        //ボタン入力の受付、使わなくなったら切らないと駄目?
        button1.GetComponent<Button>().onClick.AddListener(() =>
            {
                //攻撃時－固定値/素早さ
                ch.wait = 2000 / ch.agility;  //要調整
                ch.currentlevel = ch.level;   //現在のレベルを元々のレベルにする
                ch.icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = ch.currentlevel.ToString(); //テキスト変更    長い
                //使用済みのボタン  全部ﾌﾞｯｸｽすんだ
                Destroy(button1);
                Destroy(button2);
                Destroy(button3);
            });
        //以下略
        button2 = Instantiate(Resources.Load("Prefabs/cast"), new Vector2(0, 0), Quaternion.identity) as GameObject;
        button2.transform.SetParent(canvas.transform, false);
        button2.GetComponent<Button>().onClick.AddListener(() =>
        {
            ch.wait = 1000 / ch.agility;  //要調整   いっそのこと各キャラに固定値割り振るか
            ch.currentlevel += 2;          //現在のレベル+2
            ch.icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = ch.currentlevel.ToString();
            Destroy(button1);
            Destroy(button2);
            Destroy(button3);
        });
        button3 = Instantiate(Resources.Load("Prefabs/unison"), new Vector2(160, 0), Quaternion.identity) as GameObject;
        button3.transform.SetParent(canvas.transform, false);
        button3.GetComponent<Button>().onClick.AddListener(() =>
        {
                                            //しらね
            Destroy(button1);
            Destroy(button2);
            Destroy(button3);
        });
        while (button1 == true)
        {

            yield return 0;
        }
        yield return 0;

    }

    IEnumerator checkallcharacter(params character[] allcharacter)
    { 
        //回し用変数
        int i;
        int j;

        j = 0;
        //行動可能な奴を貯める変数
        character[] active = new character[allcharacter.Length];
        //行動可能か判定
        for (i = 0; i < allcharacter.Length; i++) {
            if (allcharacter[i].wait <= 0) {
                active[j] = allcharacter[i];
                j++;
            }
        
        }
        //配列のサイズを変更
        System.Array.Resize(ref active, j);
        //Debug.Log(active.Length.ToString());
        //もし行動可能配列が0 = 誰も行動出来ないなら左に動かす
        if (active.Length == 0) {

            for (i = 0; i < allcharacter.Length; i++)
            {
                allcharacter[i].movetoleft();

            }

            yield return 0;
        }

        if (active.Length == 1) {

            Debug.Log(active.Length.ToString());
            yield return StartCoroutine(action(active[0]));
            yield return 0;
        }

        yield return 0;
    }
}




public class character : characterdata  {

    public void movetoleft() {
        wait -= 5;
        icon.transform.localPosition = new Vector2((wait-200f)/*/(512f/Screen.width)*/, icon.transform.localPosition.y);
        //Debug.Log(name);         
        //Debug.Log(wait.ToString());               //デバッグ用

    }

}

public class characterdata {
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

    public void illear() {
        if (called != true) {
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
}


