using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemDataBase : MonoBehaviour {
 
    //　アイテムの種類
    public enum ItemType {
        Wood,
        himo,
        branch,

        vine,

        leaf,

        watch,

        transparent,

        scapegoat,

        catcheye,

        dan,

        stone,

        Other

    }
    //　アイテムデータのリスト
    public List<ItemData> itemDataList = new List<ItemData>(); 
 
    void Awake() {
        //　アイテムの全情報を作成
        itemDataList.Add(new ItemData(Resources.Load("Wood", typeof(Sprite)) as Sprite, "木材", ItemType.Wood, "ツリーハウスを直すことができる"));
        itemDataList.Add(new ItemData(Resources.Load("rope", typeof(Sprite)) as Sprite, "ひも", ItemType.himo, "ツリーハウスを直すことができる"));
        itemDataList.Add(new ItemData(Resources.Load("branch", typeof(Sprite)) as Sprite, "枝", ItemType.branch, "ツリーハウスを直すことができる"));
        itemDataList.Add(new ItemData(Resources.Load("vine", typeof(Sprite)) as Sprite, "つる", ItemType.vine, "ツリーハウスを直すことできる"));
        itemDataList.Add(new ItemData(Resources.Load("leaf", typeof(Sprite)) as Sprite, "葉", ItemType.leaf, "ギリースーツを作成できる"));
        itemDataList.Add(new ItemData(Resources.Load("watch", typeof(Sprite)) as Sprite, "腕時計", ItemType.watch, "残り時間が分かる"));
        itemDataList.Add(new ItemData(Resources.Load("transparent", typeof(Sprite)) as Sprite, "透明化", ItemType.transparent, "３０秒透明になる"));
        itemDataList.Add(new ItemData(Resources.Load("scapegoat", typeof(Sprite)) as Sprite, "スケープゴート", ItemType.scapegoat, "残り時間が分かる"));
        itemDataList.Add(new ItemData(Resources.Load("catcheye", typeof(Sprite)) as Sprite, "キャッチアイ", ItemType.catcheye, "残り時間が分かる"));
        itemDataList.Add(new ItemData(Resources.Load("dan", typeof(Sprite)) as Sprite, "段ボール", ItemType.dan, "持ち運び隠れられる"));
        itemDataList.Add(new ItemData(Resources.Load("stone", typeof(Sprite)) as Sprite, "石", ItemType.stone, "投げることで敵の意識がそちらに向く"));
 
    }
    //　全アイテムデータを返す
    public List<ItemData> GetItemDataList() {
        return itemDataList;
    }
    //　個々のアイテムデータを返す
    public ItemData GetItemData(string itemName) {
        foreach (var item in itemDataList) {
            if(item.GetItemName() == itemName) {
                return item;
            }
        }
        return null;
    }
}