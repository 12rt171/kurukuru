using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;
using character;
using party;



public class battle : MonoBehaviour
{
    //敵か味方か判別   現在未使用
    public enum my {
        ally = 0,
        enemy = 1
    }
    //行動判別 selectactionで使用
    public enum act
    {
        none = 0,
        attack = 1,
        cast = 2,
        unison = 3
    }
    public int stat;

    //キャラクター読み込み用
    party.partymember partyinfo;
    public character.character[] ally = new character.character[2];
    int numberofallys;
    
    public character.character[] enemy = new character.character[2];
    int numberofenemys = 2;

    //戦闘に参加する全キャラ保管
    character.character[] allcharacter = new character.character[1];
    int i;
    int j;

    //攻撃相手取得用
    character.character target;
    GameObject targeticon;
    public GameObject[] enemyicon = new GameObject[2];

    //予想される行動後の位置
    public GameObject expectedposition;
    GameObject expectedknockback;

    //キャンバス取得   UIの関係で必要
    GameObject canvas;

    //ボタン作成の為
    GameObject attackbutton;
    GameObject castbutton;
    GameObject unisonbutton;

    GameObject previous;
    GameObject next;
    GameObject attacktarget;

    // Use this for initialization
    void Start() {
        //unityにsceneを跨いで存在するpartyinfoを作ってそこから読み出し
        partyinfo = GameObject.Find("partyinfo").GetComponent<partymember>();
        numberofallys = partyinfo.ally.Length;
        //敵味方初期化
        for (i = 0; i < numberofallys; i++) {
            ally[i] = partyinfo.ally[i];
        }
        //現状敵は新しく作るしかない
        for (i = 0; i < numberofenemys; i++) {
            enemy[i] = new character.character();
        }

        //canvas取得
        canvas = GameObject.Find("Canvas");
        //キャラクターの追加、characterdataをデータベースにしてそこからキャラを読み込む
        //敵味方、表示するアイコン、アイコンの縦の位置はこっちで書かないと駄目
        //味方1
/*        ally[0].illear();
        ally[0].force = (int)my.ally;   //現在未使用
        ally[0].icon = Instantiate(Resources.Load("Prefabs/Icon"), new Vector2(ally[0].wait, -30), Quaternion.identity) as GameObject;    //初期位置
        ally[0].icon.transform.SetParent(canvas.transform, false);  //Canvasの子オブジェクトとして生成
        ally[0].icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = ally[0].currentlevel.ToString(); //初期レベルの表示
        //味方2
        ally[1].lullshare();
        ally[1].force = (int)my.ally;
        ally[1].icon = Instantiate(Resources.Load("Prefabs/Icon2"), new Vector2(ally[1].wait, -70), Quaternion.identity) as GameObject;    //初期位置
        ally[1].icon.transform.SetParent(canvas.transform, false);  //Canvasの子オブジェクトとして生成
        ally[1].icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = ally[1].currentlevel.ToString(); //初期レベルの表示
*/
        //緊急対応　味方のアイコン付ける
        ally[0].icon = Instantiate(Resources.Load("Prefabs/Icon"), new Vector2(ally[0].wait, -30), Quaternion.identity) as GameObject;    //初期位置
        ally[0].icon.transform.SetParent(canvas.transform, false);  //Canvasの子オブジェクトとして生成
        ally[0].icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = ally[0].currentlevel.ToString(); //初期レベルの表示
        ally[1].icon = Instantiate(Resources.Load("Prefabs/Icon2"), new Vector2(ally[1].wait, -70), Quaternion.identity) as GameObject;    //初期位置
        ally[1].icon.transform.SetParent(canvas.transform, false);  //Canvasの子オブジェクトとして生成
        ally[1].icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = ally[1].currentlevel.ToString(); //初期レベルの表示
 
        
        //敵1
        enemy[0].azel();
        enemy[0].force = (int)my.enemy;
        enemy[0].icon = Instantiate(Resources.Load("Prefabs/Icon3"), new Vector2(enemy[0].wait, 50), Quaternion.identity) as GameObject;    //初期位置
        enemy[0].icon.transform.SetParent(canvas.transform, false);  //Canvasの子オブジェクトとして生成
        enemy[0].icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = enemy[0].currentlevel.ToString(); //初期レベルの表示
        enemy[1].synn();
        enemy[1].force = (int)my.enemy;
        enemy[1].icon = Instantiate(Resources.Load("Prefabs/Icon4"), new Vector2(enemy[0].wait, 90), Quaternion.identity) as GameObject;    //初期位置
        enemy[1].icon.transform.SetParent(canvas.transform, false);  //Canvasの子オブジェクトとして生成
        enemy[1].icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = enemy[0].currentlevel.ToString(); //初期レベルの表示
        
        //全キャラリストに味方記入
        for (i = 0; i < numberofallys; i++) {
            System.Array.Resize(ref allcharacter, i+1);
            allcharacter[i] = ally[i];
        }
        //全キャラリストに敵記入
        for (j = 0; j < numberofenemys; j++)
        {
            System.Array.Resize(ref allcharacter, i + j + 1);
            allcharacter[i+j] = enemy[j];
        }

        //本編開始
        StartCoroutine(coroutine());
    }

    // Update is called once per frame
    void Update() {
        //使わねぇ
    }

    IEnumerator coroutine() {
        while (true) {

            //行動可能か確認
            yield return StartCoroutine(checkallcharacter());     //複数キャラ分

            //yield return new WaitForSeconds(0.005f); //ウェイト
        }
    }

    IEnumerator checkallcharacter()
    {
        //回し用変数
        j = 0;
        //行動可能な奴を貯める変数
        character.character[] active = new character.character[allcharacter.Length];
        //行動可能か判定
        for (i = 0; i < allcharacter.Length; i++)
        {
            if (allcharacter[i].wait <= 0)
            {
                active[j] = allcharacter[i];
                j++;
            }
        }
        //配列のサイズを変更
        System.Array.Resize(ref active, j);

        //もし行動可能配列が0 = 誰も行動出来ないなら左に動かす
        if (active.Length == 0)
        {
            for (i = 0; i < allcharacter.Length; i++)
            {
                movetoleft(allcharacter[i]);
            }
            yield return 0;
        }
        //行動可能が一人のみ
        if (active.Length == 1)
        {
            yield return StartCoroutine(selectaction(active[0]));
            yield return 0;
        }
        //行動可能が複数
        if (active.Length > 1)
        {
            yield return StartCoroutine(selectaction(active));
            yield return 0;
        }
        yield return 0;
    }

    void movetoleft(character.character nonactivecharacter) {
        nonactivecharacter.wait -= 5;
        nonactivecharacter.icon.transform.localPosition = new Vector2((nonactivecharacter.wait-200f), nonactivecharacter.icon.transform.localPosition.y);
        //Debug.Log(name);         
        //Debug.Log(wait.ToString());               //デバッグ用
    }

    //行動
    IEnumerator selectaction(params character.character[] activecharacter)
    {
        //ボタン生成
        attackbutton = Instantiate(Resources.Load("Prefabs/attack"), new Vector2(-160, 0), Quaternion.identity) as GameObject;
        //キャンバスの子オブジェクト化
        attackbutton.transform.SetParent(canvas.transform, false);
        //ボタン入力の受付、使わなくなったら切らないと駄目?
        attackbutton.GetComponent<Button>().onClick.AddListener(() =>
            {
                stat = (int)act.attack;
                //使用済みのボタン  全部ﾌﾞｯｸｽすんだ
                Destroy(attackbutton);
                Destroy(castbutton);
                Destroy(unisonbutton);
            });
        //以下略
        castbutton = Instantiate(Resources.Load("Prefabs/cast"), new Vector2(0, 0), Quaternion.identity) as GameObject;
        castbutton.transform.SetParent(canvas.transform, false);
        castbutton.GetComponent<Button>().onClick.AddListener(() =>
        {
            stat = (int)act.cast;
            //cast(activecharacter);
            Destroy(attackbutton);
            Destroy(castbutton);
            Destroy(unisonbutton);
        });
        unisonbutton = Instantiate(Resources.Load("Prefabs/unison"), new Vector2(160, 0), Quaternion.identity) as GameObject;
        unisonbutton.transform.SetParent(canvas.transform, false);
        unisonbutton.GetComponent<Button>().onClick.AddListener(() =>
        {
                                            //しらね
            Destroy(attackbutton);
            Destroy(castbutton);
            Destroy(unisonbutton);
        });

        expectedposition = Instantiate(activecharacter[0].icon, new Vector2(2000 / activecharacter[0].agility - 200, activecharacter[0].icon.transform.localPosition.y), Quaternion.identity) as GameObject;
        expectedposition.transform.SetParent(canvas.transform, false);
        expectedposition.gameObject.transform.FindChild("icon").GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.5f); 

        stat = 0;
        while (stat != -1)
        {
            switch (stat)
            {

                case 0:
                    yield return 0;
                    break;
                case 1:
                    yield return StartCoroutine(selecttarget());
                    for (int x = 0; x < activecharacter.Length; x++)
                    {
                        yield return StartCoroutine(attack(activecharacter[x]));
                        yield return new WaitForSeconds(1); //ウェイト
                    }
                        stat = -1;
                    break;
                case 2:
                    for (int x = 0; x < activecharacter.Length; x++)
                    {
                        yield return StartCoroutine(cast(activecharacter[x]));
                    }
                    stat = -1;
                    break;

            }
        }
        Destroy(expectedposition);
        yield return 0;
    }

    IEnumerator selecttarget() {

        bool targetselected = false;


        target = enemy[0];
        targeticon = Instantiate(enemyicon[0], new Vector2(150, 0), Quaternion.identity) as GameObject;
        targeticon.transform.SetParent(canvas.transform, false);  //Canvasの子オブジェクトとして生成
        int x = 0; ;
        previous = Instantiate(Resources.Load("Prefabs/previous"), new Vector2(100, -50), Quaternion.identity) as GameObject;
        previous.transform.SetParent(canvas.transform, false);
        previous.GetComponent<Button>().onClick.AddListener(() =>
        {
            x += numberofenemys -1;
            x = x % numberofenemys;
            target = enemy[x];
            Destroy(targeticon);
            targeticon = Instantiate(enemyicon[x], new Vector2(150, 0), Quaternion.identity) as GameObject;
            targeticon.transform.SetParent(canvas.transform, false);  //Canvasの子オブジェクトとして生成
        });
        //以下略
        next = Instantiate(Resources.Load("Prefabs/next"), new Vector2(200, -50), Quaternion.identity) as GameObject;
        next.transform.SetParent(canvas.transform, false);
        next.GetComponent<Button>().onClick.AddListener(() =>
        {
            x += numberofenemys +1;
            x = x % numberofenemys;
            target = enemy[x];
            Destroy(targeticon);
            targeticon = Instantiate(enemyicon[x], new Vector2(150, 0), Quaternion.identity) as GameObject;
            targeticon.transform.SetParent(canvas.transform, false);  //Canvasの子オブジェクトとして生成
         }); 
        attacktarget = Instantiate(Resources.Load("Prefabs/attack"), new Vector2(150, -150), Quaternion.identity) as GameObject;
        attacktarget.transform.SetParent(canvas.transform, false);
        attacktarget.GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(targeticon);
            Destroy(next);
            Destroy(previous);
            Destroy(attacktarget);
            targetselected = true;
        });
        while (targetselected == false) {
            
            yield return 0;
        }
        yield return 0;
    }

    IEnumerator attack(character.character activecharacter)
    {
        //Debug.Log(target.wait.ToString());
        int damage =  activecharacter.attack * activecharacter.currentlevel;


        //攻撃時－固定値/素早さ
        this.gameObject.transform.FindChild("Text").GetComponent<Text>().text =damage.ToString();
        activecharacter.wait = 2000 / activecharacter.agility;  //要調整
        activecharacter.currentlevel = activecharacter.level;   //現在のレベルを元々のレベルにする
        activecharacter.icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = activecharacter.currentlevel.ToString(); //テキスト変更    長い
        target.wait += damage;
        yield return 0;
    }

    IEnumerator cast(character.character activecharacter)
    {
        activecharacter.wait = 1000 / activecharacter.agility;  //要調整   いっそのこと各キャラに固定値割り振るか
        activecharacter.currentlevel += 2;          //現在のレベル+2
        activecharacter.icon.gameObject.transform.FindChild("Text").gameObject.GetComponent<Text>().text = activecharacter.currentlevel.ToString();
        yield return 0;
    }

}
/*
public class character : characterdata  {

    public void movetoleft() {
        wait -= 5;
        icon.transform.localPosition = new Vector2((wait-200f), icon.transform.localPosition.y);
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
    public GameObject icon2;
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


*/